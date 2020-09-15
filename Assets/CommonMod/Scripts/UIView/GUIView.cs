using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class GUIView : MonoBehaviour
    {
        public State State;


        protected GameObject _localGO;


        private bool _isOpen;
        protected bool IsOpen
        {
            get { return this._isOpen; }
        }

        public virtual void Init()
        {
            this._localGO = base.gameObject;
            this._localGO.SetActive(true);
            this.Close();
        }


        public virtual void Open()
        {
            base.gameObject.SetActive(true);
            this._isOpen = true;
        }


        public virtual void Open(State oldState)
        {
            this.Open();
        }


        public virtual void Close()
        {
            base.gameObject.SetActive(false);
            this._isOpen = false;

        }


        public virtual void Close(State newState)
        {
            this.Close();
        }


        
    }
}
