using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.TcpPing;
internal class TcpPingConnectionCheckRequestHandler : IRequestHandler<TcpPingConnectionCheckRequest, TestNetConnectionResponse>
{
    public async Task<TestNetConnectionResponse> Handle(TcpPingConnectionCheckRequest request, CancellationToken cancellationToken)
    {
        request.Domain = request.Url;
        if (request.Domain.Contains("http://") || request.Domain.Contains("https://"))
        {
            request.Domain = request.Domain.Replace("http://", "").Replace("https://", "");
            request.Protocol = "http";
        }
        var ipaddress = await NSLookup.GetIPAddress(request.Domain);
        var netResult = await TestTcpPing.TestTcpConnection(ipaddress, request.Port);
        netResult.IPAddress = ipaddress;
        return netResult;
    }
}

