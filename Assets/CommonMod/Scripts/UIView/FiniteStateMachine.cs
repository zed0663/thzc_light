using System;
using System.Collections;
using System.Collections.Generic;
using Monster.Common;
using Monster.Events;
using UnityEngine;

namespace Monster.UI
{
    public class FiniteStateMachine : MonoBehaviour
    {
        [SerializeField]
        private State[] _states;


        [SerializeField]
        private State _startState;

        private readonly Dictionary<string, State> _statesDict = new Dictionary<string, State>();

        public delegate void StateSwitched(State oldState, State newState);

		public State CurrentState { get; private set; }

		
		public event FiniteStateMachine.StateSwitched StateSwitchedEvent;

		private void FireStateSwitchedEvent(State oldState, State newState)
		{
			if (this.StateSwitchedEvent != null)
			{
				this.StateSwitchedEvent(oldState, newState);
			}
		}

		private void Start()
		{
			this.InitStatesDict();
            if (_startState != null)
            {
                this.SwitchState(this._startState);
            }
        }

		public State GetState(Type stateType)
		{
			if (!this._statesDict.ContainsKey(stateType.Name))
			{
				Debug.LogError(string.Format("Requested state {0} does not exist.", stateType.Name));
				return null;
			}
			return this._statesDict[stateType.Name];
		}

		public T GetState<T>() where T : State
		{
			State state = this.GetState(typeof(T));
			return state as T;
		}

		public void SwitchState(string newStateName)
		{
			if (string.IsNullOrEmpty(newStateName))
			{
				Debug.LogError("FiniteStateMachine.SwitchState() new state name not valid");
				return;
			}
			if (!this._statesDict.ContainsKey(newStateName))
			{
				Debug.LogError(string.Format("Requested state {0} does not exist.", newStateName));
				return;
			}
			if (this.CurrentState != null && newStateName == this.CurrentState.GetType().Name)
			{
				Debug.Log(string.Format("FSM is already in state {0} and cannot switch.", newStateName));
				return;
			}
			State currentState = this.CurrentState;
			State state = this._statesDict[newStateName];
			if (this.CurrentState)
			{
				this.CurrentState.Leave(state);
				this.CurrentState.enabled = false;
			}
			//GameEvents.Invoke<UnemiShouldCloseEvent>(new UnemiShouldCloseEvent(this));
			this.CurrentState = state;
			this.CurrentState.enabled = true;
			this.CurrentState.Enter(currentState);
			this.FireStateSwitchedEvent(currentState, state);
		}

		
		public T SwitchState<T>() where T : State
		{
			this.SwitchState(typeof(T).Name);
			return this.CurrentState as T;
		}

		public void SwitchState(State s)
		{
			if (s == null)
			{
				Debug.LogError("Cannot switch to a null state.");
			}
			else
			{
				this.SwitchState(s.GetType().Name);
			}
		}

		private void InitStatesDict()
		{
			this._statesDict.Clear();
			int i = 0;
			int num = this._states.Length;
			while (i < num)
			{
				State state = this._states[i];
				string name = state.GetType().Name;
				if (!this._statesDict.ContainsKey(name))
				{
					this._statesDict.Add(name, state);
					state.fsm = this;
					state.enabled = false;
					state.Init();
				}
				i++;
			}
		}


		
	}
}
