using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class RotaryTableConfigData 
{
    public int id;
    public int objectId;
    public int probability;

    public RotaryTableConfigData(JSONNode data)
    {
        id = data["id"].AsInt;
        objectId = data["objectId"].AsInt;
        probability = data["probability"].AsInt;
    }

    public ObjectConfigData GetObjectConfigData()
    {
        return ObjectDataHandler.Instance.GetObjectConfigData(objectId);
    }
}
