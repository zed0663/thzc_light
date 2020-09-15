using System;
using System.Collections;
using System.Collections.Generic;
using Monster.Core;
using UnityEngine;


namespace Monster.UI
{
    public class UIViewManager : SingletonMonobehaviour<UIViewManager>
    {
        [SerializeField]
        private FiniteStateMachine _fsm;

        [SerializeField]
        private GameObject _overlay;

        [SerializeField]
        private GameObject UIViewBackground;

		private readonly LinkedList<UIViewRequest> _popupQueue = new LinkedList<UIViewRequest>();
		public bool IsShowingPopup
		{
			get
			{
				return this._fsm.CurrentState is UIBaseState;
			}
		}

		private void Awake()
		{base.Awake();
			this._fsm.StateSwitchedEvent += this.OnStateSwitched;
            UIViewBackground.SetActive(false);
		}

		protected override void OnDestroy()
		{
			if (this._fsm != null)
			{
				this._fsm.StateSwitchedEvent -= this.OnStateSwitched;
			}
			base.OnDestroy();
		}

		public void ShowOverlay()
		{
			this._overlay.SetActive(true);
		}

		public void HideOverlay()
		{
			this._overlay.SetActive(false);
		}

        public void RequestPopup(UIViewRequest request)
        {
            if (!this.IsShowingPopup || !request.Enqueue)
            {
                this.OpenPopupNow(request);
                return;
            }

            this._popupQueue.AddLast(request);
        }

        public void ClosePopup(UIViewRequest request, Action callback, bool animate, bool synchronous)
		{
			((UIBaseState)this._fsm.GetState(request.PopupType)).ClosePopup(callback, animate, synchronous);

		}

        public void RemovePopupRequests<T>() where T : UIBaseState
		{
            LinkedListNode<UIViewRequest> next;
            for (LinkedListNode<UIViewRequest> linkedListNode = this._popupQueue.First;
                linkedListNode != null;
                linkedListNode = next)
            {
                next = linkedListNode.Next;
                if (linkedListNode.Value.PopupType == typeof(T))
                {
                    this._popupQueue.Remove(linkedListNode);
                }
            }
        }

        public void CloseRecursive(bool animated = true)
		{
			while (this.IsShowingPopup)
			{
				((UIBaseState)this._fsm.CurrentState).ClosePopup(null, animated, true);
			}
		}
        public void CloseOpenPopup()
		{
            UIBaseState popupBaseState = this._fsm.CurrentState as UIBaseState;
			if (popupBaseState != null)
			{
				popupBaseState.ClosePopup();
			}
		}

		public bool IsPopupOpen<T>() where T : UIBaseState
		{
			return this._fsm.CurrentState is T;
		}

		private void OpenPopupNow(UIViewRequest request)
		{
			if (!request.IsValid)
			{
				this.TryOpenNextPopup();
				return;
			}
            UIBaseState popupBaseState = (UIBaseState)this._fsm.GetState(request.PopupType);
		
			if (this._fsm.CurrentState == popupBaseState)
			{
				return;
			}
			this._fsm.SwitchState(popupBaseState);
			if (this._fsm.CurrentState == popupBaseState)
			{
                UIViewBackground.gameObject.SetActive(true);
				popupBaseState.OpenPopup(request);
				popupBaseState.transform.parent.SetAsLastSibling();
				return;
			}
			Debug.LogWarning("State change unsuccessfull! PopupManager may have become corrupted");
		}
        private void TryOpenNextPopup()
		{
			if (this._popupQueue.Count == 0)
			{
				return;
			}
            UIViewRequest value = this._popupQueue.First.Value;
			this._popupQueue.RemoveFirst();
			this.OpenPopupNow(value);
		}

		
		private void OnStateSwitched(State oldState, State newState)
		{
            if (newState is MainMenuStatus)
            {
                this.TryOpenNextPopup();
            }

           // Debug.Log("窗口 OnStateSwitched:"+ newState);
		}


        public void OnClosePopupEnd()
        {
            UIViewBackground.gameObject.SetActive(false);

		}
    }
}
