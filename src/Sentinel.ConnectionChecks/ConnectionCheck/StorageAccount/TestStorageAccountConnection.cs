using Azure.Identity;
using Azure.Storage.Blobs;
using Sentinel.ConnectionChecks.Models;
using System.Diagnostics;
using System.Text;

namespace Sentinel.ConnectionChecks.ConnectionCheck.StorageAccount
{
    internal static class TestStorageAccountConnection
    {

        public static async Task<TestNetConnectionResponse> TestConnection(StorageAccountConnectionCheckRequest request)
        {
            var sw = Stopwatch.StartNew();

            try
            {

                BlobContainerClient containerClient = null;
                if (request.ServicePrincipal != null && request.ServicePrincipal.ClientId != null && request.ServicePrincipal.ClientSecret != null && request.ServicePrincipal.TenantId != null)
                {
                    //  var url = new Uri(connectionString);

                    string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
                                                    request.ConnectionString,
                                                    request.containerName);
                    var credential = new ClientSecretCredential(request.ServicePrincipal.TenantId, request.ServicePrincipal.ClientId, request.ServicePrincipal.ClientSecret);
                    containerClient = new BlobContainerClient(new Uri(containerEndpoint), credential);
                }
                else if (request.UseMSI == true)
                {
                    string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
                                                   request.ConnectionString,
                                                   request.containerName);
                    containerClient = new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());
                }
                else
                {
                    containerClient = new BlobContainerClient(connectionString: request.ConnectionString, request.containerName);
                }
                await containerClient.CreateIfNotExistsAsync();
                var blobs = containerClient.GetBlobs();
                StringBuilder sb = new StringBuilder();
                foreach (var blob in blobs)
                {
                    Console.WriteLine(blob.Name);
                    sb.AppendLine(blob.Name);
                }

                return new TestNetConnectionResponse(CheckAccessRequestResourceType.StrorageAccount, true, sb.ToString(), sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                //throw e;
                return new TestNetConnectionResponse(CheckAccessRequestResourceType.StrorageAccount, false, ex.Message, sw.ElapsedMilliseconds);

            }
            finally
            {
                sw.Reset();
                sw.Stop();
            }
        }
    }
}
