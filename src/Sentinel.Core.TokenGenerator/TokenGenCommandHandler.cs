using MediatR;

namespace Sentinel.Core.TokenGenerator
{
    internal class TokenGenCommandHandler : IRequestHandler<TokenGenCommand, TokenGenResponse>
    {
        public Task<TokenGenResponse> Handle(TokenGenCommand request, CancellationToken cancellationToken)
        {
            TokenGenResponse tokenGenResponse = new TokenGenResponse();
            if (request.UseMSI)
            {
                // Use Managed Identity to generate token
                if (string.IsNullOrEmpty(request.Audience))
                {
                    tokenGenResponse.Error = "Audience is required when using Managed Identity.";
                    return Task.FromResult(tokenGenResponse);
                }
                try
                {
                    tokenGenResponse.Token = AzureTokenGenerator.GenerateToken(request.Audience);
                    tokenGenResponse.DecodedToken = AzureTokenGenerator.DecodeToken(tokenGenResponse.Token);
                }
                catch (Exception ex)
                {
                    tokenGenResponse.Error = $"Failed to generate token using Managed Identity: {ex.Message}";
                    return Task.FromResult(tokenGenResponse);
                }
            }
            else
            {
                // Use Service Principal to generate token
                if (string.IsNullOrEmpty(request.ServicePrincipal?.ClientId) ||
                    string.IsNullOrEmpty(request.ServicePrincipal?.TenantId) ||
                    string.IsNullOrEmpty(request.ServicePrincipal?.ClientSecret))
                {
                    tokenGenResponse.Error = "Service Principal credentials are required when not using Managed Identity.";
                    return Task.FromResult(tokenGenResponse);
                }
                try
                {
                    tokenGenResponse.Token = AzureTokenGenerator.GenerateToken(
                        request.ServicePrincipal.ClientId,
                        request.ServicePrincipal.TenantId,
                        request.ServicePrincipal.ClientSecret);
                }
                catch (Exception ex)
                {
                    tokenGenResponse.Error = $"Failed to generate token using Service Principal: {ex.Message}";
                    return Task.FromResult(tokenGenResponse);
                }
            }

            return Task.FromResult(tokenGenResponse);
        }
    }
}
