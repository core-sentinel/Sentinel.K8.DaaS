using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.ServiceBus;
internal class ServiceBusConnectionCheckRequestHandler : IRequestHandler<ServiceBusConnectionCheckRequest, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(ServiceBusConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestNetConnectionResponse());
    }
}
