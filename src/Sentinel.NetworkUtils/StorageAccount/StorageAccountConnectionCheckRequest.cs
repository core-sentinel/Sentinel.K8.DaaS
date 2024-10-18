using MediatR;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.StorageAccount
{
    public class StorageAccountConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
    {
        public string? containerName { get; set; } = default!;
        public string? ConnectionString { get; set; } = default!;
        public string Url { get; set; }
        public int Port { get; set; }
        public bool UseMSI { get; set; }
        public ServicePrincipal? ServicePrincipal { get; set; }

        public StorageAccountConnectionCheckRequest()
        {
            ServicePrincipal = new ServicePrincipal();
        }
    }
}
