using MediatR;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.Redis;
internal class RedisConnectionCheckCommandHandler : IRequestHandler<RedisConnectionCheckCommand, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(RedisConnectionCheckCommand request, CancellationToken cancellationToken)
    {
        var result = TestRedisConnection.TestConnection(request.ConnectionString, request.UseMSI, request.Principal, request.RedisUsername);
        return result;
    }
}
