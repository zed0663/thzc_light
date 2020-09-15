using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Monster.UI
{
    public class UIBaseView : GUIView
    {
        Sequence _sequence;

        public static int PopupCount;
        private float _TweenTime;
        protected UIBaseState PopupState
        {
            get
            {
                return this.State as UIBaseState;
            }
        }

        public override void Init()
        {
            base.Init();
           
        }
        public float TweenTime
        {
            get
            {
                return _TweenTime;
            }
        }

        public virtual void OnBlackOverlayClicked()
        {
            this.OnCloseClicked();
        }


        public virtual void OnCloseClicked()
        {
            this.PopupState.ClosePopup();
        }


        public void AnimateIn()
        {
            _TweenTime = 0.4f;
            transform.localScale=Vector3.zero;
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOScale(1.1f, 0.2f))
                .Append(transform.DOScale(1f, 0.2f));

            _sequence.Play();
        }

        public void SkipAnimation()
        {
            _TweenTime = 0;
            if (_sequence!=null)
            {
                _sequence.Kill();
                _sequence = null;
            }
            transform.localScale = Vector3.one;
        }

        public void AnimateOut()
        {
            if (this._sequence != null)
            {
                if (this._sequence.active)
                {
                    this._sequence.Complete(true);
                }
               
            }
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOScale(1.1f, 0.2f))
                .Append(transform.DOScale(0f, 0.2f)).OnComplete(() =>
                {


                });
        }

	}
}
