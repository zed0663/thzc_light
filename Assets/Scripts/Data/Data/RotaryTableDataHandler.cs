using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class RotaryTableDataHandler : DataHandler<WithdrawalDataHandler>
{

    static RotaryTableDataHandler _instance;
    public static RotaryTableDataHandler instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RotaryTableDataHandler();
            }
            return _instance;
        }
    }

    List<RotaryTableConfigData> _rotaryDatas;
    public int costCoinsCount;


    public void Reload()
    {
        _rotaryDatas = new List<RotaryTableConfigData>();
        var dataInfo = JSONNode.Parse(Resources.Load<TextAsset>("Configs/Json/rotaryTable").text);
        costCoinsCount = dataInfo["costCoins"].AsInt;
        var reward = dataInfo["reward"].AsArray;
        for (int i = 0; i < reward.Count; i++)
        {
            var data = new RotaryTableConfigData(reward[i]);
            _rotaryDatas.Add(data);
        }
    }

    List<int> datas = new List<int>();
    List<int> weights = new List<int>();

    public List<RotaryTableConfigData> GetRotaryTableDatas()
    {
        return _rotaryDatas;
    }
    //获取随机奖品
    public RotaryTableConfigData GetReward()
    {
        datas.Clear();
        weights.Clear();
        foreach (var v in _rotaryDatas)
        {
            datas.Add(v.id);
            weights.Add(v.probability);
        }
        //随机数生成器
        System.Random rand = new System.Random();
        int[] rands = ControllerRandomExtract(rand);
        return _rotaryDatas.Find(x => x.id == rands[0]);
    }

    public int[] ControllerRandomExtract(System.Random rand)
    {
        List<int> result = new List<int>();
        if (rand != null)
        {
            //临时变量
            Dictionary<int, int> dict = new Dictionary<int, int>();
            //为每个项算一个随机数并乘以相应的权值
            for (int i = datas.Count - 1; i >= 0; i--)
            {
                dict.Add(datas[i], rand.Next(100) * weights[i]);
            }

            //排序
            List<KeyValuePair<int, int>> listDict = SortByValue(dict);

            //拷贝抽取权值最大的前Count项
            foreach (KeyValuePair<int, int> kvp in listDict.GetRange(1, 1))
            {
                result.Add(kvp.Key);
            }
        }
        return result.ToArray();
    }

    List<KeyValuePair<int, int>> SortByValue(Dictionary<int, int> dict)
    {
        List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();

        if (dict != null)
        {
            list.AddRange(dict);

            list.Sort(
              delegate (KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2)
              {
                  return kvp2.Value - kvp1.Value;
              });
        }
        return list;
    }

    //TODO 根据服务器时间刷新次数
}
