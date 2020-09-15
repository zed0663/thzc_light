using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace Monster.UI
{
    public class UnlockCarViewRequest : UIViewRequest
    {

        public ObscuredInt UnlockCarLevel;
        public ObscuredInt RewardCarLevel;
        public ObscuredInt RewardCarAmount;

        public UnlockCarViewRequest(ObscuredInt unlockLevel, ObscuredInt rewardLevel, ObscuredInt rewardAmount) : base(typeof(UnlockCarUIState), true)
        {
            UnlockCarLevel = unlockLevel;
            RewardCarLevel = rewardLevel;
            RewardCarAmount = rewardAmount;
        }



    }
}
