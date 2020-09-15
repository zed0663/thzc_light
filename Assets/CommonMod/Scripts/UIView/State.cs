using UnityEngine;
using System.Collections;

namespace Monster.UI
{
    public class State : MonoBehaviour
    {
        public virtual bool IsHUDState
        {
            get { return false; }
        }
        public GUIView View;

        protected bool _isInState;
        protected FiniteStateMachine _fsm;
        public FiniteStateMachine fsm
        {
            get { return this._fsm; }
            set { this._fsm = value; }
        }


        public virtual void Init()
        {
            this.View.Init();
        }

        public virtual void Enter(State oldState, bool openView)
        {
            this._isInState = true;
            if (openView)
            {
                this.View.Open(oldState);
            }
        }


        public virtual void Enter(State oldState)
        {
            this.Enter(oldState, true);
        }

        public virtual void Leave(State newState, bool closeView)
        {
            this._isInState = false;
            if (closeView)
            {
                this.View.Close(newState);
             
            }
        }


        public virtual void Leave(State newState)
        {
            this.Leave(newState, true);
        }

        private void Update()
        {
            if (this._isInState)
            {
                this.OnUpdate();
            }
        }

        protected virtual void OnUpdate()
        {
        }

        
    }
}
