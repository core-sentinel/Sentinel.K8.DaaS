using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Redis;
//public record RedisConnectionCheckCommand(string ConnectionString, bool UseMSI, ServicePrincipal? Principal, string? RedisUsername) : IRequest<RedisConnectionCheckResponse>;

[ConnectionCheck(Name = "Redis", Order = 6)]
public class RedisConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = ".redis.cache.windows.net";
    public int Port { get; set; } = 6380;

    public string ConnectionString { get; set; }
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }

    public Type AdditionalRequestRazorContentType { get => typeof(RedisConnectionCheckUI); }

    public string SelectedAuthenticationType { get; set; } = "None";
    public string? RedisUsername { get; set; }



    public RedisConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }


}


