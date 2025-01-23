using MediatR;
using Microsoft.Extensions.Logging;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Http;
internal class HttpConnectionCheckRequestHandler : IRequestHandler<HttpConnectionCheckRequest, TestNetConnectionResponse>
{
    private ILogger<HttpConnectionCheckRequestHandler> _logger;

    public HttpConnectionCheckRequestHandler(ILogger<HttpConnectionCheckRequestHandler> logger)
    {
        _logger = logger;
    }

    public async Task<TestNetConnectionResponse> Handle(HttpConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        return await TestHttpRequest.TestConnection(request, _logger);
    }
}
