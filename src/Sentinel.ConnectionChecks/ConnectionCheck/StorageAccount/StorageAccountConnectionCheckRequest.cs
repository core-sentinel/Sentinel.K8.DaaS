using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.StorageAccount;

public class StorageAccountConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{

    public string Url { get; set; } = ".blob.core.windows.net";
    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }


    public string? containerName { get; set; } = default!;
    public string? ConnectionString { get; set; } = default!;
    public StorageAccountConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
