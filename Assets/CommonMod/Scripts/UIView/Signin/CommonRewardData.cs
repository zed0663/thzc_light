using Monster.Data;
using UnityEngine;


namespace Monster.Common
{
    [System.Serializable]
    public class CommonRewardData
    {
        [SerializeField]
        public int Index;
        [SerializeField]
        public Sprite Icon;
        [SerializeField]
        public RewardData reward;

    }
}

