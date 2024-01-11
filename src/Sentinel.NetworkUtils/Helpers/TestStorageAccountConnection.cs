using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc.Routing;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.Helpers;

public static class TestStorageAccountConnection
{
    public static async Task<TestNetConnectionResponse> TestConnection(string connectionString, string containerName, bool useMSI = false, ServicePrincipal principal = null)
    {
        try
        {

            BlobContainerClient containerClient = null;
            if (principal != null && principal.ClientId != null && principal.ClientSecret != null && principal.TenantId != null)
            {
                //  var url = new Uri(connectionString);

                string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
                                                connectionString,
                                                containerName);
                var credential = new ClientSecretCredential(principal.TenantId, principal.ClientId, principal.ClientSecret);
                containerClient = new BlobContainerClient(new Uri(containerEndpoint), credential);
            }
            else if (useMSI == true)
            {
                string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
                                               connectionString,
                                               containerName);
                containerClient = new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());
            }
            else
            {
                containerClient = new BlobContainerClient(connectionString, containerName);
            }
            await containerClient.CreateIfNotExistsAsync();
            return new TestNetConnectionResponse(true);
        }
        catch (Exception ex)
        {
            //throw e;
            return new TestNetConnectionResponse(false, ex.Message);
        }
    }
}
