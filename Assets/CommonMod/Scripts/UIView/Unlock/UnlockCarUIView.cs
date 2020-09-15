
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monster.UI
{
    public class UnlockCarUIView : UIBaseView
    {
        [Header("Title")]
        public TextMeshProUGUI CarName;
        public TextMeshProUGUI CarLevel;
        public Image CarImage;

        [Header("Property")]
        public Image Earning;
        public Image Timing;
        public Image Damage;
        [Header("Reward")]
        public Button _Button;
        public Image RewardCarIcon;
        public TextMeshProUGUI RewardCarAmount;
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

        public void ShowView(CarDataProperties _unlockCarProperties, Sprite rewardCarSprite,int RewardAmount)
        {
            ObscuredInt nameLevel= _unlockCarProperties.Level>30? (ObscuredInt)(_unlockCarProperties.Level.GetDecrypted() - 30) : _unlockCarProperties.Level;

            CarName.text = ApplicationLanguage.GetItemName(nameLevel);
            CarLevel.text = string.Format("Lv."+ _unlockCarProperties.Level) ;
            CarImage.sprite = _unlockCarProperties.Icon;
            Earning.fillAmount = _unlockCarProperties.VEarning;
            Timing.fillAmount = _unlockCarProperties.VSpeed;
            Damage.fillAmount = _unlockCarProperties.VDamage;
            RewardCarIcon.sprite = rewardCarSprite;
            RewardCarAmount.text = string.Format("+" + RewardAmount);
        }
    }

}