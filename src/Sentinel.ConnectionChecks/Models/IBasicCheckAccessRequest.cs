using MediatR;

namespace Sentinel.ConnectionChecks.Models
{
    public interface IBasicCheckAccessRequest : IRequest<TestNetConnectionResponse>
    {

        string Url { get; set; }
        int Port { get; set; }

        bool UseMSI { get; set; }
        ServicePrincipal? ServicePrincipal { get; set; }
    }
}
