using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.Data
{
    public enum RewardType
    {
        //金币
        SoftCurrency,
        //钻石 现金 高级货币
        HardCurrency,

        Car,
        Item,
        RedPack,
        Spin
    }

    [Serializable]
    public struct RewardData
    {
        public RewardType type;
        public double value;
        public int value_unit;
        public RewardData(RewardType _type, double _value,int _unit)
        {
           type = _type;
            value = _value;
            value_unit = _unit;
        }

        public RewardData GetDoubled()
        {
            double num = value * 2;
            int unit = value_unit;
            Helper.FixUnit(ref num, ref unit);
            return new RewardData(this.type, num, unit);
        }

    }
}
