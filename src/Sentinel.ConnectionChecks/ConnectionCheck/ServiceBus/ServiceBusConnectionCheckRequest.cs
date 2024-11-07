using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.ServiceBus;

public class ServiceBusConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = ".servicebus.windows.net";
    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }
    public string SelectedAuthenticationType { get; set; } = "None";

    public string? QueueName { get; set; } = default!;
    public string? ConnectionString { get; set; } = default!;
    public ServiceBusConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
