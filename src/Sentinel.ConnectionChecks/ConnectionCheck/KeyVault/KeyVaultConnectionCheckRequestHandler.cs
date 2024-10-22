using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.KeyVault;
internal class KeyVaultConnectionCheckRequestHandler : IRequestHandler<KeyVaultConnectionCheckRequest, TestNetConnectionResponse>
{
    public async Task<TestNetConnectionResponse> Handle(KeyVaultConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        return await TestKeyVaultConnection.TestConnection(request);
    }
}

