using Azure.Core;
using Azure.Identity;

namespace Sentinel.Core.TokenGenerator
{
    public static class AzureTokenGenerator
    {
        public static string GenerateToken(string clientId, string tenantId, string clientSecret)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrEmpty(tenantId))
                throw new ArgumentNullException(nameof(tenantId));
            if (string.IsNullOrEmpty(clientSecret))
                throw new ArgumentNullException(nameof(clientSecret));

            try
            {
                // Create a client secret credential
                var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                // Define the scope for the Azure Management API
                string[] scopes = new[] { "https://management.azure.com/.default" };

                // Get the access token
                var tokenRequestContext = new TokenRequestContext(scopes);
                var token = credential.GetToken(tokenRequestContext);

                return token.Token;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to generate Azure token: {ex.Message}", ex);
            }
        }

        public static string GenerateToken(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentNullException(nameof(clientId));

            try
            {
                // DefaultAzureCredential tries multiple authentication methods
                // in a specific order (environment, managed identity, VS Code, Azure CLI, etc.)
                var options = new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = clientId, // Only if using managed identity
                    // Other credential-specific client IDs can be set here
                };
                var credential = new DefaultAzureCredential(options);

                // Use the provided audience ID for the scope
                string[] scopes = new[] { $"{clientId}/.default" };

                // Get the access token
                var tokenRequestContext = new TokenRequestContext(scopes);
                var token = credential.GetToken(tokenRequestContext);

                return token.Token;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to generate Azure token using default credentials: {ex.Message}", ex);
            }
        }

        internal static string? DecodeToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                // JWT tokens consist of three parts: header.payload.signature
                var parts = token.Split('.');
                if (parts.Length != 3)
                    return null;

                // The payload is the second part of the token
                var payload = parts[1];

                // The payload is base64url encoded
                // Replace characters for regular base64 decoding
                string base64 = payload.Replace('-', '+').Replace('_', '/');

                // Add padding if needed
                switch (base64.Length % 4)
                {
                    case 2: base64 += "=="; break;
                    case 3: base64 += "="; break;
                }

                // Decode the base64 string
                byte[] bytes = Convert.FromBase64String(base64);

                // Convert to JSON string
                string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);

                return decodedJson;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return null;
            }
        }
    }
}
