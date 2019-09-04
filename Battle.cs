using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace mdaWar
{
    public static class Function1
    {
        [FunctionName("Battle")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"**NOT IMPLEMENTED** Another battle begins at: {DateTime.Now}");

            // TODO: Implement this
        }
    }
}
