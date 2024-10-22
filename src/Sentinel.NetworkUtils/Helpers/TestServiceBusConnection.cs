using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Sentinel.NetworkUtils.Models;
using System.Diagnostics;
using System.Reflection;

namespace Sentinel.NetworkUtils.Helpers;
public static class TestServiceBusConnection
{

    public static async Task<TestNetConnectionResponse> TestConnection(string connectionString, string queueName, bool MSI = false, ServicePrincipal? principal = null)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            ServiceBusClient serviceBusClient = null;
            ServiceBusAdministrationClient serviceBusAdministrationClient = null;

            if (principal != null)
            {
                var credential = new ClientSecretCredential(principal.TenantId, principal.ClientId, principal.ClientSecret);
                // var client = new ServiceBusClient(connectionString, credential);
                serviceBusAdministrationClient = new ServiceBusAdministrationClient(connectionString, credential);
            }
            else if (MSI)
            {
                //var client = new ServiceBusClient(connectionString, new DefaultAzureCredential());
                serviceBusAdministrationClient = new ServiceBusAdministrationClient(connectionString, new DefaultAzureCredential());
            }
            else
            {
                //serviceBusClient = new ServiceBusClient(connectionString);
                serviceBusAdministrationClient = new ServiceBusAdministrationClient(connectionString);

                // var tt = new Azure.Messaging.ServiceBus.Administration.rviceBusAdministrationClient(connectionString);//.CreateFromConnectionString(connectionString);
            }
            var rts = await serviceBusAdministrationClient.GetQueueRuntimePropertiesAsync(queueName);


            var dic = rts.GetType()
              .GetProperties(BindingFlags.Instance | BindingFlags.Public)
           .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(rts, null));

            // serviceBusAdministrationClient.

            return new TestNetConnectionResponse(CheckAccessRequestResourceType.ServiceBus, true, sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            return new TestNetConnectionResponse(CheckAccessRequestResourceType.ServiceBus, false, ex.Message, sw.ElapsedMilliseconds);
        }
        finally { sw.Stop(); }
    }

}