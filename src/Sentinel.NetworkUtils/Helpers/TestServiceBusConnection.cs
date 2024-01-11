using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Azure;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.Helpers;
public static class TestServiceBusConnection
{

    public static async Task<TestNetConnectionResponse> TestConnection(string connectionString, string queueName, bool MSI = false, ServicePrincipal? principal = null)
    {
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
            // serviceBusAdministrationClient.

            return new TestNetConnectionResponse { IsConnected = true };
        }
        catch (Exception ex)
        {
            return new TestNetConnectionResponse { IsConnected = false, Message = ex.Message };
        }
    }

}