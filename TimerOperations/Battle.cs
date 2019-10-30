using System;
using System.Linq;
using System.Threading.Tasks;
using mdaWar.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace mdaWar
{
    public class Battle
    {
        private readonly Context context;
        private readonly BattleHelper battleHelper;

        public Battle(Context context, BattleHelper battleHelper)
        {
            this.context = context;
            this.battleHelper = battleHelper;
        }

        [FunctionName("Battle")]
        public async Task Run([TimerTrigger("0 0 8 * * 1-5")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("Starting new battle round");

            var warList = await this.context.Wars.Include( x => x.Participants).Where( x => !x.Finished).ToListAsync();

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);

            foreach (var war in warList)
            {
                var participants = war.Participants.ToList();

                var battleResult = this.battleHelper.SimulateEngagement(participants);

                var msg = new SendGridMessage();

                var sendTo = participants.Select(x => new EmailAddress
                {
                    Email = x.Email,
                    Name = x.Name
                }).ToList();

                msg.SetFrom(new EmailAddress("javimollamico@gmail.com", "MDA Warbot Team"));
                msg.AddTos(sendTo);
                msg.SetSubject($"{war.Name} results at {DateTime.UtcNow.Date}");
                msg.AddContent(MimeType.Text, battleResult);

                var response = await client.SendEmailAsync(msg);

                await this.context.SaveChangesAsync();
            }
        }
    }
}
