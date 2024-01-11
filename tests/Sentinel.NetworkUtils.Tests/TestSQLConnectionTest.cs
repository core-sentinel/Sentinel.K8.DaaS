using System.Data.SqlClient;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.NetworkUtils.Models;
using Xunit;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class TestSQLConnectionTest
    {
        [Fact]
        public void Test_TestConnection_Success()
        {
            // Arrange
            string connectionString = "Server=tcp:mercan.database.windows.net,1433;Initial Catalog=healthCheck;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';";

            // Act
            var result = TestSQLConnection.TestConnection(connectionString);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public void Test_TestConnection_Failure()
        {
            // Arrange
            string connectionString = "Server=tcp:mercan.database.windows.net,1433;Initial Catalog=healthCheck;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // Act
            var result = TestSQLConnection.TestConnection(connectionString);

            // Assert
            Assert.False(result.IsConnected);
        }

        [Fact]
        public void Test_TestConnection_With_MSI_Success()
        {
            // Arrange
            //   string connectionString = "Server=tcp:mercan.database.windows.net;Authentication=Active Directory Default; Database=healthCheck;"; //"Server=tcp:mercan.database.windows.net,1433;Authentication=\"Active Directory Default\";Initial Catalog=healthCheck;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string connectionString = "Server=tcp:mercan.database.windows.net;Database=healthCheck;";
            bool useMSI = true;

            // Act
            var result = TestSQLConnection.TestConnection(connectionString, useMSI);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public void Test_TestConnection_With_MSI_Failure()
        {
            // Arrange
            string connectionString = "Server=tcp:mercan.database.windows.net,1433;Initial Catalog=healthCheck;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;";

            bool useMSI = true;

            // Act
            var result = TestSQLConnection.TestConnection(connectionString, useMSI);

            // Assert
            Assert.False(result.IsConnected);
        }


        [Fact]
        public void Test_TestConnection_With_SP_Success()
        {
            // Arrange
            string connectionString = "Server=tcp:mercan.database.windows.net,1433;Initial Catalog=healthCheck;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            bool useMSI = false;
            ServicePrincipal principal = new ServicePrincipal
            {
                TenantId = "e1870496-eab8-42d0-8eb9-75fa94cfc3b8",
                ClientId = "1dc1d6c4-1676-4eb5-adb4-4f2dc26f9ff1",
                ClientSecret = "BtM8Q~X5X1udNGsbx1i433.HsEtZLfjaL8yx-bQe"
            };
            // Act
            var result = TestSQLConnection.TestConnection(connectionString, useMSI, principal);

            // Assert
            Assert.True(result.IsConnected);
        }
    }
}