using SimpleJSON;

public class DailyRewardConfigData
{
    public int id;
    public int objectId;

    public DailyRewardConfigData(JSONNode json)
    {
        id = json["id"].AsInt;
        objectId = json["objectId"].AsInt;
    }

    public ObjectConfigData GetObjectConfigData()
    {
        return ObjectDataHandler.Instance.GetObjectConfigData(objectId);
    }
}
