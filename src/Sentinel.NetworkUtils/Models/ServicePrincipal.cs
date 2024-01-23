using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sentinel.NetworkUtils.Models
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