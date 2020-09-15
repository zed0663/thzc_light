
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monster.UI
{
    public class CashingUIView : UIBaseView
    {
        public TextMeshProUGUI CurrectAmount;
        public TextMeshProUGUI CashAmount;
        public CashingButton[] FastCashs;
        public TextMeshProUGUI Precondition;



        public Button SigninStatus;
        public override void Init()
        {
            base.Init();

            foreach (var FastCash in FastCashs)
            {
                FastCash.SetButtonStatus((Data.GameData.FastCashIndex >= FastCash.GetIndex()));
            }

            bool isOn = false;
            for (int i = 0; i < FastCashs.Length; i++)
            {
                if (!FastCashs[i].GetClaimStatus())
                {
                    if (!isOn)
                    {
                        FastCashs[i].SwitchButtonStatus(true);
                        isOn = true;
                    }
                    else
                    {
                        FastCashs[i].SwitchButtonStatus(false);
                    }
                }
            }
           

            if (Data.GameData.CashSigninStatus)
            {
                SigninStatus.interactable = false;
                SigninStatus.GetComponentInChildren<TextMeshProUGUI>().text="已签到";
            }
            else
            {
                SigninStatus.interactable = true;
                SigninStatus.GetComponentInChildren<TextMeshProUGUI>().text = "签到";
            }
           

        }
        public override void Open()
        {
            base.Open();

        }

        public override void Close()
        {

            base.Close();

        }

        public void UpdateViewAmount(int currectAmount, float cashAmount)
        {
            CurrectAmount.text = currectAmount.ToString();
            CashAmount.text = cashAmount+"元";
        }


        public void SigninClick()
        {
            ((CashingUIState)this.State).SigninClick();
        }



        public void CashClick(ObscuredInt index)
        {
            ((CashingUIState)this.State).CashButtonClick(index);
            SwtichCashButtonStatus(index);
        }


        void SwtichCashButtonStatus(int index)
        {
            for (int i = 0; i < FastCashs.Length; i++)
            {
                if (index== FastCashs[i].GetIndex())
                {
                    if (!FastCashs[i].GetClaimStatus())
                    {
                        FastCashs[i].SwitchButtonStatus(true);
                    }
                    else
                    {
                       Debug.LogError("只能领取一次 为什么还能再被点击?");
                       FastCashs[i].SetButtonStatus(true);
                       ((CashingUIState) this.State).CashButtonClick(-1);
                        return;
                    }
                }
                else
                {
                    FastCashs[i].SwitchButtonStatus(false);
                }
            }
        }


        
    }

}