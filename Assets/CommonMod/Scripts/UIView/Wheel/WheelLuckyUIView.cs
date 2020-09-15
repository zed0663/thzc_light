using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Monster.UI;
using UnityEngine;

namespace Monster.UI
{
    public class WheelLuckyUIView : UIBaseView
    {

        public Transform wheelPanel;
        public Transform rewardPanel;

        private Tweener _rot1;
        private Tweener _rot2;
        [SerializeField] private AnimationCurve _rotationCurve;

        private int _lastRewardZone;

        public float CurrentPosition
        {
            get
            {
                return this.wheelPanel.transform.localRotation.eulerAngles.z;
            }
        }
        private int RewardZone
        {
            get
            {
                return Mathf.RoundToInt(this.CurrentPosition * 8 / 360f - 0.5f) % 8;
            }
        }
        public override void Init()
        {
            base.Init();
            this._rot1 = wheelPanel.DOLocalRotate(Vector3.forward * 0f, 7f, RotateMode.FastBeyond360).SetAutoKill(false).SetEase(this._rotationCurve).Pause<Tweener>();
            this._rot2 = rewardPanel.DOLocalRotate(Vector3.forward * 0f, 7f, RotateMode.FastBeyond360).SetAutoKill(false).SetEase(this._rotationCurve).Pause<Tweener>().OnUpdate(new TweenCallback(this.RotationUpdated)).OnComplete(new TweenCallback(this.RotationEnded));

        }
        private void RotationUpdated()
        {
            if (this._lastRewardZone != this.RewardZone)
            {
               Debug.Log("播放声音");
                this._lastRewardZone = this.RewardZone;
            }
        }
        private void RotationEnded()
        {
           
        }
        public override void Open()
        {
            base.Open();

        }

        public override void Close()
        {

            base.Close();

        }

        public void Spin(int _rewardID)
        {
            float velocity = -10;
            velocity = (Mathf.Min(Mathf.Abs(velocity), 10) * (int)Mathf.Sign(velocity));
            float num;
            if (velocity < 0)
            {
                num = (float)velocity * 360f - ((float)_rewardID - 0.5f) * 360f / 8;
            }
            else
            {
                num = (float)(velocity + 1) * 360f - ((float)_rewardID - 0.5f) * 360f / 8;
            }
            float _currentSpinOffset = Mathf.Sign((float)velocity) * Random.Range(-0.2f, 0.45f) * 360f / 8f;
            float num2 = 2 * (int)((5f + Mathf.Abs(velocity)) / 2f);
            _rot1.ChangeEndValue(Vector3.forward * (num + _currentSpinOffset), num2 - 0.05f, true).Restart(true);
            _rot2.ChangeEndValue(Vector3.forward * (num + _currentSpinOffset), num2 - 0.05f, true).Restart(true);
        }
    }

}