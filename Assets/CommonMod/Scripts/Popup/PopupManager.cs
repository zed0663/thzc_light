using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;

    public GameObject CanvasPanel;
    [HideInInspector] public PopupBase current;
    public GameObject PopupCanvas;
    public PopupBase[] PopupPrefabs;


    public Action onOpened;
    public Action onClosed;

    public Stack<PopupBase> popups = new Stack<PopupBase>();

    private List<PopupBase> popupPool = new List<PopupBase>();
    void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        
    }

    public void ShowPopup(PopupType type, PopupShowType option = PopupShowType.ReplaceCurrent, Action onYes = null, Action onNo = null)
    {
        PopupBase dialog = GetPopup(type);
        ShowPopup(dialog, option, onYes, onNo);

    }

    public void ShowPopup(PopupBase popup, PopupShowType option = PopupShowType.ReplaceCurrent, Action onYes = null, Action onNo = null)
    {
        if (current != null)
        {
            if (option == PopupShowType.DontShowIfOthersShowing)
            {
                Destroy(popup.gameObject);
            }
            else if (option == PopupShowType.ReplaceCurrent)
            {
                current.OnClose();
            }
            else if (option == PopupShowType.Stack)
            {
                current.Hide();
            }
        }

        current = popup;
        if (option != PopupShowType.ShowPrevious)
        {
            current.assentCall = onYes;
            current.cancelCall = onNo;
            current.onOpened += OnePopupOpened;
            current.onClosed += OnePopupClosed;
            popups.Push(current);
        }

        current.OnShow();
        if (onOpened != null)
        {
            onOpened();
        }

        

    }

    public void ShowOk(string title, string content,
        PopupShowType option = PopupShowType.ReplaceCurrent, Action onYes = null)
    {
        ShowCustomizePopup(PopupType.Ok, title, content, option, onYes);

    }

    public void ShowYesNoPopup(string title, string content,
        PopupShowType option = PopupShowType.ReplaceCurrent, Action onYes = null, Action onNo = null)
    {
        ShowCustomizePopup(PopupType.YesNo, title, content, option, onYes, onNo);

    }

    public void ShowCustomizePopup(PopupType _type, string title, string content, PopupShowType option = PopupShowType.ReplaceCurrent, Action onYes = null, Action onNo = null, Action other = null)
    {
        if (_type == PopupType.Ok || _type == PopupType.YesNo || _type == PopupType.Customize)
        {
            var _popup = (CustomizePopup)GetPopup(PopupType.Customize);

            if (_type == PopupType.Ok)
            {
                _popup.InitCustomizePopup(title, content, onYes);
            }
            else if (_type == PopupType.YesNo)
            {
                _popup.InitCustomizePopup(title, content, onYes, onNo);
            }
            
            else
            {
                _popup.InitCustomizePopup(title, content, onYes, onNo, other);
            }
            ShowPopup(_popup, option);
        }
        else
        {
            ShowPopup(_type, option);
            Debug.LogWarning("please use ShowPopup(PopupType type, PopupShowType option = PopupShowType.ReplaceCurrent),Not custom");
        }


    }


    public void ShowCustomizePopup(string title, string content, Action onYes, Action onNo,
        PopupShowType option = PopupShowType.ReplaceCurrent)
    {


    }


    public PopupBase GetPopup(PopupType type)
    {
        foreach (var po in popupPool)
        {
            if (po.popupType == type)
            {

                return po;
            }
        }

        foreach (var po in PopupPrefabs)
        {
            if (po.popupType == type)
            {
                PopupBase _popup = (PopupBase)Instantiate(po, PopupCanvas.transform);
                popupPool.Add(_popup);
                return _popup;
            }
        }

     //   PopupBase popup = Popups[(int) type];
      //  popup.PopupType = type;
        return null;
    }


    private void OnePopupOpened(PopupBase popup)
    {
        CanvasPanel.gameObject.SetActive(true);
        popup.gameObject.SetActive(true);
    }
    private void OnePopupClosed(PopupBase popup)
    {
        if (current == popup)
        {
            current.gameObject.SetActive(false);
            current = null;
            popups.Pop();
            if (onClosed != null && popups.Count == 0)
            {
                onClosed();
            }

            if (popups.Count > 0)
            {
                ShowPopup(popups.Peek(), PopupShowType.ShowPrevious);
            }
            else
            {
                CanvasPanel.gameObject.SetActive(false);
            }
        }

    }


    public void CloseCurrentPopup()
    {
        if (current != null)
        {
            current.OnClose();
        }

    }

    public void ClosePopup(PopupType type)
    {
        if (current == null) return;
        if (current.popupType == type)
        {
            current.OnClose();
        }

    }

    public bool isPopupShowing()
    {
        return current != null;
    }

    public bool isPopupShowing(PopupType _type)
    {
        if (current == null) return false;
        return current.popupType == _type;
    }

}
