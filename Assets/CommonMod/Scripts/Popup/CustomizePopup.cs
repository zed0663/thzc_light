using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class CustomizePopup : PopupBase
{

    public TextMeshPro titleText, messageText;

    
    private Action otherCall;

    public GameObject assentBtn;
    public GameObject cancelBtn;
    public GameObject otherBtn;

    public void InitCustomizePopup(string title, string content, Action onOK)
    {
        if (title != null) titleText.text = title;
        if (messageText != null) messageText.text = content;
        assentCall = onOK;
        assentBtn.SetActive(true);
        cancelBtn.SetActive(false);
        otherBtn.SetActive(false);
        //onNo = onNo;
    }
    public void InitCustomizePopup(string title, string content, Action onYes, Action onNo)
    {
        if (title != null) titleText.text = title;
        if (messageText != null) messageText.text = content;
        assentCall = onYes;
        cancelCall = onNo;
        assentBtn.SetActive(true);
        cancelBtn.SetActive(true);
        otherBtn.SetActive(false);
        //onNo = onNo;
    }
    public void InitCustomizePopup(string title, string content, Action onYes, Action onNo, Action onOther)
    {
        if (title != null) titleText.text = title;
        if (messageText != null) messageText.text = content;
        assentCall = onYes;
        cancelCall = onNo;
        otherCall = onOther;
        assentBtn.SetActive(true);
        cancelBtn.SetActive(true);
        otherBtn.SetActive(true);
        //onNo = onNo;
    }


    public void OnClickYesBtn()
    {
        if (assentCall != null)
        {
            assentCall();
        }
        OnClose();
    }
    public void OnClickNoBtn()
    {
        if (cancelCall != null)
        {
            cancelCall();
        }
        OnClose();
    }
    public void OnClickOtherBtn()
    {
        if (otherCall != null)
        {
            otherCall();
        }
        OnClose();
    }
}
