using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Sentinel.ConnectionChecks.Models;
using System.Diagnostics;
using System.Text;

namespace Sentinel.ConnectionChecks.ConnectionCheck.KeyVault;

public static class TestKeyVaultConnection
{

    public static async Task<TestNetConnectionResponse> TestConnection(KeyVaultConnectionCheckRequest request)
    {
        var sw = Stopwatch.StartNew();
        var keyVaultUrl = $"https://{request.KeyVaultName}.vault.azure.net";
        SecretClient client = null;
        if (request.ServicePrincipal != null && request.ServicePrincipal.ClientId != null && request.ServicePrincipal.ClientSecret != null && request.ServicePrincipal.TenantId != null)
        {
            var credential = new ClientSecretCredential(request.ServicePrincipal.TenantId, request.ServicePrincipal.ClientId, request.ServicePrincipal.ClientSecret);
            client = new SecretClient(new Uri(keyVaultUrl), credential);
        }
        else
        {
            client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
        }


        try
        {

            AsyncPageable<SecretProperties> allSecrets = client.GetPropertiesOfSecretsAsync();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("SECRETS");
            stringBuilder.AppendLine("=======================");
            await foreach (SecretProperties secret in allSecrets)
            {
                Console.WriteLine($"IterateSecretsWithAwaitForeachAsync: {secret.Name}");

                var response = await client.GetSecretAsync(secret.Name, cancellationToken: new CancellationToken()).ConfigureAwait(false);
                stringBuilder.AppendLine(secret.Name);
                Console.WriteLine(response.Value.Value?.Length.ToString());


            }
            return new TestNetConnectionResponse("KeyVault", true, stringBuilder.ToString(), sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            return new TestNetConnectionResponse("KeyVault", false, ex.Message, sw.ElapsedMilliseconds);
        }
        finally { sw.Stop(); }
    }
}