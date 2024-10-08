﻿using MediatR;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.Redis;
//public record RedisConnectionCheckCommand(string ConnectionString, bool UseMSI, ServicePrincipal? Principal, string? RedisUsername) : IRequest<RedisConnectionCheckResponse>;

public class RedisConnectionCheckRequest : IRequest<TestNetConnectionResponse>
{
    private ServicePrincipal? servicePrincipal;

    public RedisConnectionCheckRequest()
    {

    }

    public RedisConnectionCheckRequest(string? connectionString, bool useMSI, ServicePrincipal? servicePrincipal, string? redisUserName)
    {
        ConnectionString = connectionString;
        UseMSI = useMSI;
        this.servicePrincipal = servicePrincipal;
        RedisUsername = redisUserName;
    }

    public string ConnectionString { get; set; }
    public bool UseMSI { get; set; }
    public ServicePrincipal? Principal { get; set; }
    public string? RedisUsername { get; set; }
}


