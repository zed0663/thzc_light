using Monster.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Monster.UI
{

    //public enum DayLoginStatus{
    //    InClaim,
    //    NotLogged,
    //    NotClaim

    //}
    public class SigninItem : MonoBehaviour
    {

        private int Index;
        [SerializeField]
        private Button Btn;
        [SerializeField]
        private Text DayText;
        [SerializeField]
        private Animator HighlightEffect;
        [SerializeField]
        private Image RewardImage;
        [SerializeField]
        private Text RewardText;

        public void SetItem(int index, string day,Sprite tewardIcon,string tewardValue,int isStatus= 0)
        {
            Index = index;
            DayText.text = string.Format("第{0}天", day);
            RewardImage.sprite = tewardIcon;
            RewardText.text= tewardValue;
            SetStatus(isStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isEnable">-1=>未登录 0 =>可领取 1 =>已领取 </param>
        public void SetStatus(int isStatus)
        {
            if (isStatus < 0)
            {
                HighlightEffect.transform.gameObject.SetActive(false);
                Btn.interactable = true;
            }
            else if (isStatus == 0)
            {
                HighlightEffect.transform.gameObject.SetActive(true);
                Btn.interactable = true;
                
            }
            else
            {
                HighlightEffect.transform.gameObject.SetActive(false);
                Btn.interactable = false;
            }
        }

        public void OnClick()
        {
           
        }
        
    }
}
