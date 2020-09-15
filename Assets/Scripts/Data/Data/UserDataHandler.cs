using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataHandler : DataHandler<UserDataHandler>
{
    UserData _userData;

    public UserData GetUserData()
    {
        return _userData;
    }

}
