using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Monster.Events
{
    public class UnemiShouldCloseEvent
    {
        private object _sender;
        public UnemiShouldCloseEvent(object sender)
        {
            this._sender = sender;
        }


        public object Sender
        {
            get { return this._sender; }
        }

        
	}
}
