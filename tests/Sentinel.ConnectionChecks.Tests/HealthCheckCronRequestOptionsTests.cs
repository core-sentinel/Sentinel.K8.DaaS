using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sentinel.ConnectionChecks;
using Xunit;

namespace Sentinel.ConnectionChecks.Tests;

public class HealthCheckCronRequestOptionsTests
{
    [Fact]
    public void Should_Deserialize_HealthCheckCronRequest_From_AppSettings_Using_Options_Pattern()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: false)
            .Build();

        var services = new ServiceCollection();
        services.Configure<HealthCheckCronRequest>(configuration.GetSection("HealthCheckCronRequest"));
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var options = serviceProvider.GetRequiredService<IOptions<HealthCheckCronRequest>>();
        var healthCheckConfig = options.Value;

        // Assert
        Assert.NotNull(healthCheckConfig);
        Assert.Equal("0 */5 * * * *", healthCheckConfig.Cron);
        
        // Verify TcpPing
        Assert.NotEmpty(healthCheckConfig.TcpPing);
        //Assert.True(healthCheckConfig.TcpPing.Contains(p=> p.Url == "api-check"));
        var tcpCheck = healthCheckConfig.TcpPing.SingleOrDefault(p=> p.Url =="api-check");
        Assert.Equal("api.example.com", tcpCheck.Url);
        Assert.Equal(443, tcpCheck.Port);
        Assert.Equal("TCP", tcpCheck.Protocol);
        
        // Verify Http
        Assert.NotEmpty(healthCheckConfig.Http);
        // Assert.True(healthCheckConfig.Http.ContainsKey("health-check"));
        var httpCheck = healthCheckConfig.Http.SingleOrDefault(p=> p.Url =="health-check");
        Assert.Equal("https://api.example.com/health", httpCheck.Url);
        Assert.Equal("Get", httpCheck.HttpMethod);
        
        // Verify StorageAccount
        Assert.NotEmpty(healthCheckConfig.StrorageAccount);
        //Assert.True(healthCheckConfig.StrorageAccount.ContainsKey("prod-storage"));
        var storageCheck = healthCheckConfig.StrorageAccount.SingleOrDefault(p => p.Url =="prod-storage");
        Assert.Equal("mystorageacct.blob.core.windows.net", storageCheck.Url);
        Assert.True(storageCheck.UseMSI);
        Assert.Equal("MSI", storageCheck.SelectedAuthenticationType);
        Assert.True(storageCheck.TestBlobStorage);
        
        // Verify KeyVault
        Assert.NotEmpty(healthCheckConfig.KeyVault);
        //Assert.True(healthCheckConfig.KeyVault.ContainsKey("app-keyvault"));
        var kvCheck = healthCheckConfig.KeyVault.SingleOrDefault(p => p.Url =="app-keyvault");
        Assert.Equal("mykeyvault.vault.azure.net", kvCheck.Url);
        Assert.Equal("mykeyvault", kvCheck.KeyVaultName);
        
        // Verify ServiceBus
        Assert.NotEmpty(healthCheckConfig.ServiceBus);
        //Assert.True(healthCheckConfig.ServiceBus.ContainsKey("orders-queue"));
        var sbCheck = healthCheckConfig.ServiceBus.SingleOrDefault(p => p.Url == "orders-queue");
        Assert.Equal("myservicebus.servicebus.windows.net", sbCheck.Url);
        Assert.Equal("orders", sbCheck.QueueName);
        
        // Verify Redis
        Assert.NotEmpty(healthCheckConfig.Redis);
        // Assert.True(healthCheckConfig.Redis.ContainsKey("cache-check"));
        var redisCheck = healthCheckConfig.Redis.SingleOrDefault( p=> p.Url =="cache-check");
        Assert.Equal("myredis.redis.cache.windows.net", redisCheck.Url);
        Assert.Equal(6380, redisCheck.Port);
        
        // Verify EventHub
        Assert.NotEmpty(healthCheckConfig.EventHub);
        // Assert.True(healthCheckConfig.EventHub.ContainsKey("telemetry-hub"));
        var ehCheck = healthCheckConfig.EventHub.SingleOrDefault(p => p.Url =="telemetry-hub");
        Assert.Equal("myeventhub.servicebus.windows.net", ehCheck.Url);
        Assert.Equal("telemetry", ehCheck.EventHubName);
        
        // Verify CosmosDB
        Assert.NotEmpty(healthCheckConfig.CosmosDB);
        //Assert.True(healthCheckConfig.CosmosDB.ContainsKey("main-db"));
        var cosmosCheck = healthCheckConfig.CosmosDB.SingleOrDefault(p => p.Url == "main-db");
        Assert.Equal("mycosmosdb.documents.azure.com", cosmosCheck.Url);
        Assert.Equal("MainDB", cosmosCheck.DatabaseName);
        Assert.Equal("Orders", cosmosCheck.ContainerName);
        
        // Verify SQLServer
        Assert.NotEmpty(healthCheckConfig.SQLServer);
        // Assert.True(healthCheckConfig.SQLServer.ContainsKey("prod-sql"));
        var sqlCheck = healthCheckConfig.SQLServer.SingleOrDefault(p => p.Url =="prod-sql");
        Assert.Equal("myserver.database.windows.net", sqlCheck.Url);
        Assert.Equal(1433, sqlCheck.Port);
        
        // Verify AzureAppConfig
        Assert.NotEmpty(healthCheckConfig.AzureAppConfig);
        //Assert.True(healthCheckConfig.AzureAppConfig.ContainsKey("app-config"));
        var appConfigCheck = healthCheckConfig.AzureAppConfig.SingleOrDefault(p => p.Url =="app-config");
        Assert.Equal("myappconfig.azconfig.io", appConfigCheck.Url);
    }

    [Fact]
    public void Should_Deserialize_HealthCheckCronRequest_With_ServicePrincipal_Authentication()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.serviceprincipal.json", optional: false, reloadOnChange: false)
            .Build();

        var services = new ServiceCollection();
        services.Configure<HealthCheckCronRequest>(configuration.GetSection("HealthCheckCronRequest"));
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var options = serviceProvider.GetRequiredService<IOptions<HealthCheckCronRequest>>();
        var healthCheckConfig = options.Value;

        // Assert
        Assert.NotNull(healthCheckConfig);
        Assert.Equal("0 */15 * * * *", healthCheckConfig.Cron);
        
        // Verify StorageAccount with ServicePrincipal
        var storageCheck = healthCheckConfig.StrorageAccount.SingleOrDefault(p=> p.Url=="prod-storage");
        Assert.NotNull(storageCheck.ServicePrincipal);
        Assert.Equal("tenant-123", storageCheck.ServicePrincipal.TenantId);
        Assert.Equal("principal-456", storageCheck.ServicePrincipal.PrincipalId);
        Assert.Equal("client-789", storageCheck.ServicePrincipal.ClientId);
        Assert.Equal("secret-abc", storageCheck.ServicePrincipal.ClientSecret);
        
        // Verify KeyVault with ServicePrincipal
        var kvCheck = healthCheckConfig.KeyVault.SingleOrDefault(p=> p.Url == "prod-keyvault");
        Assert.NotNull(kvCheck.ServicePrincipal);
        Assert.Equal("tenant-guid-1", kvCheck.ServicePrincipal.TenantId);
        Assert.Equal("client-guid-1", kvCheck.ServicePrincipal.ClientId);
        Assert.Equal("secret-1", kvCheck.ServicePrincipal.ClientSecret);
    }

    [Fact]
    public void Should_Deserialize_HealthCheckCronRequest_With_ConnectionStrings()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.connectionstrings.json", optional: false, reloadOnChange: false)
            .Build();

        var services = new ServiceCollection();
        services.Configure<HealthCheckCronRequest>(configuration.GetSection("HealthCheckCronRequest"));
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var options = serviceProvider.GetRequiredService<IOptions<HealthCheckCronRequest>>();
        var healthCheckConfig = options.Value;

        // Assert
        Assert.NotNull(healthCheckConfig);
        
        // Verify ServiceBus with ConnectionString
        var sbCheck = healthCheckConfig.ServiceBus.SingleOrDefault(p => p.Url=="legacy-queue");
        Assert.Equal("ConnectionString", sbCheck.SelectedAuthenticationType);
        Assert.Contains("Endpoint=sb://legacy.servicebus.windows.net/", sbCheck.ConnectionString);
        
        // Verify Redis with ConnectionString
        var redisCheck = healthCheckConfig.Redis.SingleOrDefault(p => p.Url == "legacy-cache");
        Assert.Equal("ConnectionString", redisCheck.SelectedAuthenticationType);
        Assert.Contains("legacycache.redis.cache.windows.net", redisCheck.ConnectionString);
        
        // Verify CosmosDB with ConnectionString
        var cosmosCheck = healthCheckConfig.CosmosDB.SingleOrDefault(p=> p.Url =="legacy-cosmos");
        Assert.Equal("ConnectionString", cosmosCheck.SelectedAuthenticationType);
        Assert.Contains("AccountEndpoint=https://legacycosmos.documents.azure.com", cosmosCheck.ConnectionString);
        
        // Verify SQL with ConnectionString
        var sqlCheck = healthCheckConfig.SQLServer.SingleOrDefault(p=> p.Url == "legacy-sql");
        Assert.Equal("ConnectionString", sqlCheck.SelectedAuthenticationType);
        Assert.Contains("Server=legacyserver.database.windows.net", sqlCheck.ConnectionString);
    }

    [Fact]
    public void Should_Handle_Empty_Dictionaries_In_Configuration()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.minimal.json", optional: false, reloadOnChange: false)
            .Build();

        var services = new ServiceCollection();
        services.Configure<HealthCheckCronRequest>(configuration.GetSection("HealthCheckCronRequest"));
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var options = serviceProvider.GetRequiredService<IOptions<HealthCheckCronRequest>>();
        var healthCheckConfig = options.Value;

        // Assert
        Assert.NotNull(healthCheckConfig);
        Assert.Equal("0 */5 * * * *", healthCheckConfig.Cron);
        Assert.Empty(healthCheckConfig.TcpPing);
        Assert.Empty(healthCheckConfig.Http);
        Assert.Empty(healthCheckConfig.StrorageAccount);
        Assert.Empty(healthCheckConfig.KeyVault);
        Assert.Empty(healthCheckConfig.ServiceBus);
        Assert.Empty(healthCheckConfig.Redis);
        Assert.Empty(healthCheckConfig.EventHub);
        Assert.Empty(healthCheckConfig.CosmosDB);
        Assert.Empty(healthCheckConfig.SQLServer);
        Assert.Empty(healthCheckConfig.AzureAppConfig);
    }
}