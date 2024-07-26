using Sentinel.NetworkUtils.Models;
using Sentinel.NetworkUtils.Redis;
using Sentinel.Tests.Helper;
using Xunit.Abstractions;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class TestRedisConnectionTest
    {
        private readonly ITestOutputHelper _output;

        public string? SuccessRedisConnectionStrings { get; }
        public string? FailureRedisConnectionStrings { get; }

        private readonly string? cachePrincipalConnectionString;
        private readonly string? successPrincipalTenantId;
        private readonly string? successPrincipalPrincipalId;
        private readonly string? successPrincipalClientId;
        private readonly string? successPrincipalClientSecret;
        private readonly string? successPrincipalUserName;
        private readonly string? failurePrincipalTenantId;
        private readonly string? failurePrincipalPrincipalId;
        private readonly string? failurePrincipalClientId;
        private readonly string? failurePrincipalClientSecret;

        public TestRedisConnectionTest(ITestOutputHelper output)
        {
            _output = output;

            var config = ConfigurationHelper.GetConfiguration(null);
            SuccessRedisConnectionStrings = config["SuccessRedisConnectionStrings"];
            FailureRedisConnectionStrings = config["FailureRedisConnectionStrings"];
            cachePrincipalConnectionString = config["CachePrincipalConnectionString"];

            successPrincipalTenantId = config["successPrincipalTenantId"];
            successPrincipalPrincipalId = config["successPrincipalPrincipalId"];
            successPrincipalClientId = config["successPrincipalClientId"];
            successPrincipalClientSecret = config["successPrincipalClientSecret"];
            successPrincipalUserName = config["successPrincipalUserName"];

            failurePrincipalTenantId = config["failurePrincipalTenantId"];
            failurePrincipalPrincipalId = config["failurePrincipalPrincipalId"];
            failurePrincipalClientId = config["failurePrincipalClientId"];
            failurePrincipalClientSecret = config["failurePrincipalClientSecret"];

            //invalidhostName = config["InvalidHostName"];



        }

        [Fact]
        public async Task Test_Connection_Successful()
        {
            // Arrange
            string connectionString = SuccessRedisConnectionStrings;

            // Act
            var result = await TestRedisConnection.TestConnection(connectionString);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_Connection_Failure()
        {
            // Arrange
            string connectionString = FailureRedisConnectionStrings;

            // Act
            var result = await TestRedisConnection.TestConnection(connectionString);

            // Assert
            Assert.False(result.IsConnected);
        }


        [Fact]
        public async Task Test_Connection_Failure_withwrongPrincipal()
        {
            // Arrange
            string connectionString = cachePrincipalConnectionString;


            bool useMSI = false;
            ServicePrincipal principal = new ServicePrincipal
            {
                TenantId = failurePrincipalTenantId,
                PrincipalId = failurePrincipalPrincipalId,
                ClientId = failurePrincipalClientId,
                ClientSecret = failurePrincipalClientSecret
            };
            // Act
            var result = await TestRedisConnection.TestConnection(connectionString, useMSI, principal);

            // Assert
            Assert.False(result.IsConnected);
        }


        [Fact]
        public async Task Test_Connection_Success_withRightPrincipal()
        {
            // Arrange
            string connectionString = cachePrincipalConnectionString;


            bool useMSI = false;
            ServicePrincipal principal = new ServicePrincipal
            {
                TenantId = successPrincipalTenantId,
                PrincipalId = successPrincipalPrincipalId,
                ClientId = successPrincipalClientId,
                ClientSecret = successPrincipalClientSecret,
                // UserName = successPrincipalUserName
            };
            // Act
            var result = await TestRedisConnection.TestConnection(connectionString, useMSI, principal, successPrincipalUserName);

            // Assert
            Assert.True(result.IsConnected);
        }
    }
}