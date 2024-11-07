using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.StorageAccount;


public enum StorageAccountEndpointType
{
    Blob,
    Staticwebsite,
    DataLakeStorage,
    AzureFiles,
    QueueStorage,
    TableStorage
}


public class StorageAccountObjectType
{
    public StorageAccountObjectType(StorageAccountEndpointType objectType, string accountName, string subComponentName = null)
    {
        ObjectType = objectType;
        AccountName = accountName;
        SubComponentName = subComponentName;
    }

    public StorageAccountEndpointType ObjectType { get; }
    public string AccountName { get; }
    public string? SubComponentName { get; }


    public static StorageAccountObjectType Blob(string AccountName, string? subComponentName = null) => new StorageAccountObjectType(StorageAccountEndpointType.Blob, AccountName, subComponentName);
    public static StorageAccountObjectType QueueStorage(string AccountName, string? subComponentName = null) => new StorageAccountObjectType(StorageAccountEndpointType.QueueStorage, AccountName, subComponentName);
    public static StorageAccountObjectType TableStorage(string AccountName, string? subComponentName = null) => new StorageAccountObjectType(StorageAccountEndpointType.TableStorage, AccountName, subComponentName);
    public static StorageAccountObjectType AzureFiles(string AccountName, string? subComponentName = null) => new StorageAccountObjectType(StorageAccountEndpointType.AzureFiles, AccountName, subComponentName);
    public static StorageAccountObjectType Staticwebsite(string AccountName, string? subComponentName = null) => new StorageAccountObjectType(StorageAccountEndpointType.Staticwebsite, AccountName, subComponentName);
    public static StorageAccountObjectType DataLakeStorage(string AccountName, string? subComponentName = null) => new StorageAccountObjectType(StorageAccountEndpointType.DataLakeStorage, AccountName, subComponentName);




    public string GetDomain()
    {
        return ObjectType switch
        {
            StorageAccountEndpointType.Blob => $"{AccountName}.blob.core.windows.net",
            StorageAccountEndpointType.QueueStorage => $"{AccountName}.queue.core.windows.net",
            StorageAccountEndpointType.TableStorage => $"{AccountName}.table.core.windows.net",
            StorageAccountEndpointType.AzureFiles => $"{AccountName}.file.core.windows.net",
            StorageAccountEndpointType.Staticwebsite => $"{AccountName}.web.core.windows.net",
            StorageAccountEndpointType.DataLakeStorage => $"{AccountName}.dfs.core.windows.net",
            _ => throw new System.NotImplementedException()
        };
    }


    public string GetUrl()
    {
        return $"https://{GetDomain()}";
    }

    public override string ToString()
    {
        return $"{ObjectType} - {AccountName}";
    }
}

public class StorageAccountExtraResponse : Dictionary<StorageAccountObjectType, TestNetConnectionResponse>
{

    // public Dictionary<string, TestNetConnectionResponse> ExtraResponse { get; set; } = new Dictionary<string, TestNetConnectionResponse>();
}


//public class StorageAccounNetConnectionResponse : TestNetConnectionResponse
//{

//}