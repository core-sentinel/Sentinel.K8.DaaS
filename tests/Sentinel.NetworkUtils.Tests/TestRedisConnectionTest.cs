using Xunit;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.Worker.NetworkUtils.Tests
{
    public class TestRedisConnectionTest
    {
        [Fact]
        public async Task Test_Connection_Successful()
        {
            // Arrange
            string connectionString = "mercan.redis.cache.windows.net:6380,password=QvM3eBpmtSfrJh8BnwJ1mCN6n6JyUTCzlAzCaI86Ua4=,ssl=True,abortConnect=False";

            // Act
            var result = await TestRedisConnection.TestConnection(connectionString);

            // Assert
            Assert.True(result.IsConnected);
        }

        [Fact]
        public async Task Test_Connection_Failure()
        {
            // Arrange
            string connectionString = "mercan.redis.cache.windows.net:6380,ssl=True,abortConnect=False";

            // Act
            var result = await TestRedisConnection.TestConnection(connectionString);

            // Assert
            Assert.False(result.IsConnected);
        }


        [Fact]
        public async Task Test_Connection_Failure_withwrongPrincipal()
        {
            // Arrange
            string connectionString = "mercan.redis.cache.windows.net:6380";


            bool useMSI = false;
            ServicePrincipal principal = new ServicePrincipal
            {
                TenantId = "e1870496-eab8-42d0-8eb9-75fa94cfc3b8",
                PrincipalId = "9608690d-04d5-480a-880b-ddc22922be9f",
                ClientId = "1dc1d6c4-1676-4eb5-adb4-4f2dc26f9ff1",
                ClientSecret = "BtM8Q~X5X1udNGsbx1i433.HsEtZLfjaL8yx-bQe"
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
            string connectionString = "mercan.redis.cache.windows.net:6380";


            bool useMSI = false;
            ServicePrincipal principal = new ServicePrincipal
            {
                TenantId = "e1870496-eab8-42d0-8eb9-75fa94cfc3b8",
                PrincipalId = "e952c25f-093a-4bef-88ca-e930885e8669",
                ClientId = "81564247-5e6e-4e52-8e04-de52753c4581",
                ClientSecret = "m0A8Q~OQqVaLzznn4ecxBJDAd5WVzDADEr4Ofb5o",
                UserName = "adbe91a3-2e2b-44e4-88cb-dac2ff42a1c5"
            };
            // Act
            var result = await TestRedisConnection.TestConnection(connectionString, useMSI, principal);

            // Assert
            Assert.True(result.IsConnected);
        }
    }
}