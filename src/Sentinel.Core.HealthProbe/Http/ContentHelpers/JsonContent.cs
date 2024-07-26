using Newtonsoft.Json;
using System.Text;

namespace Sentinel.Core.HealthProbe.Http.ContentHelpers;

public class JsonContent : StringContent
{
    public JsonContent(object value) : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
    {
    }

    public JsonContent(object value, string mediaType) : base(JsonConvert.SerializeObject(value), Encoding.UTF8, mediaType)
    {
    }
}
