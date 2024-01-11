using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sentinel.NetworkUtils.Models
{
    public class CheckAccessResponse
    {
        public string ipaddress { get; set; } = default!;
        public TestNetConnectionResponse netResult { get; set; } = default!;
        public TestNetConnectionResponse? additionalResult { get; set; } = default!;
    }
}