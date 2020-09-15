﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoxBehaviour : NodeComponent
{
    
    [Header ("Config")] [SerializeField] private PoolEnums.PoolId _PoolId;

    
    private bool IsReady;
    [SerializeField] private Transform _Box;
    [SerializeField] private SpriteRenderer _SpriteBox;
    [SerializeField] private BoxData        _BoxData;

    public override NodeComponent TouchBusy ()
    {
        UnBox ();

        return this;
    }

    public override NodeComponent Init(CarDataProperties item_node_data)
    {
        base.Init(item_node_data);

        SetZoom(_YRow != 0);
        return this;
    }
    public override NodeComponent SetPosition (Vector3 position)
    {
        IsReady = false;

        transform.position = new Vector3 (position.x, position.y + 3, 0);
        transform.DOComplete (true);
        transform.DOMoveY (position.y, Durations.DurationDrop).SetEase (Ease.OutBack).OnComplete (() =>
        {
            IsReady = true;
            this.PlayAudioSound (AudioEnums.SoundId.BoxAppear);
        });

        return this;
    }
    public virtual void SetZoom(bool IsZoom)
    {
        _Box.localScale = IsZoom ? new Vector3(0.8f, 0.8f) : Vector3.one;
    }
    public void SetIcon (BoxEnums.BoxId id)
    {
        _SpriteBox.sprite = _BoxData.GetIcon (id);
    }

    public override PoolEnums.PoolId GetPoolId ()
    {
        return _PoolId;
    }

    private void UnBox ()
    {
        if (!IsReady) return;

        this.PlayAudioSound (AudioEnums.SoundId.BoxOpen);

        GameManager.Instance.SetBaseItemGrid (_ItemData,
                                              GetIndexX (),
                                              GetIndexY ());

        PoolExtension.SetPool (_PoolId, transform);

        GameActionManager.Instance.InstanceFxTapBox (transform.position);
        GameActionManager.Instance.InstanceFxFireWork (transform.position);
    }
}