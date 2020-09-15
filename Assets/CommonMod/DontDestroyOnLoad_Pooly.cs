using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad_Pooly : MonoBehaviour
{
   
    void Start()
    {
        DontDestroyOnLoad(this);
    }


}
