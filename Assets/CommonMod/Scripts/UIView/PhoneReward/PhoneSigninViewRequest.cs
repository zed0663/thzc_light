using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class PhoneSigninViewRequest : UIViewRequest
    {

        public bool OpenSignin { get; private set; }

        public PhoneSigninViewRequest(bool openDailyQuests) : base(typeof(PhoneSigninUIState), true)
        {
            OpenSignin = openDailyQuests;
        }

    }
}
