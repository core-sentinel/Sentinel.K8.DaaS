using MediatR;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.NetworkUtils.KeyVault;
using Sentinel.NetworkUtils.Models;
using Sentinel.NetworkUtils.Redis;

namespace Sentinel.NetworkUtils.Services;
public class ConnectivityCheckService
{
    private readonly IMediator _mediator;
    public Dictionary<int, string> Categories;


    public ConnectivityCheckService(IMediator mediator)
    {
        _mediator = mediator;

        Categories = Enum.GetValues(typeof(CheckAccessRequestResourceType))
     .Cast<CheckAccessRequestResourceType>().ToDictionary(t => (int)t, t => t.ToString());



    }

    public async Task<CheckAccessResponse> ConnectionCheck(CheckAccessRequest request)
    {
        var ipaddress = await NSLookup.GetIPAddress(request.Url);
        var netResult = await TestNetConnection.TestConnection(ipaddress, request.Port);
        TestNetConnectionResponse additionalResult = null;
        switch (request.ResourceType)
        {
            case CheckAccessRequestResourceType.Http:
                additionalResult = await checkHttpRequest(request);
                break;
            case CheckAccessRequestResourceType.StrorageAccount:
                additionalResult = await checkStorageAccount(request);
                break;
            case CheckAccessRequestResourceType.KeyVault:
                additionalResult = await TestKeyVaultConnection.TestConnection(request.KeyVaultDetails.KeyVaultName, request.ServicePrincipal);
                break;
            case CheckAccessRequestResourceType.ServiceBus:
                additionalResult = await checkServiceBus(request);
                break;
            case CheckAccessRequestResourceType.Redis:
                additionalResult = await checkRedis(request);
                break;
            case CheckAccessRequestResourceType.EventHub:
                break;
            case CheckAccessRequestResourceType.CosmosDB:
                break;
            case CheckAccessRequestResourceType.SQLServer:
                additionalResult = checkSQL(request);
                break;
            default:
                break;
        }
        var response = new CheckAccessResponse
        {
            ipaddress = ipaddress,
            netResult = netResult,
            additionalResult = additionalResult
        };
        return response;
    }

    private TestNetConnectionResponse checkSQL(CheckAccessRequest request)
    {
        TestNetConnectionResponse additionalResult = null;
        if (request.SQLServerDetails == null)
        {
            additionalResult = new TestNetConnectionResponse(CheckAccessRequestResourceType.SQLServer, false, "SQLServerDetails is required for SQLServer ResourceType");
        }
        else
        {
            additionalResult = TestSQLConnection.TestConnection(request.SQLServerDetails.ConnectionString, request.UseMSI, request.ServicePrincipal);
        }
        return additionalResult;
    }

    private async Task<TestNetConnectionResponse> checkRedis(CheckAccessRequest request)
    {

        TestNetConnectionResponse additionalResult = null;
        if (request.RedisDetails == null)
        {
            additionalResult = new TestNetConnectionResponse(CheckAccessRequestResourceType.Redis, false, "RedisDetails is required for Redis ResourceType");
        }
        else
        {

            var qq = await _mediator.Send(new RedisConnectionCheckRequest(request.RedisDetails.ConnectionString, request.UseMSI, request.ServicePrincipal, request.RedisDetails.RedisUserName));
            additionalResult = await TestRedisConnection.TestConnection(request.RedisDetails.ConnectionString, request.UseMSI, request.ServicePrincipal, request.RedisDetails.RedisUserName);
        }
        return additionalResult;
    }

    private static async Task<TestNetConnectionResponse> checkServiceBus(CheckAccessRequest request)
    {
        TestNetConnectionResponse additionalResult = null;
        if (request.ServiceBusDetails == null)
        {
            additionalResult = new TestNetConnectionResponse(CheckAccessRequestResourceType.ServiceBus, false, "ServiceBusDetails is required for ServiceBus ResourceType");
        }
        else
        {
            additionalResult = await TestServiceBusConnection.TestConnection(request.ServiceBusDetails.ConnectionString, request.ServiceBusDetails.QueueName, request.UseMSI, request.ServicePrincipal);
        }

        return additionalResult;
    }

    private static async Task<TestNetConnectionResponse> checkStorageAccount(CheckAccessRequest request)
    {
        TestNetConnectionResponse additionalResult;
        if (request.StrorageAccountDetails == null)
        {
            additionalResult = new TestNetConnectionResponse(CheckAccessRequestResourceType.StrorageAccount, false, "StrorageAccountDetails is required for StrorageAccount ResourceType");
        }
        else
        {
            additionalResult = await TestStorageAccountConnection.TestConnection(request.StrorageAccountDetails.ConnectionString, request.StrorageAccountDetails.containerName, request.UseMSI, request.ServicePrincipal);
        }

        return additionalResult;
    }


    private static async Task<TestNetConnectionResponse> checkHttpRequest(CheckAccessRequest request)
    {
        HttpMethod httpMethod = HttpMethod.Get;
        switch (request.HttpRequestDetails?.HttpMethod?.ToLower())
        {
            case "get": httpMethod = HttpMethod.Get; break;
            case "post": httpMethod = HttpMethod.Post; break;
            case "put": httpMethod = HttpMethod.Put; break;
            case "delete": httpMethod = HttpMethod.Delete; break;
            case "head": httpMethod = HttpMethod.Head; break;
            default:
                break;
        }
        var additionalResult = await TestHttpRequest.TestConnection(request.HttpRequestDetails.Url, httpMethod, request.UseMSI, request.ServicePrincipal);
        return additionalResult;
    }
}
