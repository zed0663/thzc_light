using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class SigninViewRequest : UIViewRequest
    {

        public bool OpenSignin { get; private set; }
        public SigninViewRequest(bool openDailyQuests) : base(typeof(SigninUIState), true)
        {
            OpenSignin = openDailyQuests;
        }
    }
}