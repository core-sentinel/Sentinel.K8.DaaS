namespace Sentinel.ConnectionChecks.Models;
public class TestNetConnectionResponse
{
    public bool IsConnected { get; set; }
    public string? Message { get; set; }

    public string CategoryTypeName { get; set; }
    public int CategoryTypeID { get; set; }

    public long ElapsedMilliseconds { get; set; }

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
