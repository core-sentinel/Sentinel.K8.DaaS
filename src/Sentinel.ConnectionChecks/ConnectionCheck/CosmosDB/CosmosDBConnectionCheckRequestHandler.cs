using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.CosmosDB;
internal class CosmosDBConnectionCheckRequestHandler : IRequestHandler<CosmosDBConnectionCheckRequest, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(CosmosDBConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestNetConnectionResponse());
    }
}

