using System;
using UnityEngine;
using System.Collections;
using Monster.Core;

namespace Monster.UI
{
    public class UIBaseState : State
    {
        private State _previousState;

        private State _nextState;

        private bool _closed = true;

        protected  bool _skipNextAnimation;
		public bool Closed
		{
			get
			{
				return this._closed;
			}
		}

		
		protected UIBaseView PopupView
		{
			get
			{
				return this.View as UIBaseView;
			}
		}

		public virtual void OpenPopup(UIViewRequest request)
		{

		}

		
		public override void Enter(State oldState)
		{
			if (this._previousState == null)
			{
				this._closed = false;
				
				this._previousState = oldState;
				UIBaseView.PopupCount++;
				if (_skipNextAnimation)
				{
					this.PopupView.SkipAnimation();
                   _skipNextAnimation = false;
				}
				else
				{
				//	SingletonMonobehaviour<CIGAudioManager>.Instance.PlayClip(Clip.PopupOpen);
					this.PopupView.AnimateIn();
				}
				base.Enter(oldState);
			}
		}

		public override void Leave(State newState)
		{
			if (this._nextState == null)
			{
				this._nextState = newState;
			}
			if (newState == this._previousState)
			{
				this._previousState = (this._nextState = null);
                UIBaseView.PopupCount--;
				base.Leave(newState);
			}
		}

		public void ClosePopup()
		{
			this.ClosePopup(null, true, false);
		}

		public void ClosePopup(Action callback)
		{
			this.ClosePopup(callback, true, false);
		}

		public void ClosePopup(Action callback, bool animate)
		{
			this.ClosePopup(callback, animate, false);
		}

		public virtual void ClosePopup(Action callback, bool animate, bool synchronous)
		{
			if (this.Closed)
			{
				return;
			}
		
			this._closed = true;
			if (animate)
			{
				this.PopupView.AnimateOut();
				//SingletonMonobehaviour<CIGAudioManager>.Instance.PlayClip(Clip.PopupClose, true);
			}
			else
			{
				this.PopupView.Close();
			}
			if (synchronous)
			{
				this.SwitchStateSynchronously(this._previousState, callback);
				return;
			}
			StartCoroutine(WaitAndSwitchState(!animate ? 0f :PopupView.TweenTime,_previousState, callback));
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.DismissPopup();
			}
		}

		protected virtual void DismissPopup()
		{
			this.ClosePopup();
		}

		private void SwitchStateSynchronously(State state, Action callback)
		{
			this._fsm.SwitchState(state);
			if (callback != null)
			{
				callback();
			}
		}

		private IEnumerator WaitAndSwitchState(float waitTime, State state, Action callback)
		{
			if (waitTime > 0f)
			{
				yield return new WaitForSeconds(waitTime);
			}
			_fsm.SwitchState(state);
			if (callback != null)
			{
				callback();
			}
            SingletonMonobehaviour<UIViewManager>.Instance.OnClosePopupEnd();
			yield break;
		}
    }
}
