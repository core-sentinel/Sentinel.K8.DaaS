using Azure.Core;
using Azure.Identity;
using Sentinel.ConnectionChecks.Models;
using Sentinel.Core.HealthProbe.Http;
using System.Diagnostics;
using System.Text;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Http
{
    internal class TestHttpRequest
    {
        public static async Task<TestNetConnectionResponse<HttpConnectionExtraResponse>> TestConnection(HttpConnectionCheckRequest request, Microsoft.Extensions.Logging.ILogger logger)
        {
            var sw = Stopwatch.StartNew();

            StringBuilder sb = new StringBuilder();
            var result = new TestNetConnectionResponse<HttpConnectionExtraResponse>("HttpConnection", true, sb.ToString(), sw.ElapsedMilliseconds);

            result.ExtraResultRazorContentType = typeof(HttpConnectionCheckUIResult);
            try
            {
                if (!string.IsNullOrWhiteSpace(request.Url) && !request.Url.ToLower().Contains("://"))
                {
                    if (request.Port == 443)
                    {
                        request.Url = "https://" + request.Url;
                    }
                    else
                    {
                        request.Url = "http://" + request.Url;
                    }
                }
                HttpMethod method = HttpMethod.Get;

                if (request.HttpMethod != null)
                {
                    method = HttpMethod.Parse(request.HttpMethod);
                }

                HttpRequestBuilder httpbuilder = new HttpRequestBuilder()
                    .AddRequestUri(request.Url)
                    .AddLogger(logger)
                    .AddMethod(method);

                foreach (var item in request.Headers.Where(p => p.Enabled == true && !string.IsNullOrWhiteSpace(p.Key) && !string.IsNullOrWhiteSpace(p.Value)))
                {
                    httpbuilder.AddHeader(item.Key, item.Value);
                }
                if (request.UseMSI && !string.IsNullOrWhiteSpace(request.ServicePrincipal.ClientId))
                {
                    var tokenCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = request.ServicePrincipal.ClientId });
                    var accessToken = await tokenCredential.GetTokenAsync(new TokenRequestContext(scopes: new string[] { request.ServicePrincipal.ClientId + "/.default" }));
                    httpbuilder.AddBearerToken(accessToken.Token);

                }

                // HttpRequestMessage req = new HttpRequestMessage(method, request.Url);
                // HttpClient client = new HttpClient();

                // var response = await client.SendAsync(req);
                var response = await httpbuilder.SendAsync();
                result.ExtraResult.Headers = httpbuilder.request.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault());
                result.ExtraResult.URL = request.Url;
                var message = response.StatusCode.ToString() + " " + await response.Content.ReadAsStringAsync();
                sb.AppendLine(message);

                // return new TestNetConnectionResponse("Http", true, message, sw.ElapsedMilliseconds);
                result.IsConnected = true;
                result.Message = message;
                return result; //new TestNetConnectionResponse<HttpConnectionExtraResponse>("Http", true, message, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                result.IsConnected = false;
                sb.AppendLine(ex.Message);
                result.Message = ex.Message;
                return result;  //new TestNetConnectionResponse<HttpConnectionExtraResponse>("Http", false, ex.Message, sw.ElapsedMilliseconds);
            }
        }
    }
}
