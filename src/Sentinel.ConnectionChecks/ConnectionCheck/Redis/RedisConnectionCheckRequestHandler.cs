using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Redis;
internal class RedisConnectionCheckRequestHandler : IRequestHandler<RedisConnectionCheckRequest, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(RedisConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        var result = TestRedisConnection.TestConnection(request.ConnectionString, request.UseMSI, request.ServicePrincipal, request.RedisUsername);
        return result;
    }
}

