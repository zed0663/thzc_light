using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class WheelLuckyViewRequest : UIViewRequest
    {

        public bool OpenSignin { get; private set; }

        public WheelLuckyViewRequest(bool openDailyQuests) : base(typeof(WheelLuckyUIState), true)
        {
            OpenSignin = openDailyQuests;
        }

    }
}
