using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.SQL;
internal class SQLConnectionCheckRequestHandler : IRequestHandler<SQLConnectionCheckRequest, TestNetConnectionResponse>
{
    public async Task<TestNetConnectionResponse> Handle(SQLConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        return await TestSQLConnection.TestConnection(request);
    }
}
