
using Microsoft.Extensions.Options;
using Sentinel.ConnectionChecks;
using Sentinel.ConnectionChecks.Models;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Interfaces.Managers;

namespace DaaS.UI.Blazor.Background
{
    public class CronTrigger : BackgroundService
    {
        

        private readonly ICronTickerManager<CronTickerEntity> _cronTickerManager;

        public CronTrigger(ICronTickerManager<CronTickerEntity> cronTickerManager, IOptions<HealthCheckCronRequest> healthcheck)
        {
            _cronTickerManager = cronTickerManager;
            if (healthcheck.Value != null)
            {
                Console.WriteLine("CronTrigger initialized with HealthCheckCronRequest settings.");
            }
        }

        public async Task ScheduleJob()
        {
            var result = await _cronTickerManager.AddAsync(new CronTickerEntity
            {
                Function = "TestJob",
                Description =  "1234 ",
                Expression = "0 */5 * * * *",
                

                //ExecutionTime = DateTime.UtcNow.AddSeconds(10) // Run in 10 seconds
            });

            var result2 = await _cronTickerManager.AddAsync(new CronTickerEntity
            {
                Function = "TestJob",
                Description = "12345",
                Expression = "0 */5 * * * *",


                //ExecutionTime = DateTime.UtcNow.AddSeconds(10) // Run in 10 seconds
            });

            if (result.IsSucceeded)
            {
                Console.WriteLine($"Job scheduled! ID: {result.Result.Id}");
            }

            if (result2.IsSucceeded)
            {
                Console.WriteLine($"Job scheduled! ID: {result2.Result.Id}");
            }
        }

        




        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ScheduleJob();
        }
    }
}
