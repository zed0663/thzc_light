using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using com.ootii.Messages;
using Monster.Common;
using Random = UnityEngine.Random;

namespace Monster.UI
{
    public class CashingUIState : UIBaseState
    {
        private ObscuredInt cashingIndex=-1;
        public override void Init()
        {
            base.Init();

            MessageDispatcher.AddListener("UpdateCarAmount", UpdateViewAmount);
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

        void UpdateViewAmount(IMessage rMessage)
        {
            ((CashingUIView)View).UpdateViewAmount((int)Data.GameData.RedPackCash, Data.GameData.RedPackCash/100f);
        }

        public void OnClickReward()
        {
           
            ClosePopup();
        }

        public void SigninClick()
        {
            if (!Data.GameData.CashSigninStatus)
            {
                //签到
            }
        }

        public void CashButtonClick(ObscuredInt index)
        {
            cashingIndex = index;
          
        }

        public void CashingConfirmClick()
        {
            //点击立即提现按钮
            if (cashingIndex!=-1)
            {
                
            }

        }


    }
}
