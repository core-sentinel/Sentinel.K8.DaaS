using MediatR;

namespace Sentinel.Core.HealthProbe.Http;

public class HttpHealthProbeCommand : IRequest<HttpHealthProbeResponse>
{
    public bool IsItHeathCheck { get; set; }
    public string Url { get; set; }
    public HttpMethod Method { get; set; }
    public string JsonBody { get; set; }
    public Dictionary<string, string>? Headers { get; set; }

    public string? ClientCertificateBase64 { get; set; }
    public string? ClientCertificatePassword { get; set; }
    public string? JWTokenMSIClientID { get; set; }

    public string ExpectedResponse { get; set; }
    public string ExpectedResponseTime { get; set; }
    public string ExpectedResponseCode { get; set; }
    public string ExpectedResponseContentType { get; set; }





}

