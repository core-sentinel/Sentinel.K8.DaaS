using System;
using System.Collections.Generic;
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
                return new TestNetConnectionResponse(true);
            }
        }
        catch (Exception ex)
        {
            return new TestNetConnectionResponse(false, ex.Message);
        }
    }
}
