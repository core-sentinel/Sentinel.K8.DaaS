using Sentinel.ConnectionChecks.Models;
using System.Diagnostics;

namespace Sentinel.ConnectionChecks.ConnectionCheck.TcpPing
{
    internal static class TestTcpPing
    {

        public static async Task<TestNetConnectionResponse> TestTcpConnection(string hostName, int port)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var tcpClient = new System.Net.Sockets.TcpClient())
                {
                    await tcpClient.ConnectAsync(hostName, port);
                    var isAvailable = tcpClient.Available.ToString();
                    var isConnected = tcpClient.Connected.ToString();
                    tcpClient.Close();
                    tcpClient.Dispose();

                    return new TestNetConnectionResponse(CheckAccessRequestResourceType.TcpPing, true, sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                return new TestNetConnectionResponse(CheckAccessRequestResourceType.TcpPing, false, ex.Message, sw.ElapsedMilliseconds);
            }
            finally { sw.Stop(); }
        }

    }
}
