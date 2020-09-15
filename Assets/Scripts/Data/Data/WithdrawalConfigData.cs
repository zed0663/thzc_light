using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WithdrawalConfigData
{
    public int id;
    public int objectId;
    public int signedCount;
    public int userLevel;
    public int adsCount;
    public int invitationCount;
    public int withdrawalCountLimit;

    public WithdrawalConfigData(JSONNode data)
    {
        id = data["id"].AsInt;
        objectId = data["objectId"].AsInt;
        signedCount = data["signedCount"].AsInt;
        userLevel = data["userLevel"].AsInt;
        adsCount = data["adsCount"].AsInt;
        invitationCount = data["invitationCount"].AsInt;
        withdrawalCountLimit = data["withdrawalCountLimit"].AsInt;
    }

    public ObjectConfigData GetObjectConfigData()
    {
        return ObjectDataHandler.Instance.GetObjectConfigData(objectId);
    }
}
