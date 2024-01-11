using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Sentinel.NetworkUtils.Helpers;
using Xunit;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class NSLookupTest
    {
        [Fact]
        public async Task Test_GetIPAddress_Goggle()
        {
            // Arrange
            string hostName = "google.com";
            string expectedIpAddress = "142.250.70.174";

            // Act
            string ipAddress = await NSLookup.GetIPAddress(hostName);

            // Assert
            Assert.Equal(expectedIpAddress, ipAddress);
        }

        [Fact]
        public async Task Test_GetIPAddress_localhost()
        {
            // Arrange
            string hostName = "localhost";
            string expectedIpAddress = "127.0.0.1";

            // Act
            string ipAddress = await NSLookup.GetIPAddress(hostName);

            // Assert
            Assert.Equal(expectedIpAddress, ipAddress);
        }
    }
}