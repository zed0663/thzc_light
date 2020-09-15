using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class WheelLuckyUIState : UIBaseState
    {


        public override void Init()
        {
            base.Init();

          //  ((PhoneSigninUIView)this.View).ShowView(isClaimDay, CurrectDay, _DayRewardData.Rewards, _DayRewardData.OnlineRewards);
        }


        public override void Enter(State oldState)
        {
            _skipNextAnimation = true;
            base.Enter(oldState);

        }


        public override void OpenPopup(UIViewRequest request)
        {
            base.OpenPopup(request);
            





        }
        public override void Leave(State newState)
        {
            base.Leave(newState);

        }

        public void ClickSpin()
        {
            ((WheelLuckyUIView)this.View).Spin(Random.Range(0, 8));
        }

        
    }
}
