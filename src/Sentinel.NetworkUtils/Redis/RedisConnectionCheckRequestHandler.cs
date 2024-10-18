using MediatR;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.Redis;
internal class RedisConnectionCheckRequestHandler : IRequestHandler<RedisConnectionCheckRequest, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(RedisConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        var result = TestRedisConnection.TestConnection(request.ConnectionString, request.UseMSI, request.ServicePrincipal, request.RedisUsername);
        return result;
    }
}



internal class IIdentityRequestHandler : IRequestHandler<IBasicCheckAccessRequest, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(IBasicCheckAccessRequest request, CancellationToken cancellationToken)
    {
        var result = new TestNetConnectionResponse();
        return Task.FromResult(result);
    }
}
