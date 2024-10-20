using System.Net;
using System.Net.Sockets;

namespace Sentinel.ConnectionChecks.ConnectionCheck.TcpPing
{
    internal class NSLookup
    {
        public static async Task<string> GetIPAddress(string hostName)
        {
            try
            {
                var host = await Dns.GetHostEntryAsync(hostName);
                return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
