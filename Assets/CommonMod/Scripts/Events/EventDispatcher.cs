using System;
using System.Collections.Generic;

namespace Monster.Events
{
    public class EventDispatcher
    {

        private Dictionary<Type, HashSet<Action<object>>> _events = new Dictionary<Type, HashSet<Action<object>>>();

        private Dictionary<object, Action<object>> _lookupTable = new Dictionary<object, Action<object>>();

        public void Subscribe<T>(Action<T> callback) where T : class
        {
            Action<object> action = delegate(object o) { callback((T) ((object) o)); };
            if (!this._events.ContainsKey(typeof(T)))
            {
                this._events[typeof(T)] = new HashSet<Action<object>>();
            }

            this._events[typeof(T)].Add(action);
            this._lookupTable.Add(callback, action);
        }


        public void Unsubscribe<T>(Action<T> callback) where T : class
        {
            if (this._events.ContainsKey(typeof(T)))
            {
                this._events[typeof(T)].Remove(this._lookupTable[callback]);
            }

            this._lookupTable.Remove(callback);
        }

        public void Invoke<T>(T evt) where T : class
        {
            HashSet<Action<object>> hashSet;
            if (this._events.TryGetValue(typeof(T), out hashSet) && hashSet != null)
            {
                foreach (Action<object> action in hashSet)
                {
                    action(evt);
                }
            }
        }



    }
}