using Sentinel.ConnectionChecks.Models;
using Sentinel.Core.HealthProbe.Http;
using System.Diagnostics;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Http
{
    internal class TestHttpRequest
    {
        public static async Task<TestNetConnectionResponse> TestConnection(HttpConnectionCheckRequest request, Microsoft.Extensions.Logging.ILogger logger)
        {
            var sw = Stopwatch.StartNew();
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



                // HttpRequestMessage req = new HttpRequestMessage(method, request.Url);
                // HttpClient client = new HttpClient();

                // var response = await client.SendAsync(req);
                var response = await httpbuilder.SendAsync();


                string message = response.StatusCode.ToString() + " " + await response.Content.ReadAsStringAsync();

                return new TestNetConnectionResponse("Http", true, message, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                return new TestNetConnectionResponse("Http", false, ex.Message, sw.ElapsedMilliseconds);
            }
        }
    }
}
