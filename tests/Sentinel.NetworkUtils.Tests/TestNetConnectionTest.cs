using System.Threading.Tasks;
using Sentinel.NetworkUtils.Helpers;
using Xunit;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class TestNetConnectionTest
    {
        [Fact]
        public async Task Test_TestConnection_Success()
        {
            // Arrange
            string hostName = "google.com";
            int port = 80;

            // Act
            var result = await TestNetConnection.TestConnection(hostName, port);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_TestConnection_Failure()
        {
            // Arrange
            string hostName = "invalidhost";
            int port = 1234;

            // Act
            var result = await TestNetConnection.TestConnection(hostName, port);

            // Assert
            Assert.False(result.IsConnected);
        }


        [Fact]
        public async Task Test_TestConnection_sql()
        {
            // Arrange
            string hostName = "mercan.database.windows.net";
            int port = 1433;

            // Act
            var result = await TestNetConnection.TestConnection(hostName, port);

            // Assert
            Assert.True(result.IsConnected);
        }


        [Fact]
        public async Task Test_TestConnection_ServiceBus()
        {
            // Arrange
            string hostName = "mercan.servicebus.windows.net";
            int port = 5672;

            // Act
            var result = await TestNetConnection.TestConnection(hostName, port);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_TestConnection_Redis()
        {
            // Arrange
            string hostName = "mercan.redis.cache.windows.net";
            int port = 6380;

            // Act
            var result = await TestNetConnection.TestConnection(hostName, port);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_TestConnection_Redis_Wrong_Port()
        {
            // Arrange
            string hostName = "mercan.redis.cache.windows.net";
            int port = 6379;

            // Act
            var result = await TestNetConnection.TestConnection(hostName, port);

            // Assert
            Assert.False(result.IsConnected);
        }
    }
}