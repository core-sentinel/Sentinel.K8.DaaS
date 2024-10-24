﻿using MediatR;
using Sentinel.ConnectionChecks.Models;

namespace Sentinel.ConnectionChecks.ConnectionCheck.TcpPing;

public class TcpPingConnectionCheckRequest : IRequest<TestNetConnectionResponse>, IBasicCheckAccessRequest
{
    public string Url { get; set; } = "";

    public string Domain { get; set; } = "";

    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }
    public string Protocol { get; set; } = "TCP";
    public TcpPingConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}
