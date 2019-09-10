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

namespace mdaWar
{
    public class FullWar
    {
        private readonly Context context;

        public FullWar(Context context)
        {
            this.context = context;
        }

        [FunctionName("FullWar")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var duh = await this.context.Wars.ToListAsync();

            log.LogInformation("Starting new battle round.");

            return new OkObjectResult($"Hello");
        }
    }
}
