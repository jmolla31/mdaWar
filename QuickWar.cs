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
using mdaWar.Helpers;

namespace mdaWar
{
    public class QuickWar
    {
        private readonly BattleHelper battleHelper;

        public QuickWar(BattleHelper battleHelper)
        {
            this.battleHelper = battleHelper;
        }

        [FunctionName("QuickWar")]
        public async Task<IActionResult> CreateQuickWar(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route ="QuickWar")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new QuickWar");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var participants = JsonConvert.DeserializeObject<string[]>(requestBody);

            if (participants == null || participants.Length < 2) return new BadRequestObjectResult("Please upload a list with a least 2 participants");

            var battles = new List<string>();
            var aliveParticipants = participants.ToList();

            while (aliveParticipants.Count > 1)
            {
                battles.Add(this.battleHelper.SimulateEngagement(aliveParticipants));
            }

            battles.Add($"Finally, the last survivor is {aliveParticipants[0]}");

            return new OkObjectResult(battles);
        }

        [FunctionName("QuickWarHelp")]
        public IActionResult Help(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "QuickWar")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Processing QuickWar help request.");

            var list = new List<string>()
            {
                "Welcome to the QuickWar endpoint help",
                "To quickly simulate a war please send a POST request with a list of names for the contenders",
                "Body JSON example: [\"JOHNDOE1\",\"JOHNDOE2\",\"JOHNDOE3\",\"JOHNDOE4\"]",
                "The QuickWar endpoint will return the winner and a list of the battles ocurred in the war"
            };

            return new OkObjectResult(list);
        }
    }
}
