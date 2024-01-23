using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Sentinel.NetworkUtils.Models;
using System.Diagnostics;

namespace Sentinel.NetworkUtils.Helpers;
/// <summary>
/// Helper class for testing Redis connection.
/// </summary>
public static class TestRedisConnection
{
    /// <summary>
    /// Tests the Redis connection using the provided connection string.
    /// </summary>
    /// <param name="connectionString">The Redis connection string.</param>
    /// <param name="useMSI">Indicates whether to use Managed Service Identity (MSI) for authentication. Default is false.</param>
    /// <param name="principal">The Service Principal object for authentication. Default is null.</param>
    /// <returns>True if the connection is successful, otherwise false.</returns>
    public static async Task<TestNetConnectionResponse> TestConnection(string connectionString, bool useMSI = false, ServicePrincipal principal = null, string redisUsername = null)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            ConnectionMultiplexer connection = null;
            if (principal != null && principal.ClientId != null && principal.ClientSecret != null && principal.TenantId != null)
            {
                // StringWriter connectionLog = new();
                var credential = new ClientSecretCredential(principal.TenantId, principal.ClientId, principal.ClientSecret);
                var configurationOptions = await ConfigurationOptions.Parse(connectionString).ConfigureForAzureWithTokenCredentialAsync(redisUsername, credential);
                //var configurationOptions = await ConfigurationOptions.Parse(connectionString).ConfigureForAzureWithServicePrincipalAsync(clientId: principal.ClientId, principalId: principal.PrincipalId, tenantId: principal.TenantId, secret: principal.ClientSecret);
                configurationOptions.AbortOnConnectFail = true; // Fail fast for the purposes of this sample. In production code, this should remain false to retry connections on startup
                                                                //  LogTokenEvents(configurationOptions);
                connection = ConnectionMultiplexer.Connect(configurationOptions);
            }
            else if (useMSI)
            {
                var credential = new DefaultAzureCredential();
                var configurationOptions = await ConfigurationOptions.Parse(connectionString).ConfigureForAzureWithTokenCredentialAsync(redisUsername, credential);
                configurationOptions.AbortOnConnectFail = true; // Fail fast for the purposes of this sample. In production code, this should remain false to retry connections on startup
                                                                //  LogTokenEvents(configurationOptions);
                connection = ConnectionMultiplexer.Connect(configurationOptions);
            }
            else
            {
                connection = ConnectionMultiplexer.Connect(connectionString);
            }
            var status = connection.GetStatus();
            var dbs = connection.GetDatabase();
            // connection.WaitAll(connection.GetServer(connectionString).PingAsync());
            return new TestNetConnectionResponse(CheckAccessRequestResourceType.Redis, connection.IsConnected, sw.ElapsedMilliseconds);
            //return true;
        }
        catch (Exception ex)
        {
            return new TestNetConnectionResponse(CheckAccessRequestResourceType.Redis, false, ex.Message);
        }
        finally { sw.Stop(); }
    }
}
