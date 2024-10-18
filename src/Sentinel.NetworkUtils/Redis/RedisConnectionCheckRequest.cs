using MediatR;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.Redis;
//public record RedisConnectionCheckCommand(string ConnectionString, bool UseMSI, ServicePrincipal? Principal, string? RedisUsername) : IRequest<RedisConnectionCheckResponse>;

public class RedisConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{

    public string Url { get; set; } = "";
    public int Port { get; set; }

    public string ConnectionString { get; set; }
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }
    public string? RedisUsername { get; set; }



    public RedisConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }


}


