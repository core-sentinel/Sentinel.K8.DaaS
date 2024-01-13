using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using Sentinel.NetworkUtils.Models;

namespace Sentinel.NetworkUtils.Helpers;
public static class TestSQLConnection
{

    public static TestNetConnectionResponse TestConnection(string connectionString, bool useMSI = false, ServicePrincipal principal = null)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {

                if (useMSI)
                {
                    // var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = clientId });
                    var credential = new DefaultAzureCredential();
                    var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
                    connection.AccessToken = token.Token;
                }
                else if (principal != null)
                {
                    var credential = new ClientSecretCredential(principal.TenantId, principal.ClientId, principal.ClientSecret);
                    var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
                    connection.AccessToken = token.Token;
                }

                connection.Open();
                connection.Database.ToString();
                return new TestNetConnectionResponse(CheckAccessRequestResourceType.SQLServer, true, sw.ElapsedMilliseconds);
            }
        }
        catch (Exception ex)
        {
            return new TestNetConnectionResponse(CheckAccessRequestResourceType.SQLServer, false, ex.Message, sw.ElapsedMilliseconds);
        }
        finally { sw.Stop(); }
    }
}
