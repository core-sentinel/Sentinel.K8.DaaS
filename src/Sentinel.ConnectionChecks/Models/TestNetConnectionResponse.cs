namespace Sentinel.ConnectionChecks.Models;
public class TestNetConnectionResponse
{
    public bool IsConnected { get; set; }
    public string? Message { get; set; }

    public string CategoryTypeName { get; set; }
    public int CategoryTypeID { get; set; }

    public string IPAddress { get; set; } = "";

    public long ElapsedMilliseconds { get; set; }


    public Type? ExtraResultRazorContentType { get; set; } = null;

    public Dictionary<string, object> ExtraResultDictionary { get; set; } = new Dictionary<string, object>();

    public TestNetConnectionResponse()
    {

    }

    public TestNetConnectionResponse(CheckAccessRequestResourceType categoryType, bool isConnected, long elapsedMilliseconds)
    {
        IsConnected = isConnected;
        ElapsedMilliseconds = elapsedMilliseconds;
        CategoryTypeID = Convert.ToInt32(categoryType);
        CategoryTypeName = categoryType.ToString();
    }

    public TestNetConnectionResponse(CheckAccessRequestResourceType categoryType, bool isConnected, string message, long elapsedMilliseconds = 0)
    {
        IsConnected = isConnected;
        ElapsedMilliseconds = elapsedMilliseconds;
        Message = message;
        CategoryTypeID = Convert.ToInt32(categoryType);
        CategoryTypeName = categoryType.ToString();


    }
}


public class TestNetConnectionResponse<T> : TestNetConnectionResponse where T : new()
{
    public T? ExtraResult { get; set; } = new T();



    public TestNetConnectionResponse()
    {
        AddExtraResultToDictionary();
    }

    public TestNetConnectionResponse(CheckAccessRequestResourceType categoryType, bool isConnected, long elapsedMilliseconds)
        : base(categoryType, isConnected, elapsedMilliseconds)
    {
        AddExtraResultToDictionary();


    }

    public TestNetConnectionResponse(CheckAccessRequestResourceType categoryType, bool isConnected, string message, long elapsedMilliseconds = 0, string iPAddress = "")
        : base(categoryType, isConnected, message, elapsedMilliseconds)
    {
        AddExtraResultToDictionary();
        IPAddress = iPAddress;
    }

    public TestNetConnectionResponse(CheckAccessRequestResourceType categoryType, bool isConnected, long elapsedMilliseconds, T extraResult)
        : base(categoryType, isConnected, elapsedMilliseconds)
    {
        ExtraResult = extraResult;

        AddExtraResultToDictionary();
    }

    public TestNetConnectionResponse(CheckAccessRequestResourceType categoryType, bool isConnected, string message, long elapsedMilliseconds, T extraResult)
        : base(categoryType, isConnected, message, elapsedMilliseconds)
    {

        AddExtraResultToDictionary();


    }


    private void AddExtraResultToDictionary()
    {
        ExtraResultDictionary.Add(nameof(ExtraResult), ExtraResult);
        ExtraResultDictionary.Add("IsConnected", IsConnected);
        ExtraResultDictionary.Add("ElapsedMilliseconds", ElapsedMilliseconds);
        ExtraResultDictionary.Add("Message", Message);
    }
}
