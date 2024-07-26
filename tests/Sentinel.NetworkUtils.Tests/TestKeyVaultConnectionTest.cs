using Xunit;
using Sentinel.NetworkUtils.Models;
using Sentinel.Tests.Helper;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;
using Sentinel.NetworkUtils.KeyVault;

namespace Sentinel.Worker.NetworkUtils.Tests
{

    public class TestKeyVaultConnectionTest
    {
        string keyVaultName;
        ServicePrincipal principal;
        private readonly ITestOutputHelper _output;

        public TestKeyVaultConnectionTest(ITestOutputHelper output)
        {
            _output = output;

            var config = ConfigurationHelper.GetConfiguration(null);
            keyVaultName = config["KeyVaultName"];
            var principal = new ServicePrincipal
            {
                TenantId = config["TenantId"],
                PrincipalId = config["PrincipalId"],
                ClientId = config["ClientId"],
                ClientSecret = config["ClientSecret"]
            };
        }

        [Fact]
        public async Task Test_Connection_Successful_WithPrincipal()
        {
            // Act
            var result = await TestKeyVaultConnection.TestConnection(keyVaultName, principal);
            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_Connection_Successful_WithoutPrincipal()
        {
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