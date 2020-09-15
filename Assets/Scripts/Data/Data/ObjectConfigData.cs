using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ObjectConfigData
{
    public int id;
    public int count;
    public string type;
    public string desc;

    public ObjectConfigData(JSONNode data)
    {
        id = data["id"].AsInt;
        count = data["count"].AsInt;
        type = data["type"].ToString().Trim('"');
        type = data["desc"].ToString().Trim('"');
    }
}
