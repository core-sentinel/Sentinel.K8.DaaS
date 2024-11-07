namespace Sentinel.ConnectionChecks.Models
{
    public interface IBasicCheckAccessRequest
    {

        string Url { get; set; }
        int Port { get; set; }

        string SelectedAuthenticationType { get; set; }

        bool UseMSI { get; set; }
        ServicePrincipal? ServicePrincipal { get; set; }
    }
}
