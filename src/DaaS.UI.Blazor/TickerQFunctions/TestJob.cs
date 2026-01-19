using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using Sentinel.ConnectionChecks.Models;
using TickerQ.Utilities.Base;

namespace DaaS.UI.Blazor.TickerQFunctions
{
    public class TestJobs
    {
        private readonly IMediator _mediator;

        public TestJobs(IMediator mediator)
        {
            _mediator = mediator;
        }

        [TickerFunction("TestJob")]
        public async Task TestJob(
            TickerFunctionContext<string> context,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(context.Request);

            JObject jsonObject = JObject.Parse(context.Request);

            // Access properties using indexers and cast the values
            string typeName = (string)jsonObject["JsonDeserializationType"];
            Type type = Type.GetType(typeName);

            var q = Newtonsoft.Json.JsonConvert.DeserializeObject(context.Request, type);
            var result  = await _mediator.Send(q) as TestNetConnectionResponse;
            Console.WriteLine($"TickerQ is working! Job ID: {context.Id}");
        }
    }
}
