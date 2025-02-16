using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CloudCommerceFunctionSuite.Functions
{
    public static class TimerDataCleanupFunction
    {
        [FunctionName("TimerDataCleanup")]
        public static async Task Run(
            [TimerTrigger("0 0 3 * * *", RunOnStartup = true)] TimerInfo timer, // Runs daily at 3:00 AM UTC
            ILogger log)
        {
            log.LogInformation($"TimerDataCleanupFunction triggered at: {DateTime.UtcNow}");

            try
            {
                await CleanupOldRecordsAsync(log);

                log.LogInformation("Cleanup completed successfully.");
            }
            catch (Exception ex)
            {
                log.LogError($"Error during cleanup: {ex.Message}");
            }
        }

        private static Task CleanupOldRecordsAsync(ILogger log)
        {
            // Simulate data cleanup logic

            int i = 0;

            while (i < 10)
            {
                // Example: Delete old logs, archive data, remove expired sessions, etc.
                i++;
                //Thread.Sleep(200);
                log.LogInformation($"file {i} deleted successfully.");

            }

            return Task.CompletedTask;
        }
    }
}
