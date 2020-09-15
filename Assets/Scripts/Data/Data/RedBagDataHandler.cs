using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class RedBagDataHandler : DataHandler<RedBagDataHandler>
{
    RedBagData _redBagData;

    public void ReloadData(JSONNode data)
    {
        _redBagData = new RedBagData(data);
    }

    public RedBagData GetRedBagData()
    {
        return _redBagData;
    }

    //todo 更新红包更新接口(包括红包币，邀请人数，广告次数，签到天数)

    //更新红包币
    public void RequestUpdateRedBagData(MonoBehaviour mono, ObjectConfigData objectConfigData)
    {
        //服务器参数 key 红包币 objectId 和 objectId对应的数值 具体跟服务器对
        Dictionary<string, object> form = new Dictionary<string, object>();
        form.Add("objectId", objectConfigData.id);
        form.Add("count", objectConfigData.count);

        HttpHelper.Request(mono, CommonConfig.ServerAddress, HttpHelper.MethodType.POST, form,
      delegate (object value)
      {
          //服务器返回数据
          var json = JSONNode.Parse(value.ToString());
          //更新RedBagData

      }, delegate (object value)
      {
          Debug.Log("error:" + value.ToString());
      }, HttpHelper.DownloadHanlderType.kHttpTEXT);
    }
}
