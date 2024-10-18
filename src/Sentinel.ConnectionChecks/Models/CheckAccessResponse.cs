namespace Sentinel.ConnectionChecks.Models
{
    public class CheckAccessResponse
    {
        public string ipaddress { get; set; } = default!;
        public TestNetConnectionResponse netResult { get; set; } = default!;
        public TestNetConnectionResponse? additionalResult { get; set; } = default!;
    }
}