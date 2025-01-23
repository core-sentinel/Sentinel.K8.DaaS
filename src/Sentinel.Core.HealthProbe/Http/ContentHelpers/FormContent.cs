namespace Sentinel.Core.HealthProbe.Http.ContentHelpers
{
    public class FormContent : FormUrlEncodedContent
    {
        public FormContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection) : base(nameValueCollection)
        {
        }
    }
}
