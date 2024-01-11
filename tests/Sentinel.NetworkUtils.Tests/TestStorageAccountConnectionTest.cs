using Xunit;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class TestStorageAccountConnectionTest
    {
        [Fact]
        public async Task Test_Connection_Successful()
        {
            // Arrange
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=mercan;AccountKey=hf27A5YXJ1jyCMfOT1ZsDr0Td0YA+ZvcZbgLhc3kDyCX4OcNaFmvrcMZCoJ1e9XcBS6ZaKx5HzqK+ASt6/x42w==;EndpointSuffix=core.windows.net";
            string containerName = "tests";

            // Act
            var result = await TestStorageAccountConnection.TestConnection(connectionString, containerName);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_Connection_Successful_WithPrincipal()
        {
            // Arrange
            string connectionString = "mercan";
            string containerName = "test123";
            bool useMSI = false;
            ServicePrincipal principal = new ServicePrincipal
            {
                TenantId = "e1870496-eab8-42d0-8eb9-75fa94cfc3b8",
                PrincipalId = "595fa408-0be0-40ae-b6d1-c71075c141ea",
                ClientId = "5f2e9333-5af3-4954-b55d-5ff6072bd5ed",
                ClientSecret = "Xpv8Q~GpeGZ12ZwRmIAXDqQo6fCPElYnZNnsbasy"
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
            string connectionString = "mercan";
            string containerName = "tests";
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
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=mercan;AccountKey=hf27AJ1jyCMfOT1ZsDr0Td0YA+ZvcZbgLhc3kDyCX4OcNaFmvrcMZCoJ1e9XcBS6ZaKx5HzqK+ASt6/x42w==;EndpointSuffix=core.windows.net";
            string containerName = "tests";

            // Act
            var result = await TestStorageAccountConnection.TestConnection(connectionString, containerName);

            // Assert
            Assert.False(result.IsConnected);
        }
    }
}