﻿using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.CosmosDB;

public class CosmosDBConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = ".documents.azure.com";
    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }

    public string DatabaseName { get; set; } = "";
    public string ContainerName { get; set; } = "";

    public string ConnectionString { get; set; } = string.Empty;

    public CosmosDBConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}

