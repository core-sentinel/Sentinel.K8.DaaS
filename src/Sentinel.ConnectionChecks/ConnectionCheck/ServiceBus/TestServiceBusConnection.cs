using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Sentinel.ConnectionChecks.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Sentinel.ConnectionChecks.ConnectionCheck.ServiceBus;
internal static class TestServiceBusConnection
{


    public static async Task<TestNetConnectionResponse> TestConnection(ServiceBusConnectionCheckRequest request)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            ServiceBusClient serviceBusClient = null;
            ServiceBusAdministrationClient serviceBusAdministrationClient = null;

            if (request.ServicePrincipal.ClientId != null)
            {
                var credential = new ClientSecretCredential(request.ServicePrincipal.TenantId, request.ServicePrincipal.ClientId, request.ServicePrincipal.ClientSecret);
                // var client = new ServiceBusClient(connectionString, credential);
                serviceBusAdministrationClient = new ServiceBusAdministrationClient(request.ConnectionString, credential);
            }
            else if (request.UseMSI)
            {
                //var client = new ServiceBusClient(connectionString, new DefaultAzureCredential());
                serviceBusAdministrationClient = new ServiceBusAdministrationClient(request.ConnectionString, new DefaultAzureCredential());
            }
            else
            {
                //serviceBusClient = new ServiceBusClient(connectionString);
                serviceBusAdministrationClient = new ServiceBusAdministrationClient(request.ConnectionString);

                // var tt = new Azure.Messaging.ServiceBus.Administration.rviceBusAdministrationClient(connectionString);//.CreateFromConnectionString(connectionString);
            }
            var rts = await serviceBusAdministrationClient.GetQueueRuntimePropertiesAsync(request.QueueName);
            //  rts.Value

            var dic = typeof(QueueRuntimeProperties)
              .GetProperties(BindingFlags.Instance | BindingFlags.Public)
           .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(rts.Value, null).ToString());

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in dic)
            {
                stringBuilder.AppendLine(item.Key + " : " + item.Value + "\n");
            }

            // serviceBusAdministrationClient.

            return new TestNetConnectionResponse("ServiceBus", true, stringBuilder.ToString(), sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            return new TestNetConnectionResponse("ServiceBus", false, ex.Message, sw.ElapsedMilliseconds);
        }
        finally { sw.Stop(); }
    }
}
