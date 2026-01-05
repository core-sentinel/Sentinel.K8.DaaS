using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.StorageAccount;

[ConnectionCheck(Name = "StrorageAccount", Order = 3)]
public class StorageAccountConnectionCheckRequest : IRequest<TestNetConnectionResponse<StorageAccountExtraResponse>>, IBasicCheckAccessRequest
{

    public string Url { get; set; } = ".blob.core.windows.net";
    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }

    public Type AdditionalRequestRazorContentType { get => typeof(StorageAccountConnectionCheckUI); }

    public string SelectedAuthenticationType { get; set; } = "None";

    public string StorageAccountName { get; set; } = default!;

    public bool TestBlobStorage { get; set; } = true;        //   https://<storage-account>.blob.core.windows.net
    public bool TestStaticwebsite { get; set; } = false;     //(Blob Storage)   https://<storage-account>.web.core.windows.net
    public bool TestDataLakeStorage { get; set; } = false;   // https://<storage-account>.dfs.core.windows.net
    public bool TestAzureFiles { get; set; } = false;        // https://<storage-account>.file.core.windows.net
    public bool TestQueueStorage { get; set; } = true;       // https://<storage-account>.queue.core.windows.net
    public bool TestTableStorage { get; set; } = true;       // https://<storage-account>.table.core.windows.net


    public string? containerName { get; set; } = default!;
    public string? FileshareName { get; set; } = default!;
    public string? QueueName { get; set; } = default!;
    public string? TableName { get; set; } = default!;

    public string? ConnectionString { get; set; } = default!;
    public StorageAccountConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
    public string JsonDeserializationType => this.GetType().FullName;
}
