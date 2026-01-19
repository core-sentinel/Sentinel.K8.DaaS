using Sentinel.ConnectionChecks.ConnectionCheck.AzureAppConfig;
using Sentinel.ConnectionChecks.ConnectionCheck.CosmosDB;
using Sentinel.ConnectionChecks.ConnectionCheck.EventHub;
using Sentinel.ConnectionChecks.ConnectionCheck.Http;
using Sentinel.ConnectionChecks.ConnectionCheck.KeyVault;
using Sentinel.ConnectionChecks.ConnectionCheck.Redis;
using Sentinel.ConnectionChecks.ConnectionCheck.ServiceBus;
using Sentinel.ConnectionChecks.ConnectionCheck.SQL;
using Sentinel.ConnectionChecks.ConnectionCheck.StorageAccount;
using Sentinel.ConnectionChecks.ConnectionCheck.TcpPing;
using Sentinel.ConnectionChecks.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Sentinel.ConnectionChecks;

public class HealthCheckCronRequest
{
    public string Cron { get; set; } = string.Empty;
    public string ApplicationInsightsConnectionstring { get; set; } = string.Empty; 

    public List<TcpPingConnectionCheckRequest> TcpPing { get; set; } = new();
    public List<HttpConnectionCheckRequest> Http { get; set; } = new();
    public List<StorageAccountConnectionCheckRequest> StrorageAccount { get; set; } = new();
    public List<KeyVaultConnectionCheckRequest> KeyVault { get; set; } = new();
    public List<ServiceBusConnectionCheckRequest> ServiceBus { get; set; } = new();
    public List<RedisConnectionCheckRequest> Redis { get; set; } = new();
    public List<EventHubConnectionCheckRequest> EventHub { get; set; } = new();
    public List<CosmosDBConnectionCheckRequest> CosmosDB { get; set; } = new();
    public List<SQLConnectionCheckRequest> SQLServer { get; set; } = new();
    public List<AzureAppConfigConnectionCheckRequest> AzureAppConfig { get; set; } = new();

    

    public Dictionary<string, IReadOnlyList<IBasicCheckAccessRequest>> HealthChecks
    {
        get
        {
            Dictionary<string, IReadOnlyList<IBasicCheckAccessRequest>> hc = new();
            hc.Add("TcpPing", TcpPing);
            hc.Add("Http", Http);
            hc.Add("StrorageAccount", StrorageAccount);
            hc.Add("KeyVault", KeyVault);
            hc.Add("ServiceBus", ServiceBus);
            hc.Add("Redis", Redis);
            hc.Add("EventHub", EventHub);
            hc.Add("CosmosDB", CosmosDB);
            hc.Add("SQLServer", SQLServer);
            hc.Add("AzureAppConfig", AzureAppConfig);

            return hc;
        }
    }

    public IBasicCheckAccessRequest AddnewRequest(string categoryName)
    {

        if(categoryName == "TcpPing")
        {
            var req = new TcpPingConnectionCheckRequest { Url="new item"};
            TcpPing.Add(req);
            return req;
        }else if(categoryName == "Http")
        {
            var req = new HttpConnectionCheckRequest { Url = "new item" };
            Http.Add(req);
            return req;
        }else if(categoryName == "StrorageAccount")
        {
            var req= new StorageAccountConnectionCheckRequest { Url = "new item" };
            StrorageAccount.Add(req);
            return req;
        }else if(categoryName == "KeyVault")
        {
            var req= new KeyVaultConnectionCheckRequest { Url = "new item" };
            KeyVault.Add(req);
            return req;
        }
        else if(categoryName == "ServiceBus")
        {
            var req = new ServiceBusConnectionCheckRequest { Url = "new item" };
            ServiceBus.Add(req);
            return req;
        }
        else if( categoryName == "Redis")
        {
            var req = new RedisConnectionCheckRequest { Url = "new item" };
            Redis.Add(req);
            return req;
        }
        else if(categoryName == "EventHub")
        {
            var req = new EventHubConnectionCheckRequest { Url = "new item" };
            EventHub.Add(req);
            return req;
        }
        else if( categoryName == "CosmosDB")
        {
            var req = new CosmosDBConnectionCheckRequest { Url = "new item" };
            CosmosDB.Add(req);
            return req;
        }
        else if(categoryName == "SQLServer")
        {
            var req = new SQLConnectionCheckRequest { Url = "new item" };
            SQLServer.Add(req);
            return req;
        }
        else if(categoryName == "AzureAppConfig")
        {
            var req = new AzureAppConfigConnectionCheckRequest { Url = "new item" };
            AzureAppConfig.Add(req);
            return req;
        }
        else
        {
            throw new ArgumentException("category type doesn't match with the types");
        }
  
}
}

public class HealthCheckCronRequestConfiguration
{
    public HealthCheckCronRequest HealthChecks { get; set; } = new();
}