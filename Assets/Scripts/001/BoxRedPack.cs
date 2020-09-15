using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoxRedPack : NodeComponent
{
    [Header("Config")] [SerializeField] private PoolEnums.PoolId _PoolId;
    private bool IsReady;

    [SerializeField] private SpriteRenderer _SpriteBox;
    [SerializeField] private TextMeshPro _AmountTextMesh;
    private double RedPackAmount;



    public override NodeComponent SetPosition(Vector3 position)
    {
        IsReady = false;

        transform.position = new Vector3(position.x, position.y + 3, 0);
        transform.DOComplete(true);
        transform.DOMoveY(position.y, Durations.DurationDrop).SetEase(Ease.OutBack).OnComplete(() =>
        {
            IsReady = true;
            this.PlayAudioSound(AudioEnums.SoundId.BoxAppear);
        });

        return this;
    }
    public override void SetZoom(bool IsZoom)
    {
        _SpriteBox.transform.localScale = IsZoom ? new Vector3(0.8f, 0.8f) : Vector3.one;
    }
    public void SetAmount(float value)
    {
        _SpriteBox.sprite = GameData.Instance.BoxData.GetIcon(BoxEnums.BoxId.RedPack);
        double amount = Random.Range(0.1f, 1.1f);
        amount = Math.Round(amount, 2);
        RedPackAmount = amount;
        _AmountTextMesh.text = amount.ToString();
    }
    public override PoolEnums.PoolId GetPoolId()
    {
        return _PoolId;
    }
    public override NodeComponent TouchBusy()
    {
        UnBox();

        return this;
    }

    private void UnBox()
    {
        if (!IsReady) return;

        this.PlayAudioSound(AudioEnums.SoundId.BoxOpen);


        GameManager.Instance.SetNodeInGrid(GetIndexX(), GetIndexY(), null);
        GameManager.Instance.SetFreeIndexGrid(GetIndexX(), GetIndexY());
     

        PoolExtension.SetPool(_PoolId, transform);
        GameActionManager.Instance.InstanceFxTapBox(transform.position);
        GameActionManager.Instance.InstanceFxFireWork(transform.position);
        UIGameManager.Instance.OpenClaimRedPack(RedPackAmount);

        //  RandomGiftManager.Instance.EnableHud();
    }
}
