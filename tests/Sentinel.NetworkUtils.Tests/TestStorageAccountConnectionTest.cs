using Xunit;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.NetworkUtils.Models;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Sentinel.Tests.Helper;
using Xunit.Abstractions;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class TestStorageAccountConnectionTest
    {
        private readonly ITestOutputHelper _output;

        public string? SASuccessConnectionString { get; }
        public string? SAContainerName { get; }
        public string? SAMSIConnectionString { get; }
        public string? SAMSIContainerName { get; }
        public string? SAFailConnectionString { get; }

        private string? successPrincipalTenantId;
        private readonly string? successPrincipalPrincipalId;
        private readonly string? successPrincipalClientId;
        private readonly string? successPrincipalClientSecret;

        public TestStorageAccountConnectionTest(ITestOutputHelper output)
        {
            _output = output;

            var config = ConfigurationHelper.GetConfiguration(null);


            SASuccessConnectionString = config["SASuccessConnectionString"];

            SAContainerName = config["SAContainerName"];
            SAMSIConnectionString = config["SAMSIConnectionString"];
            SAMSIContainerName = config["SAMSIContainerName"];
            SAFailConnectionString = config["SAFailConnectionString"];

            successPrincipalTenantId = config["successPrincipalTenantId"];
            successPrincipalPrincipalId = config["successPrincipalPrincipalId"];
            successPrincipalClientId = config["successPrincipalClientId"];
            successPrincipalClientSecret = config["successPrincipalClientSecret"];
        }
        [Fact]
        public async Task Test_Connection_Successful()
        {
            // Arrange
            string connectionString = SASuccessConnectionString;
            string containerName = SAContainerName;

            // Act
            var result = await TestStorageAccountConnection.TestConnection(connectionString, containerName);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_Connection_Successful_WithPrincipal()
        {
            // Arrange
            string connectionString = SAMSIConnectionString;
            string containerName = SAMSIContainerName;
            bool useMSI = false;
            ServicePrincipal principal = new ServicePrincipal
            {
                TenantId = successPrincipalTenantId,
                PrincipalId = successPrincipalPrincipalId,
                ClientId = successPrincipalClientId,
                ClientSecret = successPrincipalClientSecret
            };

            // Act
            var result = await TestStorageAccountConnection.TestConnection(connectionString, containerName, useMSI, principal);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_Connection_Successful_WithMSI()
        {
            // Arrange
            string connectionString = SAMSIConnectionString;
            string containerName = SAContainerName;
            bool useMSI = true;

            // Act
            var result = await TestStorageAccountConnection.TestConnection(connectionString, containerName, useMSI);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_Connection_Failure()
        {
            // Arrange
            string connectionString = SAFailConnectionString;
            string containerName = SAContainerName;

            // Act
            var result = await TestStorageAccountConnection.TestConnection(connectionString, containerName);

            // Assert
            Assert.False(result.IsConnected);
        }
    }
}