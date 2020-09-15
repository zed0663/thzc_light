using Monster.Data;
using UnityEngine;
using UnityEditor;


namespace Monster.Common
{
    public class WheelRewardData : CommonRewardData
    {
        [Range(0, 100)] 
        public int Percent;
    }
}