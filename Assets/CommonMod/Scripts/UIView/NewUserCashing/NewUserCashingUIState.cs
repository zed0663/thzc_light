using System;
using System.Collections;
using System.Collections.Generic;
using Monster.Common;
using Random = UnityEngine.Random;

namespace Monster.UI
{
    public class NewUserCashingUIState : UIBaseState
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

          
        }
        public override void Leave(State newState)
        {
            base.Leave(newState);

        }

        public void OnClickReward()
        {
           
            ClosePopup();
        }

        
    }
}
