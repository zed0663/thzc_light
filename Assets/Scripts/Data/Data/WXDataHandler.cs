using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class WXDataHandler : DataHandler<WXDataHandler>
{
    public bool IsLogin;
    WXData _wxData;
    public string wxName;
    public string wxImgUrl;

    public void Reload()
    {
        if (PlayerPrefs.GetString("WXUserInfo") != string.Empty)
        {
            _wxData = new WXData();
            var json = JSONNode.Parse(PlayerPrefs.GetString("WXUserInfo"));
            _wxData.Reload(json);

            wxImgUrl = PlayerPrefs.GetString("wxImgUrl");
            wxName = PlayerPrefs.GetString("wxName");
            IsLogin = true;
        }
        else
        {
            IsLogin = false;
        }
    }

    public WXData GetWXData()
    {
        return _wxData;
    }

    public void SaveWXData(WXData data)
    {
        _wxData = data;
        if (_wxData.openId != string.Empty)
        {
            IsLogin = true;
        }
    }

    //请求微信数据
    public void WXRequest(MonoBehaviour mono, Dictionary<string, object> form, string address)
    {
        HttpHelper.Request(mono, address, HttpHelper.MethodType.POST, form,
        delegate (object value)
        {
            var wxData = new WXData();
            var json = JSONNode.Parse(value.ToString());
            wxData.Reload(json);
            Debug.Log("postLoginRecord :" + value.ToString());
            form = new Dictionary<string, object>();
            form.Add("access_token", wxData.access_token);
            form.Add("openid", wxData.openId);
            WXReuqestUserInfo(mono, form, CommonConfig.wxAddress);
            SaveWXData(wxData);
            PlayerPrefs.SetString("WXUserInfo", value.ToString());
        }, delegate (object value)
        {
            Debug.Log("postLoginRecord error:" + value.ToString());
        }, HttpHelper.DownloadHanlderType.kHttpTEXT);
    }

    //请求微信用户数据
    public void WXReuqestUserInfo(MonoBehaviour mono, Dictionary<string, object> form, string address)
    {
        HttpHelper.Request(mono, address, HttpHelper.MethodType.POST, form,
       delegate (object value)
       {
           var json = JSONNode.Parse(value.ToString());
           wxImgUrl = json["headimgurl"].ToString().Trim('"');
           wxName = json["nickname"].ToString().Trim('"');
           PlayerPrefs.SetString("wxImgUrl", wxImgUrl);
           PlayerPrefs.SetString("wxName", wxName);
           Debug.Log("wxImgUrl" + wxImgUrl);
           Debug.Log("wxName" + wxName);
           Notification.NotificationCenter.Default.Post(NotificationKey.WXInfoRelod, null);
       }, delegate (object value)
       {
           Debug.Log("postLoginRecord error:" + value.ToString());
       }, HttpHelper.DownloadHanlderType.kHttpTEXT);
    }
}
