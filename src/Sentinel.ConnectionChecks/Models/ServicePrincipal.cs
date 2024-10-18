namespace Sentinel.ConnectionChecks.Models
{
    public class ServicePrincipal
    {
        public string? TenantId { get; set; } = default!;

        public string? PrincipalId { get; set; } = default!;
        public string? ClientId { get; set; } = default!;
        public string? ClientSecret { get; set; } = default!;

        //   public string? UserName { get; set; } = default!;
    }
}