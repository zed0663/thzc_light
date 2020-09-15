using SimpleJSON;

public class UserData
{
    public string userId;

    public UserData(JSONNode data)
    {
        userId = data["userId"].ToString().Trim('"');
    }

    public WXData GetWXData()
    {
        return WXDataHandler.Instance.GetWXData();
    }
}
