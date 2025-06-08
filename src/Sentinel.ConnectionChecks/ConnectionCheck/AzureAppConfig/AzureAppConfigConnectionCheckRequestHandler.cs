using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.AzureAppConfig;
internal class AzureAppConfigConnectionCheckRequestHandler : IRequestHandler<AzureAppConfigConnectionCheckRequest, TestNetConnectionResponse>
{
    public Task<TestNetConnectionResponse> Handle(AzureAppConfigConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new TestNetConnectionResponse());
    }


}

