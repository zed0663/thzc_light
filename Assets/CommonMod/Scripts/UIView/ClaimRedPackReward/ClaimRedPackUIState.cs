using System;
using System.Collections;
using System.Collections.Generic;
using Monster.Common;
using Random = UnityEngine.Random;

namespace Monster.UI
{
    public class ClaimRedPackUIState : UIBaseState
    {
        private double Amount;

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

            ClaimRedPackViewRequest _Request = request as ClaimRedPackViewRequest;
            Amount = _Request.RedPackAmount;

            double totalAmount = 800.639f;
            totalAmount = Math.Round(totalAmount, 2);
            ((ClaimRedPackUIView)this.View).ShowView(Amount, totalAmount);
        }
        public override void Leave(State newState)
        {
            base.Leave(newState);

        }

        public void OnClickReward()
        {
            GameActionManager.Instance.CliamReward(ClaimRewardType.RedPack, Amount, 0);
            ClosePopup();
        }

        
    }
}
