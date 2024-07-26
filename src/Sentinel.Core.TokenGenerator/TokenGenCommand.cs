using MediatR;

namespace Sentinel.Core.TokenGenerator;
public class TokenGenCommand : IRequest<TokenGenResponse>
{
    public bool UseMSI { get; set; } = true;
    public string Audience { get; set; } = default!;

    public ServicePrincipal? ServicePrincipal { get; set; } = default!;

    public TokenGenCommand()
    {
        ServicePrincipal = new ServicePrincipal();
    }
}

public class ServicePrincipal
{
    public string? ClientId { get; set; } = default!;
    public string? ClientSecret { get; set; } = default!;
    public string? TenantId { get; set; } = default!;
}


public class TokenGenResponse
{
    public string? Token { get; set; } = default!;
    public string? Error { get; set; } = default!;
}
