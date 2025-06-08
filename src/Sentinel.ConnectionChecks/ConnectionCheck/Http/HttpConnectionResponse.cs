
public class HttpConnectionExtraResponse
{


    public string Name { get; set; } = string.Empty;
    public Dictionary<string, string?> Headers { get; internal set; }
    public string URL { get; internal set; }
}

//public class HttpConnectionExtraResponse : Dictionary<HttpConnectionObjectType, TestNetConnectionResponse>
//{

//    // public Dictionary<string, TestNetConnectionResponse> ExtraResponse { get; set; } = new Dictionary<string, TestNetConnectionResponse>();
//}
