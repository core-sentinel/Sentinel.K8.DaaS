using System.Data.SqlClient;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.NetworkUtils.Models;
using Sentinel.Tests.Helper;
using Xunit;
using Xunit.Abstractions;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class TestSQLConnectionTest
    {
        public string? SQLADConnectionStrings { get; }
        public string? SQLConnectionStrings { get; }
        public string? SQLFailConnectionStrings { get; }
        public string? SQLMSISuccessConnectionStrings { get; }
        public string? SQLMSIFailConnectionStrings { get; }
        public string? SQLSPSuccessConnectionStrings { get; }

        private readonly string? successPrincipalTenantId;
        private readonly string? successPrincipalPrincipalId;
        private readonly string? successPrincipalClientId;
        private readonly string? successPrincipalClientSecret;
        private readonly ITestOutputHelper _output;

        public TestSQLConnectionTest(ITestOutputHelper output)
        {
            _output = output;

            var config = ConfigurationHelper.GetConfiguration(null);


            SQLADConnectionStrings = config["SQLADConnectionStrings"];
            SQLConnectionStrings = config["SQLConnectionStrings"];
            SQLFailConnectionStrings = config["SQLFailConnectionStrings"];
            SQLMSISuccessConnectionStrings = config["SQLMSISuccessConnectionStrings"];
            SQLMSIFailConnectionStrings = config["SQLMSIFailConnectionStrings"];
            SQLSPSuccessConnectionStrings = config["SQLSPSuccessConnectionStrings"];


            successPrincipalTenantId = config["successPrincipalTenantId"];
            successPrincipalPrincipalId = config["successPrincipalPrincipalId"];
            successPrincipalClientId = config["successPrincipalClientId"];
            successPrincipalClientSecret = config["successPrincipalClientSecret"];




        }

        [Fact]
        public void Test_TestConnection_Success()
        {
            // Arrange
            string connectionString = SQLADConnectionStrings;
            // Act
            var result = TestSQLConnection.TestConnection(connectionString);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public void Test_TestConnection_Failure()
        {
            // Arrange
            string connectionString = SQLFailConnectionStrings;

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
            string connectionString = SQLMSISuccessConnectionStrings;
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
            string connectionString = SQLMSIFailConnectionStrings;

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
            string connectionString = SQLSPSuccessConnectionStrings;

            bool useMSI = false;
            ServicePrincipal principal = new ServicePrincipal
            {
                TenantId = successPrincipalTenantId,
                ClientId = successPrincipalClientId,
                ClientSecret = successPrincipalClientSecret
            };
            // Act
            var result = TestSQLConnection.TestConnection(connectionString, useMSI, principal);

            // Assert
            Assert.True(result.IsConnected);
        }
    }
}