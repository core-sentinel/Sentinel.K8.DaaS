using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.SQL;

public class SQLConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{

    public string Url { get; set; } = ".database.windows.net";
    public int Port { get; set; } = 1433;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }
    public string SelectedAuthenticationType { get; set; } = "None";


    public string? ConnectionString { get; set; } = default!;
    public SQLConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
