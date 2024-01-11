using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Sentinel.NetworkUtils.Helpers;
public class NSLookup
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