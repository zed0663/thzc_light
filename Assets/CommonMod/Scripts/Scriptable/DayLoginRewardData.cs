using UnityEngine;

namespace Monster.Common
{


    [CreateAssetMenu(fileName = "DayLoginRewardData", menuName = "CommonMod/DayLoginRewardData")]

    public class DayLoginRewardData : ScriptableObject
    {
        [SerializeField]
        public CommonRewardData[] Rewards;

        [SerializeField]
        public OnlineRewardData[] OnlineRewards;
    }
    [System.Serializable]
    public class OnlineRewardData : CommonRewardData
    {

        public int Time;
    }
}