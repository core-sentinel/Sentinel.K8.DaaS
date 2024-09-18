using MediatR;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.Redis;
internal class RedisConnectionCheckRequestHandler : IRequestHandler<RedisConnectionCheckRequest, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(RedisConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        var result = TestRedisConnection.TestConnection(request.ConnectionString, request.UseMSI, request.Principal, request.RedisUsername);
        return result;
    }
}
