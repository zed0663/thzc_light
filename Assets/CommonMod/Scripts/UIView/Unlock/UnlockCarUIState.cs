using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Monster.Common;
using Random = UnityEngine.Random;

namespace Monster.UI
{
    public class UnlockCarUIState : UIBaseState
    {
        private ObscuredInt RewardAmount;
        private ObscuredInt RewardLevel;
        public override void Init()
        {
            base.Init();

            
        }


        public override void Enter(State oldState)
        {
            base.Enter(oldState);

        }


        public override void OpenPopup(UIViewRequest request)
        {
            base.OpenPopup(request);
            UnlockCarViewRequest _requestData = request as UnlockCarViewRequest;
            RewardLevel = _requestData.RewardCarLevel;
            RewardAmount = _requestData.RewardCarAmount;
            ((UnlockCarUIView)this.View).ShowView(GameDataManager.Instance._CarNodeGroupData.GetProperties(_requestData.UnlockCarLevel), GameDataManager.Instance._CarNodeGroupData.GetIcon(RewardLevel), RewardAmount);

        }
        public override void Leave(State newState)
        {
            base.Leave(newState);

        }

        public void OnClickReward()
        {
           GameActionManager.Instance.CliamReward(ClaimRewardType.CarAmount, RewardAmount,0);
            ClosePopup();
        }

        
    }
}
