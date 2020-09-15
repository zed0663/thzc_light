

using TMPro;

namespace Monster.UI
{
    public class ClaimRedPackUIView : UIBaseView
    {

        public TextMeshProUGUI CurrectRewardText;
        public TextMeshProUGUI TotalValueText;
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
            CurrectRewardText.text=string.Format("{0}元", currect);
            TotalValueText.text = string.Format("{0}元", total);
        }
    }

}