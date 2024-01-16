using Xunit;
using Sentinel.NetworkUtils.Helpers;
using Sentinel.NetworkUtils.Models;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Sentinel.Tests.Helper;
using Xunit.Abstractions;

namespace Sentinel.Worker.NetworkUtils.Tests;
public class TestServiceBusConnectionTest
{

    private readonly string? webhostname;
    private readonly ITestOutputHelper _output;

    public string? SBSuccessConnectionString { get; }
    public string? SBMSIConnectionString { get; }
    public string? SBcontainerName1 { get; }
    public string? SBcontainerName2 { get; }
    public string? SBFailconnectionString { get; }

    private string? successPrincipalTenantId;
    private readonly string? successPrincipalPrincipalId;
    private readonly string? successPrincipalClientId;
    private readonly string? successPrincipalClientSecret;
    private readonly string? successPrincipalUserName;
    private readonly string? failurePrincipalTenantId;
    private readonly string? failurePrincipalPrincipalId;
    private readonly string? failurePrincipalClientId;
    private readonly string? failurePrincipalClientSecret;

    public TestServiceBusConnectionTest(ITestOutputHelper output)
    {
        _output = output;

        var config = ConfigurationHelper.GetConfiguration(null);
        SBSuccessConnectionString = config["SBSuccessConnectionString"];
        SBMSIConnectionString = config["SBMSIConnectionString"];
        SBcontainerName1 = config["SBcontainerName1"];
        SBcontainerName2 = config["SBcontainerName2"];
        SBFailconnectionString = config["SBFailconnectionString"];


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
        string connectionString = SBSuccessConnectionString;
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
        string connectionString = SBMSIConnectionString;
        bool useMSI = false;
        ServicePrincipal principal = new ServicePrincipal
        {
            TenantId = successPrincipalTenantId,
            PrincipalId = successPrincipalPrincipalId,
            ClientId = successPrincipalClientId,
            ClientSecret = successPrincipalClientSecret
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
        string connectionString = SBMSIConnectionString;
        bool useMSI = false;
        ServicePrincipal principal = new ServicePrincipal
        {
            TenantId = failurePrincipalTenantId,
            PrincipalId = failurePrincipalPrincipalId,
            ClientId = failurePrincipalClientId,
            ClientSecret = failurePrincipalClientSecret
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
        string connectionString = SBMSIConnectionString;
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
        string connectionString = SBFailconnectionString;
        bool useMSI = false;

        // Act
        var result = await TestServiceBusConnection.TestConnection(connectionString, "tests", useMSI);

        // Assert
        Assert.False(result.IsConnected);

    }
}
