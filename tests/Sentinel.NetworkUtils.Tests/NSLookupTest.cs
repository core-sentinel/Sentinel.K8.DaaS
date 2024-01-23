using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.Tests.Helper;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class NSLookupTest
    {
        private readonly ITestOutputHelper _output;
        private readonly string? webhostname;

        public NSLookupTest(ITestOutputHelper output)
        {
            _output = output;

            var config = ConfigurationHelper.GetConfiguration(null);
            webhostname = config["WebHostName"];
            //invalidhostName = config["InvalidHostName"];



        }
        [Fact]
        public async Task Test_GetIPAddress_Goggle()
        {
            // Arrange
            string hostName = webhostname;
            // string expectedIpAddress = "142.250.70.174";

            // Act
            string ipAddress = await NSLookup.GetIPAddress(hostName);

            // Assert
            //Assert.StartsWith("142.250", ipAddress);
            Assert.Equal(4, ipAddress.Split('.').Count());
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