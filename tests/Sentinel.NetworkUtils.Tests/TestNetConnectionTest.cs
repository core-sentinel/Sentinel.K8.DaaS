using System.Threading.Tasks;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.Tests.Helper;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class TestNetConnectionTest
    {
        private readonly ITestOutputHelper _output;
        private readonly string webhostname;
        private readonly string invalidhostName;
        private readonly string dbhostName;
        private readonly string sbhostName;
        private readonly string cacheHostName;

        public TestNetConnectionTest(ITestOutputHelper output)
        {
            _output = output;

            var config = ConfigurationHelper.GetConfiguration(null);
            webhostname = config["WebHostName"];
            invalidhostName = config["InvalidHostName"];
            dbhostName = config["DbHostName"];
            sbhostName = config["SbHostName"];
            cacheHostName = config["CacheHostName"];


        }


        [Fact]
        public async Task Test_TestConnection_Success()
        {
            // Arrange
            string hostName = webhostname;
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
            string hostName = invalidhostName;
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
            string hostName = dbhostName;
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
            string hostName = sbhostName;
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
            string hostName = cacheHostName;
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
            string hostName = cacheHostName;
            int port = 6379;

            // Act
            var result = await TestNetConnection.TestConnection(hostName, port);

            // Assert
            Assert.False(result.IsConnected);
        }
    }
}