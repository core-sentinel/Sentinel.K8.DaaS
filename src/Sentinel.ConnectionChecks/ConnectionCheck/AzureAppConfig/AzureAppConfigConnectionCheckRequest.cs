using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.AzureAppConfig;
public class AzureAppConfigConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = "";
    public int Port { get; set; }
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }

    public AzureAppConfigConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}

