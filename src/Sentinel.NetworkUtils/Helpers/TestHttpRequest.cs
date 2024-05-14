using Sentinel.NetworkUtils.Models;
using System.Diagnostics;

namespace Sentinel.NetworkUtils.Helpers;

public static class TestHttpRequest
{
    public static async Task<TestNetConnectionResponse> TestConnection(string Url, HttpMethod method, bool UseMSI, ServicePrincipal principal = null)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            if (method == null) { method = HttpMethod.Get; }
            HttpRequestMessage req = new HttpRequestMessage(method, Url);
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
