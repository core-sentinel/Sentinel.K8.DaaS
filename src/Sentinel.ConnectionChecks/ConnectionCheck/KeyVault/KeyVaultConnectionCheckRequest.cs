﻿using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.KeyVault;

public class KeyVaultConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = ".vault.azure.net";
    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }

    public string? KeyVaultName { get; set; } = default!;

    public KeyVaultConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
