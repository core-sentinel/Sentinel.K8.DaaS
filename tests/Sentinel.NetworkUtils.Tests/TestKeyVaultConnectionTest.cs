using Xunit;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class TestKeyVaultConnectionTest
    {
        [Fact]
        public async Task Test_Connection_Successful_WithPrincipal()
        {
            // Arrange
            string keyVaultName = "mercan";
            ServicePrincipal principal = new ServicePrincipal
            {
                TenantId = "e1870496-eab8-42d0-8eb9-75fa94cfc3b8",
                PrincipalId = "c94a676a-0675-45d6-a2b7-177207f3a0b1",
                ClientId = "b7ed020e-9672-4b42-91ce-f5636c756f71",
                ClientSecret = "35j8Q~Ofgy9B7L7O87qyya0iyhnzwDLoZrVtHcpC"
            };

            // Act
            var result = await TestKeyVaultConnection.TestConnection(keyVaultName, principal);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_Connection_Successful_WithoutPrincipal()
        {
            // Arrange
            string keyVaultName = "mercan";

            // Act
            var result = await TestKeyVaultConnection.TestConnection(keyVaultName);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_Connection_Failure()
        {
            // Arrange
            string keyVaultName = "invalidkeyvault";

            // Act
            var result = await TestKeyVaultConnection.TestConnection(keyVaultName);

            // Assert
            Assert.False(result.IsConnected);
        }
    }
}