
using Azure.Core;
using MediatR;
using Microsoft.Extensions.Options;
using Sentinel.ConnectionChecks;
using Sentinel.ConnectionChecks.Models;
using TickerQ.Utilities;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Interfaces.Managers;

namespace DaaS.UI.Blazor.Background
{
    public class CronTrigger : BackgroundService
    {


        private readonly ICronTickerManager<CronTickerEntity> _cronTickerManager;
        private readonly HealthCheckCronRequest _healthcheck;
        private readonly IMediator _mediator;

        public CronTrigger(ICronTickerManager<CronTickerEntity> cronTickerManager, IOptions<HealthCheckCronRequest> healthcheck, IMediator mediator)
        {
            _cronTickerManager = cronTickerManager;
            if (healthcheck.Value != null)
            {
                Console.WriteLine("CronTrigger initialized with HealthCheckCronRequest settings.");
            }
            _healthcheck = healthcheck.Value;
            _mediator = mediator;
        }

        public async Task ScheduleJob()
        {
            var result = await _cronTickerManager.AddAsync(new CronTickerEntity
            {
                Function = "TestJob",
                Description = "1234 ",
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
            //try
            //{
                // await ScheduleJob();
                foreach (var list in _healthcheck.HealthChecks)
                {
                    foreach (var req in list.Value)
                    {
                        var result = await _cronTickerManager.AddAsync(new CronTickerEntity
                        {
                            Function = "TestJob",
                            Description = req.Url,
                            Expression = "0 * * * * *",
                            Request = TickerHelper.CreateTickerRequest(
                                Newtonsoft.Json.JsonConvert.SerializeObject(req)
                                ),

                            //ExecutionTime = DateTime.UtcNow.AddSeconds(10) // Run in 10 seconds
                        });

                        if (result.IsSucceeded)
                        {
                            Console.WriteLine($"Job scheduled! ID: {result.Result.Id}");
                        }

                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error in CronTrigger: {ex.Message}");
            //}
        }
    }
}
