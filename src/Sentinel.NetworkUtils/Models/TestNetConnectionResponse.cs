using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sentinel.NetworkUtils.Models
{
    public class TestNetConnectionResponse
    {
        public bool IsConnected { get; set; }
        public string? Message { get; set; }

        public TestNetConnectionResponse()
        {

        }

        public TestNetConnectionResponse(bool isConnected)
        {
            IsConnected = isConnected;
        }

        public TestNetConnectionResponse(bool isConnected, string message)
        {
            IsConnected = isConnected;
            Message = message;
        }
    }
}