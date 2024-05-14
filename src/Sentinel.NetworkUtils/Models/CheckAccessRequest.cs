namespace Sentinel.NetworkUtils.Models
{
    public enum CheckAccessRequestResourceType
    {
        General,
        Http,
        StrorageAccount,
        KeyVault,
        ServiceBus,
        Redis,
        EventHub,
        CosmosDB,
        SQLServer,
    }
    public class CheckAccessRequest
    {
        public string Url { get; set; } = default!;
        public int Port { get; set; }


        public CheckAccessRequestResourceType ResourceType { get; set; }

        public ServicePrincipal? ServicePrincipal { get; set; } = default!;

        public bool UseMSI { get; set; } = false;
        public StrorageAccountDetails? StrorageAccountDetails { get; set; } = default!;
        public ServiceBusDetails? ServiceBusDetails { get; set; } = default!;

        public RedisDetails? RedisDetails { get; set; } = default!;

        public SQLServerDetails? SQLServerDetails { get; set; } = default!;
        public KeyVaultDetails? KeyVaultDetails { get; set; } = default!;

        public EventHubDetails? EventHubDetails { get; set; } = default!;

        public HttpRequestDetails? HttpRequestDetails { get; set; } = default!;

        public CheckAccessRequest()
        {
            ServicePrincipal = new ServicePrincipal();
            HttpRequestDetails = new HttpRequestDetails();
            StrorageAccountDetails = new StrorageAccountDetails();
            ServiceBusDetails = new ServiceBusDetails();
            RedisDetails = new RedisDetails();
            SQLServerDetails = new SQLServerDetails();
            KeyVaultDetails = new KeyVaultDetails();

            EventHubDetails = new EventHubDetails();
        }

    }

    public class HttpRequestDetails
    {
        public string? Url { get; set; }
        public string? HttpMethod { get; set; } = default!;
    }

    public class KeyVaultDetails
    {
        public string? KeyVaultName { get; set; } = default!;
    }

    public class StrorageAccountDetails
    {
        public string? containerName { get; set; } = default!;
        public string? ConnectionString { get; set; } = default!;
    }

    public class ServiceBusDetails
    {
        public string? QueueName { get; set; } = default!;
        public string? ConnectionString { get; set; } = default!;
    }

    public class RedisDetails
    {
        public string? ConnectionString { get; set; } = default!;
        public string? RedisUserName { get; set; } = default!;
    }

    public class SQLServerDetails
    {
        public string? ConnectionString { get; set; } = default!;
    }

    public class EventHubDetails
    {
        public string? ConnectionString { get; set; } = default!;
        public string? EventHubName { get; set; } = default!;
    }
}