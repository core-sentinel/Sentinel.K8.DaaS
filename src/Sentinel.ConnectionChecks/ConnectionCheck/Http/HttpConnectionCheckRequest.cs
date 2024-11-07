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
    public string SelectedAuthenticationType { get; set; } = "None";
    public HttpConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
