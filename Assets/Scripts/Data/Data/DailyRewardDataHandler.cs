using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class DailyRewardDataHandler : DataHandler<DailyRewardDataHandler>
{
    List<DailyRewardConfigData> _dailyRewardDatas;

    public void Reload()
    {
        _dailyRewardDatas = new List<DailyRewardConfigData>();
        var dataInfo = Resources.Load<TextAsset>("Configs/Json/dailyReward").text;
        var jsons = JSONNode.Parse(dataInfo) as JSONArray;
        for (int i = 0; i < jsons.Count; i++)
        {
            DailyRewardConfigData data = new DailyRewardConfigData(jsons[i]);
            _dailyRewardDatas.Add(data);
        }
    }

    public List<DailyRewardConfigData> GetDailyRewardDatas()
    {
        return _dailyRewardDatas;
    }

    //TODO
    public DailyRewardConfigData GetDailyRewardData()
    {
        //return _dailyRewardDatas.Find(s => s.id == UserDataHandler.instance.GetUserData().dailyCheckIn);
        return null;
    }

    //TODO签到服务器接口 统一用红包

}
