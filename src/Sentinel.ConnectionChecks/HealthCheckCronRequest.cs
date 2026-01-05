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

namespace Sentinel.ConnectionChecks;

public class HealthCheckCronRequest
{
    public string Cron { get; set; } = string.Empty;
    public Dictionary<string, TcpPingConnectionCheckRequest> TcpPing { get; set; } = new();
    public Dictionary<string, HttpConnectionCheckRequest> Http { get; set; } = new();
    public Dictionary<string, StorageAccountConnectionCheckRequest> StrorageAccount { get; set; } = new();
    public Dictionary<string, KeyVaultConnectionCheckRequest> KeyVault { get; set; } = new();
    public Dictionary<string, ServiceBusConnectionCheckRequest> ServiceBus { get; set; } = new();
    public Dictionary<string, RedisConnectionCheckRequest> Redis { get; set; } = new();
    public Dictionary<string, EventHubConnectionCheckRequest> EventHub { get; set; } = new();
    public Dictionary<string, CosmosDBConnectionCheckRequest> CosmosDB { get; set; } = new();
    public Dictionary<string, SQLConnectionCheckRequest> SQLServer { get; set; } = new();
    public Dictionary<string, AzureAppConfigConnectionCheckRequest> AzureAppConfig { get; set; } = new();

    // public static readonly HashSet<string> SupportedResourceTypes = new();

    public Dictionary<string, List<IBasicCheckAccessRequest>> HealthChecks { get; set; } = new();
}

public class HealthCheckCronRequestConfiguration
{
    public HealthCheckCronRequest HealthChecks { get; set; } = new();
}