using System;
using UnityEngine;
using System.Collections;

namespace Monster.UI
{
    public class UIViewRequest
    {
        public Type PopupType { get; private set; }

        public bool Enqueue { get; private set; }

        public virtual bool IsValid
        {
            get { return true; }
        }

        protected UIViewRequest(Type popupType, bool enqueue)
        {
            this.PopupType = popupType;
            this.Enqueue = enqueue;
        }

    }
}
