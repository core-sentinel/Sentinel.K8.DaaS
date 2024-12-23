using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.CosmosDB;

[ConnectionCheck(Name = "CosmosDB", Order = 8)]
public class CosmosDBConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = ".documents.azure.com";
    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }
    public Type AdditionalRequestRazorContentType { get => typeof(CosmosDBConnectionCheckUI); }
    public string SelectedAuthenticationType { get; set; } = "None";
    public string DatabaseName { get; set; } = "";
    public string ContainerName { get; set; } = "";

    public string ConnectionString { get; set; } = string.Empty;

    public CosmosDBConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}

