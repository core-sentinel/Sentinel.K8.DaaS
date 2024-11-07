using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Sentinel.ConnectionChecks.ConnectionCheck.TcpPing;
using Sentinel.ConnectionChecks.Models;
using System.Diagnostics;
using System.Text;

namespace Sentinel.ConnectionChecks.ConnectionCheck.StorageAccount;

internal class TestStorageAccountConnection
{

    StorageAccountConnectionCheckRequest _request;
    public TestStorageAccountConnection(StorageAccountConnectionCheckRequest request)
    {
        _request = request;
    }

    public async Task<TestNetConnectionResponse<StorageAccountExtraResponse>> TestConnection()
    {
        var sw = Stopwatch.StartNew();

        try
        {
            StringBuilder sb = new StringBuilder();

            var result = new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, true, sb.ToString(), sw.ElapsedMilliseconds);
            result.ExtraResultRazorContentType = typeof(StorageAccountConnectionCheckUIResult);


            //blah


            List<StorageAccountObjectType> urls = new List<StorageAccountObjectType>();

            if (_request.TestBlobStorage)
                urls.Add(StorageAccountObjectType.Blob(_request.StorageAccountName, _request.containerName));
            if (_request.TestQueueStorage)
                urls.Add(StorageAccountObjectType.QueueStorage(_request.StorageAccountName, _request.QueueName));
            if (_request.TestTableStorage)
                urls.Add(StorageAccountObjectType.TableStorage(_request.StorageAccountName, _request.TableName));
            if (_request.TestAzureFiles)
                urls.Add(StorageAccountObjectType.AzureFiles(_request.StorageAccountName, _request.FileshareName));
            if (_request.TestStaticwebsite)
                urls.Add(StorageAccountObjectType.Staticwebsite(_request.StorageAccountName));
            if (_request.TestDataLakeStorage)
                urls.Add(StorageAccountObjectType.DataLakeStorage(_request.StorageAccountName));

            foreach (var item in urls)
            {

                var res = await TestStorageConnection(item);
                //var res = await TestUrlConnection(item);
                result.ExtraResult.Add(res.Key, res.Value);
            }


            return result;
        }
        catch (Exception ex)
        {
            //throw e;
            return new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, false, ex.Message, sw.ElapsedMilliseconds);

        }
        finally
        {
            sw.Reset();
            sw.Stop();
        }
    }


    public async Task<KeyValuePair<StorageAccountObjectType, TestNetConnectionResponse>> TestStorageConnection(StorageAccountObjectType req)
    {
        TestNetConnectionResponse netResult = null;
        switch (req.ObjectType)
        {
            case StorageAccountEndpointType.Blob:
                netResult = await testBlobConnection(req);
                break;
            case StorageAccountEndpointType.TableStorage:
                netResult = await TestTableConnection(req);
                break;
            case StorageAccountEndpointType.QueueStorage:
                netResult = await TestQueueConnection(req);
                break;
            case StorageAccountEndpointType.AzureFiles:
                netResult = await TestFileshareeConnection(req);
                break;
        }

        return new KeyValuePair<StorageAccountObjectType, TestNetConnectionResponse>(req, netResult);
    }

    private async Task<TestNetConnectionResponse> testBlobConnection(StorageAccountObjectType req)
    {
        var sw = Stopwatch.StartNew();
        StringBuilder sb = new StringBuilder();
        string IPAddress = "";
        try
        {

            var ip = await TestUrlConnection(req);
            IPAddress = ip.IPAddress;
            if (string.IsNullOrWhiteSpace(req.SubComponentName))
            {
                sb.AppendLine("Container Name Not Found, Listing Containers");
                var client = GetBlobServiceClient(req);
                var resultSegment = client.GetBlobContainersAsync(Azure.Storage.Blobs.Models.BlobContainerTraits.Metadata, Azure.Storage.Blobs.Models.BlobContainerStates.None).AsPages(default, 100);

                await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
                {
                    foreach (BlobContainerItem containerItem in containerPage.Values)
                    {
                        Console.WriteLine("Container name: {0}", containerItem.Name);
                        sb.AppendLine(containerItem.Name);
                    }
                }
            }
            else
            {
                var client = GetBlobContainerClient(req);
            }

            return new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, true, sb.ToString(), sw.ElapsedMilliseconds, IPAddress);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, false, ex.Message, sw.ElapsedMilliseconds, IPAddress);
        }
    }



    private async Task<TestNetConnectionResponse> TestQueueConnection(StorageAccountObjectType req)
    {
        var sw = Stopwatch.StartNew();
        StringBuilder sb = new StringBuilder();
        string IPAddress = "";
        try
        {
            var ip = await TestUrlConnection(req);
            IPAddress = ip.IPAddress;

            if (string.IsNullOrWhiteSpace(req.SubComponentName))
            {
                sb.AppendLine("Queue Name Not Found, Listing Queues");
                var client = GetQueueServiceClient(req);
                var resultSegment = client.GetQueuesAsync(Azure.Storage.Queues.Models.QueueTraits.Metadata, null).AsPages(default, 100);

                await foreach (Azure.Page<QueueItem> containerPage in resultSegment)
                {
                    foreach (QueueItem containerItem in containerPage.Values)
                    {
                        Console.WriteLine("Queue name: {0}", containerItem.Name);
                        sb.AppendLine(containerItem.Name);
                    }
                }
            }
            else
            {
                //var client = GetBlobContainerClient(req);
            }

            return new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, true, sb.ToString(), sw.ElapsedMilliseconds, IPAddress);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, false, ex.Message, sw.ElapsedMilliseconds, IPAddress);
        }
    }


    private async Task<TestNetConnectionResponse> TestTableConnection(StorageAccountObjectType req)
    {
        var sw = Stopwatch.StartNew();
        StringBuilder sb = new StringBuilder();
        string IPAddress = "";
        try
        {
            var ip = await TestUrlConnection(req);
            IPAddress = ip.IPAddress;

            if (string.IsNullOrWhiteSpace(req.SubComponentName))
            {
                sb.AppendLine("Table Name Not Found, Listing Tables");
                var client = GetTableServiceClient(req);
                var resultSegment = client.QueryAsync().AsPages(default, 100);

                await foreach (Azure.Page<TableItem> containerPage in resultSegment)
                {
                    foreach (TableItem containerItem in containerPage.Values)
                    {
                        Console.WriteLine("Table name: {0}", containerItem.Name);
                        sb.AppendLine(containerItem.Name);
                    }
                }
            }
            else
            {
                //var client = GetBlobContainerClient(req);
            }

            return new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, true, sb.ToString(), sw.ElapsedMilliseconds, IPAddress);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, false, ex.Message, sw.ElapsedMilliseconds, IPAddress);
        }
    }

    private async Task<TestNetConnectionResponse> TestFileshareeConnection(StorageAccountObjectType req)
    {
        var sw = Stopwatch.StartNew();
        StringBuilder sb = new StringBuilder();
        string IPAddress = "";
        try
        {
            var ip = await TestUrlConnection(req);
            IPAddress = ip.IPAddress;

            if (string.IsNullOrWhiteSpace(req.SubComponentName))
            {
                sb.AppendLine("ShareItem Name Not Found, Listing Shares");
                var client = GetShareServiceClient(req);
                var resultSegment = client.GetSharesAsync().AsPages(default, 100);

                await foreach (Azure.Page<ShareItem> containerPage in resultSegment)
                {
                    foreach (ShareItem containerItem in containerPage.Values)
                    {
                        Console.WriteLine("ShareItem name: {0}", containerItem.Name);
                        sb.AppendLine(containerItem.Name);
                    }
                }
            }
            else
            {
                //var client = GetBlobContainerClient(req);
            }

            return new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, true, sb.ToString(), sw.ElapsedMilliseconds, IPAddress);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new TestNetConnectionResponse<StorageAccountExtraResponse>(CheckAccessRequestResourceType.StrorageAccount, false, ex.Message, sw.ElapsedMilliseconds, IPAddress);
        }
    }



    private BlobServiceClient GetBlobServiceClient(StorageAccountObjectType req)
    {
        BlobServiceClient client;
        if (_request.UseMSI)
            client = new BlobServiceClient(new Uri(req.GetUrl()), new DefaultAzureCredential());
        else if (_request.ServicePrincipal?.ClientId != null)
            client = new BlobServiceClient(new Uri(req.GetUrl()), new ClientSecretCredential(_request.ServicePrincipal.TenantId, _request.ServicePrincipal.ClientId, _request.ServicePrincipal.ClientSecret));
        else
            client = new BlobServiceClient(connectionString: _request.ConnectionString);
        return client;
    }

    private BlobContainerClient GetBlobContainerClient(StorageAccountObjectType req)
    {
        BlobContainerClient client;
        if (_request.UseMSI)
            client = new BlobContainerClient(new Uri(req.GetUrl()), new DefaultAzureCredential());
        else if (_request.ServicePrincipal?.ClientId != null)
            client = new BlobContainerClient(new Uri(req.GetUrl()), new ClientSecretCredential(_request.ServicePrincipal.TenantId, _request.ServicePrincipal.ClientId, _request.ServicePrincipal.ClientSecret));
        else
            client = new BlobContainerClient(connectionString: _request.ConnectionString, _request.containerName);
        return client;
    }


    private ShareServiceClient GetShareServiceClient(StorageAccountObjectType req)
    {
        ShareServiceClient client;
        if (_request.UseMSI)
            client = new ShareServiceClient(new Uri(req.GetUrl()), new DefaultAzureCredential());
        else if (_request.ServicePrincipal?.ClientId != null)
            client = new ShareServiceClient(new Uri(req.GetUrl()), new ClientSecretCredential(_request.ServicePrincipal.TenantId, _request.ServicePrincipal.ClientId, _request.ServicePrincipal.ClientSecret));
        else
            client = new ShareServiceClient(connectionString: _request.ConnectionString);
        return client;
    }

    private QueueServiceClient GetQueueServiceClient(StorageAccountObjectType req)
    {
        QueueServiceClient client;
        if (_request.UseMSI)
            client = new QueueServiceClient(new Uri(req.GetUrl()), new DefaultAzureCredential());
        else if (_request.ServicePrincipal?.ClientId != null)
            client = new QueueServiceClient(new Uri(req.GetUrl()), new ClientSecretCredential(_request.ServicePrincipal.TenantId, _request.ServicePrincipal.ClientId, _request.ServicePrincipal.ClientSecret));
        else
            client = new QueueServiceClient(connectionString: _request.ConnectionString);
        return client;
    }

    private TableServiceClient GetTableServiceClient(StorageAccountObjectType req)
    {
        TableServiceClient client;
        if (_request.UseMSI)
            client = new TableServiceClient(new Uri(req.GetUrl()), new DefaultAzureCredential());
        else if (_request.ServicePrincipal?.ClientId != null)
            client = new TableServiceClient(new Uri(req.GetUrl()), new ClientSecretCredential(_request.ServicePrincipal.TenantId, _request.ServicePrincipal.ClientId, _request.ServicePrincipal.ClientSecret));
        else
            client = new TableServiceClient(connectionString: _request.ConnectionString);
        return client;
    }

    //private BlobContainerClient GetBlobContainerClient(StorageAccountObjectType req)
    //{
    //    BlobContainerClient client;
    //    if (_request.UseMSI)
    //        client = new BlobContainerClient(new Uri(req.GetUrl()), new DefaultAzureCredential());
    //    else if (_request.ServicePrincipal?.ClientId != null)
    //        client = new BlobContainerClient(new Uri(req.GetUrl()), new ClientSecretCredential(_request.ServicePrincipal.TenantId, _request.ServicePrincipal.ClientId, _request.ServicePrincipal.ClientSecret));
    //    else
    //        client = new BlobContainerClient(connectionString: _request.ConnectionString, _request.containerName);
    //    return client;
    //}


    public static async Task<TestNetConnectionResponse> TestUrlConnection(StorageAccountObjectType Domain)
    {
        var ipaddress = await NSLookup.GetIPAddress(Domain.GetDomain());
        var netResult = await TestTcpPing.TestTcpConnection(ipaddress, 443);
        netResult.IPAddress = ipaddress;
        return netResult;
    }
}
