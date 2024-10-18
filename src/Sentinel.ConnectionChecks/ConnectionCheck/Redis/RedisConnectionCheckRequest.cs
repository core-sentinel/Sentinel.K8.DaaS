using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Redis;
//public record RedisConnectionCheckCommand(string ConnectionString, bool UseMSI, ServicePrincipal? Principal, string? RedisUsername) : IRequest<RedisConnectionCheckResponse>;

public class RedisConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = ".redis.cache.windows.net";
    public int Port { get; set; } = 6380;

    public string ConnectionString { get; set; }
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }
    public string? RedisUsername { get; set; }



    public RedisConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }


}


