using Azure.Identity;
using Microsoft.Data.SqlClient;
using Sentinel.ConnectionChecks.Models;
using System.Diagnostics;
using System.Text;

namespace Sentinel.ConnectionChecks.ConnectionCheck.SQL;
internal static class TestSQLConnection
{
    public static Task<TestNetConnectionResponse> TestConnection(SQLConnectionCheckRequest request)
    {
        var sw = Stopwatch.StartNew();
        StringBuilder stringBuilder = new StringBuilder();
        try
        {
            using (var connection = new SqlConnection(request.ConnectionString))
            {

                if (request.UseMSI)
                {
                    // var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = clientId });
                    var credential = new DefaultAzureCredential();
                    var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
                    connection.AccessToken = token.Token;
                }
                else if (request.ServicePrincipal?.ClientId != null)
                {
                    var credential = new ClientSecretCredential(request.ServicePrincipal.TenantId, request.ServicePrincipal.ClientId, request.ServicePrincipal.ClientSecret);
                    var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
                    connection.AccessToken = token.Token;
                }

                connection.Open();
                connection.Database.ToString();
                stringBuilder.AppendLine(connection.Database + " Tables");
                stringBuilder.AppendLine("=======================");
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "select id,name from sysobjects where xtype='U'";
                //command.CommandText = "select schema_name(t.schema_id) as schema_name, t.name as table_name, t.create_date, t.modify_date from sys.tables t order by schema_name,table_name;";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string tableInfo = "id : " + reader[0].ToString() + " name : " + reader[1].ToString();
                    stringBuilder.AppendLine(tableInfo);
                }

                return Task.FromResult(new TestNetConnectionResponse("SQLServer", true, stringBuilder.ToString(), sw.ElapsedMilliseconds));
            }
        }
        catch (Exception ex)
        {
            return Task.FromResult(new TestNetConnectionResponse("SQLServer", false, ex.Message, sw.ElapsedMilliseconds));
        }
        finally { sw.Stop(); }
    }
}

