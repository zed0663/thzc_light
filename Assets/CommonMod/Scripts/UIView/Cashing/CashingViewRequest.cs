using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class CashingViewRequest : UIViewRequest
    {

        public double RedPackAmount;

        public CashingViewRequest() : base(typeof(CashingUIState), true)
        {

        }



    }
}
