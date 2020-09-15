using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class CashingTwoViewRequest : UIViewRequest
    {

        public double RedPackAmount;

        public CashingTwoViewRequest() : base(typeof(CashingTwoUIState), true)
        {

        }



    }
}
