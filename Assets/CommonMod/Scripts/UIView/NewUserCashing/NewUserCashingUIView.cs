
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monster.UI
{
    public class NewUserCashingUIView : UIBaseView
    {
       
        public TextMeshProUGUI CurrectAmount;
        public TextMeshProUGUI Description;
        public Transform DescriptionTips;

        public Transform EndTips;
        public TextMeshProUGUI EndDescription;

        public Button _Button;
        public TextMeshProUGUI _ButtonText;
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

        public void ShowView(double currect,double total)
        {
           
        }
    }

}