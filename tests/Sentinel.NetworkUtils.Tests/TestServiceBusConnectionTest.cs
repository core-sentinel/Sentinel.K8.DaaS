using Xunit;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.Worker.NetworkUtils.Tests;
public class TestServiceBusConnectionTest
{
    [Fact]
    public async Task Test_Connection_Successful()
    {
        // Arrange
        string connectionString = "Endpoint=sb://mercan.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=FLzjGRoIB1DZM+H05s9XXO06Hm+QT1pBM+ASbK0rdmc=";
        bool useMSI = false;

        // Act
        var result = await TestServiceBusConnection.TestConnection(connectionString, "tests", useMSI);

        // Assert
        Assert.True(result.IsConnected);
    }

    [Fact]
    public async Task Test_Connection_Successful_WithPrincipal()
    {
        // Arrange
        string connectionString = "mercan.servicebus.windows.net";
        bool useMSI = false;
        ServicePrincipal principal = new ServicePrincipal
        {
            TenantId = "e1870496-eab8-42d0-8eb9-75fa94cfc3b8",
            PrincipalId = "35f0c1c4-1e44-4206-b3de-ac5f06300897",
            ClientId = "b3fb6373-8036-49c8-bc1f-31f26ebcdea9",
            ClientSecret = "jKV8Q~K7Sc8-nUuIHu6FUfnm2KIzo_PloHSCgblL"
        };

        // Act
        var result = await TestServiceBusConnection.TestConnection(connectionString, "tests", useMSI, principal);

        // Assert
        Assert.True(result.IsConnected);
    }

    [Fact]
    public async Task Test_Connection_Fails_WithPrincipal_NoAccess()
    {
        // Arrange
        string connectionString = "mercan.servicebus.windows.net";
        bool useMSI = false;
        ServicePrincipal principal = new ServicePrincipal
        {
            TenantId = "e1870496-eab8-42d0-8eb9-75fa94cfc3b8",
            PrincipalId = "595fa408-0be0-40ae-b6d1-c71075c141ea",
            ClientId = "5f2e9333-5af3-4954-b55d-5ff6072bd5ed",
            ClientSecret = "Xpv8Q~GpeGZ12ZwRmIAXDqQo6fCPElYnZNnsbasy"
        };

        // Act
        var result = await TestServiceBusConnection.TestConnection(connectionString, "tests", useMSI, principal);

        // Assert
        Assert.False(result.IsConnected);
    }


    [Fact]
    public async Task Test_Connection_Successful_WithMSI()
    {
        // Arrange
        string connectionString = "mercan.servicebus.windows.net";
        bool useMSI = true;

        // Act
        var result = await TestServiceBusConnection.TestConnection(connectionString, "tests", useMSI);

        // Assert
        Assert.True(result.IsConnected);
    }

    [Fact]
    public async Task Test_Connection_Failure()
    {
        // Arrange
        string connectionString = "Endpoint=sb://mercan.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=FLzjGRoIB1DZM+H0XXO06Hm+QT1pBM+ASbK";
        bool useMSI = false;

        // Act
        var result = await TestServiceBusConnection.TestConnection(connectionString, "tests", useMSI);

        // Assert
        Assert.False(result.IsConnected);

    }
}
