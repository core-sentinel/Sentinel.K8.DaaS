using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.SQL;
internal class SQLConnectionCheckRequestHandler : IRequestHandler<SQLConnectionCheckRequest, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(SQLConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestNetConnectionResponse());
    }
}
