using MediatR;
using Sentinel.ConnectionChecks.Models;
using System.Text.Json.Serialization;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Http;

[ConnectionCheck(Name = "Http", Order = 2)]
public class HttpConnectionCheckRequest : IRequest<TestNetConnectionResponse<HttpConnectionExtraResponse>>, IBasicCheckAccessRequest
{
    public string? Url { get; set; } = "https://";
    public string? HttpMethod { get; set; } = "Get";
    public int Port { get; set; } = 443;
    public bool UseMSI { get; set; }
    public ServicePrincipal? ServicePrincipal { get; set; }

    public List<HttpHeader> Headers { get; set; } = new List<HttpHeader>();

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Type AdditionalRequestRazorContentType { get => typeof(HttpConnectionCheckUI); }
    public string SelectedAuthenticationType { get; set; } = "None";
    public HttpConnectionCheckRequest()
    {
        ServicePrincipal = new ServicePrincipal();
    }
    public string JsonDeserializationType => this.GetType().AssemblyQualifiedName;
}

public class HttpHeader
{

    public HttpHeader()
    {

    }
    public HttpHeader(string key, string value, bool enabled = true)
    {
        Key = key;
        Value = value;
        Enabled = enabled;
    }
    public bool Enabled { get; set; } = true;

    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
}
