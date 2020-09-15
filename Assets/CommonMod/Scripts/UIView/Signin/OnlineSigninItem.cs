using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineSigninItem : MonoBehaviour
{
    public GameObject NotArrived;
    public GameObject Arrived;
    public GameObject ClaimImage;
    public Text ValueText;
    public Text TimeText;

    public void SetItem(string value, int Time)
    {
        ValueText.text = value;
        TimeText.text = string.Format("{0}分钟", Time);

    }
    public void UpdateView(int _isStatus)
    {
        NotArrived.gameObject.SetActive(_isStatus==-1);
        Arrived.gameObject.SetActive(_isStatus == 1|| _isStatus == 0);
        ClaimImage.gameObject.SetActive(_isStatus == 1);
    }
}
