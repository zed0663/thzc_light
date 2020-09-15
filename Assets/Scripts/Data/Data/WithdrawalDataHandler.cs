using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public class WithdrawalDataHandler : DataHandler<WithdrawalDataHandler>
{
    List<WithdrawalConfigData> _configDatas;
    List<WithdrawalData> _withdrawalDatas;

    public void ReloadConfigs()
    {
        _configDatas = new List<WithdrawalConfigData>();
        var dataInfo = Resources.Load<TextAsset>("Configs/Json/withdrawal").text;
        var jsons = JSONNode.Parse(dataInfo) as JSONArray;
        for (int i = 0; i < jsons.Count; i++)
        {
            var data = new WithdrawalConfigData(jsons[i]);
            _configDatas.Add(data);
        }
    }

    public void ReloadData()
    {
        _configDatas = new List<WithdrawalConfigData>();
        var dataInfo = Resources.Load<TextAsset>("Configs/Json/withdrawal").text;
        var jsons = JSONNode.Parse(dataInfo) as JSONArray;
        for (int i = 0; i < jsons.Count; i++)
        {
            var data = new WithdrawalConfigData(jsons[i]);
            _configDatas.Add(data);
        }
    }

    public WithdrawalConfigData GetConfigData(int id)
    {
        return _configDatas.Find(x => x.id == id);
    }

    public WithdrawalData GetWithdrawalData(int id)
    {
        return _withdrawalDatas.Find(x => x.id == id);
    }

    public List<WithdrawalData> GetWithdrawalData()
    {
        return _withdrawalDatas;
    }

    public List<WithdrawalConfigData> GetConfigDatas()
    {
        return _configDatas;
    }

    //TODO
    ///提现接口
}
