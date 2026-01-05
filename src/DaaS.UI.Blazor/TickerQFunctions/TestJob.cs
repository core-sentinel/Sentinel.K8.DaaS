using TickerQ.Utilities.Base;

namespace DaaS.UI.Blazor.TickerQFunctions
{
    public class TestJobs
    {
        [TickerFunction("TestJob")]
        public async Task TestJob(
            TickerFunctionContext context,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"TickerQ is working! Job ID: {context.Id}");
        }
    }
}
