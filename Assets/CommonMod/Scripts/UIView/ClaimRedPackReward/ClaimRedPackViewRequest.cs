using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class ClaimRedPackViewRequest : UIViewRequest
    {

        public double RedPackAmount;

        public ClaimRedPackViewRequest(double Amount ) : base(typeof(ClaimRedPackUIState), true)
        {
            RedPackAmount = Amount;
        }



    }
}
