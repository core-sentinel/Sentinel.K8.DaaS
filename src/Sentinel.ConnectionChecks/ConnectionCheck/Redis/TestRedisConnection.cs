using Azure.Identity;
using Sentinel.ConnectionChecks.Models;
using StackExchange.Redis;
using System.Diagnostics;
using System.Net;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Redis;
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
        StringWriter connectionLog = new();
        try
        {
            ConnectionMultiplexer connection = null;
            if (principal != null && principal.ClientId != null && principal.ClientSecret != null && principal.TenantId != null)
            {
                // StringWriter connectionLog = new();
                var credential = new ClientSecretCredential(principal.TenantId, principal.ClientId, principal.ClientSecret);
                var configurationOptions = await ConfigurationOptions.Parse(connectionString).ConfigureForAzureWithTokenCredentialAsync(credential);
                //  var configurationOptions = await ConfigurationOptions.Parse(connectionString).ConfigureForAzureWithTokenCredentialAsync(redisUsername, credential);
                //var configurationOptions = await ConfigurationOptions.Parse(connectionString).ConfigureForAzureWithServicePrincipalAsync(clientId: principal.ClientId, principalId: principal.PrincipalId, tenantId: principal.TenantId, secret: principal.ClientSecret);
                configurationOptions.AbortOnConnectFail = true; // Fail fast for the purposes of this sample. In production code, this should remain false to retry connections on startup
                                                                //  LogTokenEvents(configurationOptions);
                connection = ConnectionMultiplexer.Connect(configurationOptions, connectionLog);
            }
            else if (useMSI)
            {
                var credential = new DefaultAzureCredential(true);
                var configurationOptions = await ConfigurationOptions.Parse(connectionString).ConfigureForAzureWithTokenCredentialAsync(credential);
                configurationOptions.AbortOnConnectFail = true; // Fail fast for the purposes of this sample. In production code, this should remain false to retry connections on startup
                                                                //  LogTokenEvents(configurationOptions);
                connection = ConnectionMultiplexer.Connect(configurationOptions, connectionLog);
            }
            else
            {
                connection = ConnectionMultiplexer.Connect(connectionString, connectionLog);
            }

            var status = connection.GetStatus();
            var dbs = connection.GetDatabase();
            var isConnected = connection.IsConnected;
            Console.WriteLine("Redis Connection status: " + status);
            Console.WriteLine("Redis Connection dbs: " + dbs);
            Console.WriteLine("Redis Connection isConnected: " + isConnected);

            string message = "";


            //connection.GetServer(connectionString).Ping();
            IDatabase db = connection.GetDatabase();
            EndPoint endPoint = connection.GetEndPoints().First();
            RedisKey[] keys = connection.GetServer(endPoint).Keys(pattern: "*").ToArray();
            Console.WriteLine("Redis Connection keys: " + keys.Length);
            foreach (var key in keys)
            {
                Console.WriteLine(" - " + key);
                message = message + key + ", ";
            }


            // connection.WaitAll(connection.GetServer(connectionString).PingAsync());
            return new TestNetConnectionResponse(CheckAccessRequestResourceType.Redis, connection.IsConnected, message, sw.ElapsedMilliseconds);
            //return true;
        }
        catch (Exception ex)
        {
            return new TestNetConnectionResponse(CheckAccessRequestResourceType.Redis, false, ex.Message);
        }
        finally { sw.Stop(); }
    }
}
