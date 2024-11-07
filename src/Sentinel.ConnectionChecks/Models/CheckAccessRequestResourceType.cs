namespace Sentinel.ConnectionChecks.Models;

public enum CheckAccessRequestResourceType
{
    TcpPing,
    Http,
    StrorageAccount,
    KeyVault,
    ServiceBus,
    Redis,
    EventHub,
    CosmosDB,
    SQLServer,
    AzureAppConfig,
}

public static class RequestResourceTypeHelper
{
    public static Dictionary<int, string> Categories = Enum.GetValues(typeof(CheckAccessRequestResourceType))
        .Cast<CheckAccessRequestResourceType>().ToDictionary(t => (int)t, t => t.ToString());
}
