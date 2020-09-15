using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WXData
{
    public string access_token;
    public int expires_in;
    public string refresh_token;
    public string openId;
    public string scope;
    public string unionId;

    public void Reload(JSONNode json)
    {
        access_token = json["access_token"].ToString().Trim('"');
        refresh_token = json["refresh_token"].ToString().Trim('"');
        openId = json["openid"].ToString().Trim('"');
        scope = json["scope"].ToString().Trim('"');
        unionId = json["unionId"].ToString().Trim('"');
        expires_in = json["expires_in"].AsInt;
    }
}
