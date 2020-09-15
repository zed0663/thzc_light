using UnityEngine;
using System.Collections;
using Monster.Common;
using UnityEngine.UI;

namespace Monster.UI
{

    public class SigninUIView : UIBaseView
    {
        
        public SigninItem[] _DayItems;
        public OnlineSigninItem[] _OnlineSigninItems;

        public Image[] _OnlineProgress;

        public override void Init()
        {
            base.Init();
          
        }
        public override void Open()
        {
            base.Open();

        }

        public override void Close()
        {
          
            base.Close();

        }

        public void ShowView(bool isClaim,int enableDay, CommonRewardData[] dayDatas, OnlineRewardData[] onlineDatas)
        {
            for (int i = 0; i < _DayItems.Length; i++)
            {
                double value = dayDatas[i].reward.value;
                int unit = dayDatas[i].reward.value_unit;
                Helper.FixUnit(ref value, ref unit);
                int status = DayLoginStatus(isClaim, enableDay,i);


                _DayItems[i].SetItem(i,(i + 1).ToString(), dayDatas[i].Icon, ApplicationManager.Instance.AppendFromUnit(value, unit), status);
            }

            for (int i = 0; i < _OnlineSigninItems.Length; i++)
            {
                double value = onlineDatas[i].reward.value;
                int unit = onlineDatas[i].reward.value_unit;

                _OnlineSigninItems[i].SetItem(ApplicationManager.Instance.AppendFromUnit(value, unit), onlineDatas[i].Time);
            }
        }

        public void UpdateDayLoginElements(bool isClaim, int enableDay)
        {
            for (int i = 0; i < _DayItems.Length; i++)
            {
                int status = DayLoginStatus(isClaim, enableDay, i);
                _DayItems[i].SetStatus(status);
            }

        }

        /// <summary>
        ///   
        /// </summary>
        /// <param name="inClaim">已领取过的个数</param>
        /// <param name="_completeCount">达到计数</param>
        /// <param name="_onlineProgress">当前进度</param>
        public void UpdateOnlineElements(int inClaimCount, int _completeCount, float _onlineProgress)
        {
            for (int i = 0; i < _OnlineSigninItems.Length; i++)
            {
                if (i< _OnlineProgress.Length)
                {
                    if (_completeCount > i + 1)
                    {
                        _OnlineProgress[i].fillAmount = 1;
                    }
                    else if (_completeCount == i + 1)
                    {
                        _OnlineProgress[i].fillAmount = _onlineProgress;
                    }
                    else
                    {
                        _OnlineProgress[i].fillAmount = 0;
                    }
                }
              
                int status = -1;
                if (_completeCount< i + 1)
                {
                    status = -1;
                }
                else if (inClaimCount < _completeCount)
                {
                    if (_completeCount <= i + 1)
                    {
                        status = 0;
                    }
                }
                else
                {
                    status = 1;
                }

                _OnlineSigninItems[i].UpdateView(status);
            }
        }

        int DayLoginStatus(bool isClaim, int enableDay,int index)
        {
            int status = 0;
            if (isClaim)
            {
                if (enableDay < index + 1)
                {
                    status = -1;
                }
                else if (enableDay > index + 1)
                {
                    status = 1;
                }
                else
                {
                    status = 0;
                }
            }
            else
            {
                if (enableDay < index + 1)
                {
                    status = -1;
                }
                else
                {
                    status = 1;
                }
            }

            return status;
        }

    }
}
