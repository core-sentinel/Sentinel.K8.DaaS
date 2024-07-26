using Newtonsoft.Json;
using System.Text;

namespace Sentinel.Core.HealthProbe.Http.ContentHelpers;

public class PatchContent : StringContent
{
    public PatchContent(object value) : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json-patch+json")
    {
    }
}
