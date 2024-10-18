using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Http;

public class HttpConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string? Url { get; set; } = "https://";
    public string? HttpMethod { get; set; } = "Get";
    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }

    public HttpConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
