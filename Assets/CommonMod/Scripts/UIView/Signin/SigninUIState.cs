using System.Collections;
using System.Collections.Generic;
using Monster.Common;
using Monster.Core;
using UnityEngine;

namespace Monster.UI
{
    public class SigninUIState : UIBaseState
    {

        public DayLoginRewardData _DayRewardData;
        private int CurrectDay;
        private bool isClaimDay;

        private int isClaimOnlineCount;//已领取的
        private int isCompleteOnlineCount=0;//达到的
       

        private long _totalRunTime;

        public long TotalRunTime
        {
            get
            {
                return _totalRunTime + (long)Time.realtimeSinceStartup;
            }
        }
        public override void Init()
        {
            base.Init();
            _totalRunTime = Data.GameData.TotalRunTime;
            CurrectDay = Data.GameData.LoginDays;
            isClaimDay = Data.GameData.ClaimDayLoginReward;
            ((SigninUIView)this.View).ShowView(isClaimDay, CurrectDay, _DayRewardData.Rewards, _DayRewardData.OnlineRewards);
        }

        
        public override void Enter(State oldState)
        {
            base.Enter(oldState);
           
        }

       
        public override void OpenPopup(UIViewRequest request)
        {
            base.OpenPopup(request);


            ((SigninUIView)this.View).UpdateDayLoginElements(isClaimDay, CurrectDay);

            isClaimOnlineCount = Data.GameData.ClaimOnlineReward;
           

        }

        void Update()
        {
            //暂定
            for (int i = 0; i < _DayRewardData.OnlineRewards.Length; i++)
            {
                if (_DayRewardData.OnlineRewards[i].Time * 60 > TotalRunTime)
                {
                    isCompleteOnlineCount = i + 1;
                }
            }

            float lastProgress = 0;
            if (isCompleteOnlineCount > 0)
            {
                lastProgress = TotalRunTime - _DayRewardData.OnlineRewards[isCompleteOnlineCount].Time * 60;
            }
            else
            {
                lastProgress = TotalRunTime;
            }

            float onlineProgress = lastProgress / _DayRewardData.OnlineRewards[isCompleteOnlineCount + 1].Time;

            ((SigninUIView)this.View).UpdateOnlineElements(isClaimOnlineCount, isCompleteOnlineCount, onlineProgress);
        }

        void OnDestroy()
        {
           Data.GameData.TotalRunTime+=TotalRunTime;
        }
        public override void Leave(State newState)
        {
            base.Leave(newState);

        }

        public void OnClickDayLogin(int index)
        {

            CurrectDay++;
            Data.GameData.LoginDays = CurrectDay;
            isClaimDay = Data.GameData.ClaimDayLoginReward = false;
        }

        public void OnClickSigninReward()
        {

        }
    }
}
