using System;
using System.Linq;
using System.Threading.Tasks;
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

        public Battle(Context context)
        {
            this.context = context;
        }

        [FunctionName("Battle")]
        public async Task Run([TimerTrigger("0 0 8 * * 1-5")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("Starting new battle round.");

            var warList = await this.context.Wars.Include( x => x.Participants).Where( x => !x.Finished).ToListAsync();

            var weapons = new string[] { "watergun", "butter knife", "rusted candlestick", "toy rifle", "posioned potato", "broken Duff bottle",
                                        "BelleDelphine bathwater jar", "Avril Lavigne marble sculpture", "area 51 stolen raygun", "Outsystems manual"};

            Random random = new Random();

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);

            foreach (var war in warList)
            {
                var participants = war.Participants.ToList();

                var lives = random.Next(participants.Count);

                var dies = 0;
                do
                {
                    dies = random.Next(participants.Count);
                } while (lives == dies);

                var weapon = weapons[random.Next(weapons.Length - 1)];

                var msg = new SendGridMessage();

                msg.SetFrom(new EmailAddress("javimollamico@gmail.com", "MDA Warbot Team"));

                var sendTo = participants.Select(x => new EmailAddress
                {
                    Email = x.Email,
                    Name = x.Name
                }).ToList();

                msg.AddTos(sendTo);


                msg.SetSubject($"{war.Name} results at {DateTime.UtcNow.Date}");

                msg.AddContent(MimeType.Text, $"{participants[lives].Name} kills {participants[dies].Name} with a {weapon}");

                var response = await client.SendEmailAsync(msg);

                participants[dies].Alive = false;

                this.context.Update(participants[dies]);

                await this.context.SaveChangesAsync();
            }
        }
    }
}
