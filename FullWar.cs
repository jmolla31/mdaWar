using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using mdaWar.Models;

namespace mdaWar
{
    public class FullWar
    {
        private readonly Context context;

        public FullWar(Context context)
        {
            this.context = context;
        }

        [FunctionName("FullWarSingleRun")]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating a new FullWar");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var requestModel = JsonConvert.DeserializeObject<WarCreateModel>(requestBody);

            if (string.IsNullOrEmpty(requestModel.Name) || requestModel.Participants == null ||requestModel.Participants.Count() < 2)
            {
                return new BadRequestObjectResult("Malformed request body");
            }

            var war = new War
            {
                Name = requestModel.Name
            };

            await this.context.AddAsync(war);

            var participants = requestModel.Participants.Select(x => new Participant
            {
                Email = x.Email,
                Name = x.Name
            });

            await this.context.AddRangeAsync(participants);
        }

        [FunctionName("FullWarSingleRun")]
        public async Task<IActionResult> SingleRun(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Starting new battle round.");

            var warList = await this.context.Wars.ToListAsync();

            var weapons = new string[] { "watergun", "butter knife", "rusted candlestick", "toy rifle", "posioned potato", "broken Duff bottle",
                                        "BelleDelphine bathwater jar", "Avril Lavigne marble sculpture", "area 51 stolen raygun", "Outsystems manual"};

            Random random = new Random();

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);

            foreach (var war in warList)
            {
                var participants = await this.context.Participants.Where(x => x.WarId == war.Id).ToListAsync();

                var lives = random.Next(participants.Count);

                var dies = 0;
                do
                {
                    dies = random.Next(participants.Count);
                } while (lives == dies);

                var weapon = weapons[random.Next(weapons.Length - 1)];

                var msg = new SendGridMessage();

                msg.SetFrom(new EmailAddress("dx@example.com", "SendGrid DX Team"));

                var sendTo = participants.Select(x => new EmailAddress
                {
                    Email = x.Email,
                    Name = x.Name
                }).ToList();
                msg.AddTos(sendTo);

                msg.SetSubject($"{war.Name} results at {DateTime.UtcNow.Date}");

                msg.AddContent(MimeType.Text, $"{participants[lives]} kills {participants[dies]} with a {weapon}");

                var response = await client.SendEmailAsync(msg);

                this.context.Remove(participants[dies]);

                await this.context.SaveChangesAsync();
            }


            return new OkObjectResult($"Hello");
        }

        [FunctionName("FullWarHelp")]
        public static IActionResult Help(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Processing FullWar help request.");


            var bodyExample = new WarCreateModel
            {
                Name = "Example name",
                Participants = new List<ParticipantModel>
                {
                    new ParticipantModel { Name = "Participant1", Email = "participant1@gmail.com"},
                    new ParticipantModel { Name = "Participant2", Email = "participant2@gmail.com"},
                    new ParticipantModel { Name = "Participant3", Email = "participant3@gmail.com"}
                }
            };

            var list = new List<string>()
            {
                "Welcome to the FullWar endpoint help",
                "To start a war please send a POST request with a the war name and a list of participants for the contenders",
                $"Body JSON example: {JsonConvert.SerializeObject(bodyExample)}",
                "The war will be generated and the next battle will ocurr a the next 8:00AM UTC trigger"
            };

            return new OkObjectResult(list);
        }
    }
}
