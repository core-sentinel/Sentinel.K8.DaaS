using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.EventHub;

public class EventHubConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = ".servicebus.windows.net";
    public int Port { get; set; } = 5671;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }

    public string? ConnectionString { get; set; } = default!;
    public string? EventHubName { get; set; } = default!;

    public EventHubConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
