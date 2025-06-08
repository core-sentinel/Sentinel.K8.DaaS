using Microsoft.Extensions.DependencyInjection;
using Sentinel.ConnectionChecks.ConnectionCheck.AzureAppConfig;
using Sentinel.ConnectionChecks.ConnectionCheck.CosmosDB;
using Sentinel.ConnectionChecks.Models;
using System.Reflection;

namespace Sentinel.ConnectionChecks.Tests
{
    public class PipelineTests
    {
        List<object> _handlers;
        public PipelineTests()
        {
            List<object> handlers = new List<object>
                {
                    new BasicClass(), //as ICommonHandler<IBasicCheckAccessRequest>,
                    new AppConfigClass(), //as ICommonHandler<IBasicCheckAccessRequest>,
                    new CosmosClass(), //as ICommonHandler<IBasicCheckAccessRequest>
                };
            _handlers = handlers;
        }

        [Fact]
        public void DI_test()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ICommonHandler, BasicClass>()
                .AddSingleton<ICommonHandler, AppConfigClass>()
                .AddSingleton<ICommonHandler, CosmosClass>()
                .BuildServiceProvider();

            var handlers = serviceProvider.GetServices<ICommonHandler>().ToList();
            Console.WriteLine(handlers.Count);
        }

        [Fact]
        public async Task RequestHandler_generic_test()
        {
            AzureAppConfigConnectionCheckRequest request1 = new AzureAppConfigConnectionCheckRequest();
            if (request1 is IBasicCheckAccessRequest)
            {
                Console.WriteLine("request1 is IBasicCheckAccessRequest");
            }

            CosmosDBConnectionCheckRequest request = new CosmosDBConnectionCheckRequest();

            foreach (var handler in _handlers)
            {
                Type handlerType = handler.GetType();

                MethodInfo method = handlerType.GetMethod("Handle");
                if (method != null && method.GetParameters().Length == 2 && method.GetParameters()[0].ParameterType.IsAssignableFrom(request.GetType()))
                {
                    var result = method.Invoke(handler, new object[] { request, CancellationToken.None });




                    if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        Console.WriteLine("Task<>");
                    }



                    if (result is Task)
                    {
                        if (result is Task<ITestNetConnectionResponse>)
                        {
                            var res1 = result.GetType().GetProperty("Result").GetValue(result);
                            Console.WriteLine(res1.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Not a Task<ITestNetConnectionResponse>");
                        }
                        await (Task)result;
                        var res = result.GetType().GetProperty("Result").GetValue(result);
                        if (res is ITestNetConnectionResponse)
                        {
                            var res1 = res as ITestNetConnectionResponse;
                            Console.WriteLine(res1.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Not a ITestNetConnectionResponse");
                        }
                        Type tt = typeof(TestNetConnectionResponse<>);

                        //if (res is tt)
                        //{
                        //    Console.WriteLine((res as TestNetConnectionResponse).Message);
                        //}
                        Console.WriteLine(result.ToString());
                        //  result = await (Task<object>)result;

                        //  Console.WriteLine(result as object);
                    }
                    Console.WriteLine(result.GetType());
                }
            }
        }
    }

    public class BasicClass : ICommonHandler<IBasicCheckAccessRequest, extendedresult>
    {
        public Task<TestNetConnectionResponse<extendedresult>> Handle(IBasicCheckAccessRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestNetConnectionResponse<extendedresult>());
        }
    }

    public class extendedresult
    {
        public string extendedname = "Blahhh";
    }

    public class AppConfigClass : ICommonHandler<AzureAppConfigConnectionCheckRequest>
    {
        public Task<TestNetConnectionResponse> Handle(AzureAppConfigConnectionCheckRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestNetConnectionResponse());
        }
    }


    public class CosmosClass : ICommonHandler<CosmosDBConnectionCheckRequest>
    {
        public Task<TestNetConnectionResponse> Handle(CosmosDBConnectionCheckRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestNetConnectionResponse());
        }
    }
}
