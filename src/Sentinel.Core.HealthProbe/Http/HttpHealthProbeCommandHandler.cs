using MediatR;
using Sentinel.Core.HealthProbe.Http.ContentHelpers;

namespace Sentinel.Core.HealthProbe.Http;
public class HttpHealthProbeCommandHandler : IRequestHandler<HttpHealthProbeCommand, HttpHealthProbeResponse>
{
    public Task<HttpHealthProbeResponse> Handle(HttpHealthProbeCommand request, CancellationToken cancellationToken)
    {
        HttpHealthProbeResponse response = new();

        HttpRequestBuilder httpRequestBuilder = new();
        httpRequestBuilder.AddRequestUri(request.Url)
          .AddMethod(request.Method);

        if (request.JsonBody != null)
            httpRequestBuilder.AddContent(new JsonContent(request.JsonBody));
        if (request.Headers != null && request.Headers.Count > 0)
            httpRequestBuilder.AddHeaders(request.Headers);
        if (request.ClientCertificateBase64 != null)
            httpRequestBuilder.AddClientCertificate(request.ClientCertificateBase64, request.ClientCertificatePassword);

        // httpRequestBuilder.AddBearerToken()
        //   .AddMethod(new HttpMethod(request.Method))
        //    .AddContent(new JsonContent(request.Body))
        //    .AddHeaders(request.Headers)
        //    .AddClientCertificate(request.ClientCertificateBase64, request.ClientCertificatePassword)
        //    .AddBearerToken(request.JWTokenMSIClientID);



        //     public bool IsItHeathCheck { get; set; }
        // public string Url { get; set; }
        // public HttpMethod Method { get; set; }
        //public string Body { get; set; }

        //public string[]? Headers { get; set; }

        //public string? ClientCertificateBase64 { get; set; }
        //public string? ClientCertificatePassword { get; set; }
        //public string? JWTokenMSIClientID { get; set; }

        return Task.FromResult(response);
    }
}

