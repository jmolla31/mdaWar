using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace mdaWar
{
    public static class QuickWar
    {
        [FunctionName("QuickWar")]
        public static async Task<IActionResult> CreateQuickWar(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new QuickWar");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var participants = JsonConvert.DeserializeObject<string[]>(requestBody);

            if (participants == null || participants.Length < 2) return new BadRequestObjectResult("Please upload a list with a least 2 participants");

            var weapons = new string[] { "watergun", "butter knife", "rusted candlestick", "toy rifle", "posioned potato", "broken Duff bottle",
                                        "BelleDelphine bathwater jar", "Avril Lavigne marble sculpture", "area 51 stolen raygun"};

            var battles = new List<string>();
            var aliveParticipants = participants.ToList();

            Random random = new Random();

            while (aliveParticipants.Count > 1)
            {
                var lives = random.Next(aliveParticipants.Count);

                var dies = 0;
                do
                {
                    dies = random.Next(aliveParticipants.Count);
                } while (lives == dies);

                var weapon = weapons[random.Next(weapons.Length - 1)];

                battles.Add($"{aliveParticipants[lives]} kills {aliveParticipants[dies]} with a {weapon}");

                aliveParticipants.RemoveAt(dies);
            }

            battles.Add($"Finally, the last survivor is {aliveParticipants[0]}");

            return new OkObjectResult(battles);
        }

        [FunctionName("QuickWarHelp")]
        public static IActionResult Help(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Processing QuickWar help request.");

            var list = new List<string>()
            {
                "Welcome to the QuickWar endpoint help",
                "To quickly simulate a war please send a POST request with a list of names for the contenders",
                "The QuickWar endpoint will return the winner and a list of the battles ocurred in the war"
            };

            return new OkObjectResult(list);
        }
    }
}
