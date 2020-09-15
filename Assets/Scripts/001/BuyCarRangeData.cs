using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class BuyCarRangeData : ScriptableObject
{
    [Header("Data")] [SerializeField] private BuyCarRangeProperties[] RangeDatas;

    public ObscuredVector2Int GetBuyRange(ObscuredInt _currect)
    {
        for (int i = 0; i < RangeDatas.Length; i++)
        {
            if (_currect == RangeDatas[i].AllowLevel)
            {
                return RangeDatas[i].OffsetLevel;
            }
        }
        return new ObscuredVector2Int(_currect-1, 1);
    }

}
[System.Serializable]
public struct BuyCarRangeProperties
{
    //
    public ObscuredInt AllowLevel;

    //第一个为可购买的最大等级的偏移 ，第二个为可以购买几个等级的车，按可购买最大等级之后排序,
    //ObscuredVector2Int 范围等级的索引，[x,y) 即包含X，不包含Y。
    //如果AllowLevel=10, OffsetLevel.x=2，OffsetLevel.y=3 ,可以购买的最大等级为10-2=8，可以购买的等级为8 7 6 三个等级;
    public ObscuredVector2Int OffsetLevel;
}
