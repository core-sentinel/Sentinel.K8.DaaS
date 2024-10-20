using Sentinel.ConnectionChecks.Models;
using System.Diagnostics;

namespace Sentinel.ConnectionChecks.ConnectionCheck.Http
{
    internal class TestHttpRequest
    {
        public static async Task<TestNetConnectionResponse> TestConnection(HttpConnectionCheckRequest request)
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
                HttpMethod method = null;

                if (request.HttpMethod == null) { method = HttpMethod.Get; }
                else
                {
                    method = HttpMethod.Parse(request.HttpMethod);
                }
                HttpRequestMessage req = new HttpRequestMessage(method, request.Url);
                HttpClient client = new HttpClient();

                var response = await client.SendAsync(req);
                string message = response.StatusCode.ToString() + " " + await response.Content.ReadAsStringAsync();

                return new TestNetConnectionResponse(CheckAccessRequestResourceType.Http, true, message, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                return new TestNetConnectionResponse(CheckAccessRequestResourceType.Http, false, ex.Message, sw.ElapsedMilliseconds);
            }
        }
    }
}
