using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.EventHub;
internal class EventHubConnectionCheckRequestHandler : IRequestHandler<EventHubConnectionCheckRequest, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(EventHubConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestNetConnectionResponse());
    }
}
