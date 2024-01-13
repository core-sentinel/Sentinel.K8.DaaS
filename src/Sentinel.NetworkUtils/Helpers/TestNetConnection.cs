using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.Helpers;

public static class TestNetConnection
{
    public static async Task<TestNetConnectionResponse> TestConnection(string hostName, int port)
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

                return new TestNetConnectionResponse(CheckAccessRequestResourceType.General, true, sw.ElapsedMilliseconds);
            }
        }
        catch (Exception ex)
        {
            return new TestNetConnectionResponse(CheckAccessRequestResourceType.General, false, ex.Message, sw.ElapsedMilliseconds);

        }
        finally { sw.Stop(); }
    }
}