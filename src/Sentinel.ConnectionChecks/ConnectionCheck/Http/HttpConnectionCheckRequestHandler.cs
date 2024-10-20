using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Http;
internal class HttpConnectionCheckRequestHandler : IRequestHandler<HttpConnectionCheckRequest, TestNetConnectionResponse>
{
    public async Task<TestNetConnectionResponse> Handle(HttpConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        return await TestHttpRequest.TestConnection(request);
    }
}
