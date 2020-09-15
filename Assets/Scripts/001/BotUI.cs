using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotUI : MonoBehaviour
{
    [Header("BotUI Hub")]
    [SerializeField] private Transform buyButtonTransform;
    [SerializeField] private Transform BuyCarButton;
    [SerializeField] private Transform  DeleteButton;
    [SerializeField] private TextMeshProUGUI CarAmounText;
    [SerializeField] private RectTransform CarProcessTransform;
    [SerializeField] private Animator CarProcessAnimation;


    private Vector2 _CarProcessPosition;
    private bool ButtonFlashAnimtion;

    void Start()
    {
        _CarProcessPosition = CarProcessTransform.sizeDelta;
        CarProcessTransform.localPosition = new Vector2(CarProcessTransform.localPosition.x, -_CarProcessPosition.y );
    }

    public void SetCarAmounText(int amount,int total)
    {
        CarAmounText.text =string.Format("{0}/{1}",amount, total);
    }


    public void SetCarProcess(float _process)
    {

        float currect = _CarProcessPosition.y * _process;
        CarProcessTransform.localPosition=new Vector3(CarProcessTransform.localPosition.x, currect - _CarProcessPosition.y);
    }

    public void SetCarProcessAnimation()
    {
        //CarProcessAnimation.Play();
    }


    public void ButtonFlash()
    {
        if (!ButtonFlashAnimtion)
        {
            ButtonFlashAnimtion = true;
               Vector3 _position = buyButtonTransform.localPosition;
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(buyButtonTransform.GetComponent<Image>().DOColor(Color.red, 0.15f)).OnComplete(delegate
                {
                    buyButtonTransform.GetComponent<Image>().color = Color.white;
                })
                .Append(buyButtonTransform.GetComponent<Image>().DOColor(Color.white, 0.1f))
                .Append(buyButtonTransform.GetComponent<Image>().DOColor(Color.red, 0.1f)).OnComplete(delegate
                {
                    buyButtonTransform.GetComponent<Image>().color = Color.white;
                });
            buyButtonTransform.DOShakePosition(0.1f, 15f).SetLoops(3).OnComplete(delegate
            {
                buyButtonTransform.localPosition = _position;
                ButtonFlashAnimtion = false;
            });
        }
        

    }


    public Vector3 BuyCarButtonPosition()
    {
        return BuyCarButton.position;
    }
}
