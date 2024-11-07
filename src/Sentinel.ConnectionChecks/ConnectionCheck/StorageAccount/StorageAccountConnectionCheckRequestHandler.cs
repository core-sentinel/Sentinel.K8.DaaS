using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.StorageAccount;
internal class StorageAccountConnectionCheckRequestHandler : IRequestHandler<StorageAccountConnectionCheckRequest, TestNetConnectionResponse<StorageAccountExtraResponse>>
{
    public Task<TestNetConnectionResponse<StorageAccountExtraResponse>> Handle(StorageAccountConnectionCheckRequest request, CancellationToken cancellationToken)
    {

        var test = new TestStorageAccountConnection(request);

        return test.TestConnection();
    }
}

// StorageAccountConnectionResponse