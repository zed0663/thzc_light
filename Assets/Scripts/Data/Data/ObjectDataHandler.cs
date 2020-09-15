using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ObjectDataHandler : DataHandler<ObjectDataHandler>
{
    List<ObjectConfigData> _objectDatas;

    public void Reload()
    {
        _objectDatas = new List<ObjectConfigData>();
        var dataInfo = Resources.Load<TextAsset>("Configs/Json/object").text;
        var jsons = JSONNode.Parse(dataInfo) as JSONArray;
        for (int i = 0; i < jsons.Count; i++)
        {
            var data = new ObjectConfigData(jsons[i]);
            _objectDatas.Add(data);
        }
    }

    public ObjectConfigData GetObjectConfigData(int objectId)
    {
        return _objectDatas.Find(x => x.id == objectId);
    }
}
