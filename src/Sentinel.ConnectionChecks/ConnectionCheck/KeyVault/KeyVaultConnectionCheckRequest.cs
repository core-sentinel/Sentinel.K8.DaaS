using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.KeyVault;
[ConnectionCheck(Name = "KeyVault", Order = 4)]
public class KeyVaultConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = ".vault.azure.net";
    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }
    public string SelectedAuthenticationType { get; set; } = "None";
    public string? KeyVaultName { get; set; } = default!;

    public Type AdditionalRequestRazorContentType { get => typeof(KeyVaultConnectionCheckUI); }

    public KeyVaultConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
