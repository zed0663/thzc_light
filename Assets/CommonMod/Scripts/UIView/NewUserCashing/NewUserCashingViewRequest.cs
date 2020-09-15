using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class NewUserCashingViewRequest : UIViewRequest
    {

        public double RedPackAmount;

        public NewUserCashingViewRequest() : base(typeof(NewUserCashingUIState), true)
        {

        }



    }
}
