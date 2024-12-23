using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.TcpPing;

[ConnectionCheck(Name = "TcpPing", Order = 1)]
public class TcpPingConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = "";

    public string Domain { get; set; } = "";

    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }

    public Type AdditionalRequestRazorContentType { get => typeof(TcpPingConnectionCheckUI); }

    public string SelectedAuthenticationType { get; set; } = "None";
    public string Protocol { get; set; } = "TCP";
    public TcpPingConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
