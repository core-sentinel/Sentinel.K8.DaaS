using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.AzureAppConfig;

[ConnectionCheck(Name = "AzureAppConfig", Order = 10)]
public class AzureAppConfigConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = "";
    public int Port { get; set; }
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }
    public Type AdditionalRequestRazorContentType { get => typeof(AzureAppConfigConnectionCheckUI); }

    public string SelectedAuthenticationType { get; set; } = "None";

    public AzureAppConfigConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}

