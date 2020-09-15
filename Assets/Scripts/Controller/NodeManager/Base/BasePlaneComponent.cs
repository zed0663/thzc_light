using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BasePlaneComponent : NodeComponent, IIdle
{
    [Header ("Data")] [SerializeField] private SpriteRenderer sprite_renderer;

    [SerializeField] private Transform transform_renderer;

    [Header ("Shooter")] [SerializeField] private WeaponBehaviour weapon_behaviour;

    private new Transform BoxTransform;

    private BasePlaneMoving _BasePlaneMoving;

    private bool IsMinerGold;
    private bool IsIdleGold;

    private bool IsIdlePause;
    private bool IsIdleMinerGold;

    private SpriteRenderer[] _spriteRenderers;

    private bool IsReady;

    public Sprite _Sprite()
    {
        return sprite_renderer.sprite;
    }

    public override NodeComponent Init (CarDataProperties _item_node_data)
    {
        if (ReferenceEquals (BoxTransform, null))
        {
            BoxTransform = gameObject.transform;
        }

        _ItemData = _item_node_data;
        _IndexId      = GetInstanceID ();

        sprite_renderer.enabled = true;

        int _weaponLevel= _item_node_data.Level;
        if (_weaponLevel > 30)
        {
            _weaponLevel -= 30;
        }

        weapon_behaviour.Init (_item_node_data, TagEnums.GetKey (TagEnums.TagId.Enemy));

        SetStatePause (true);

        weapon_behaviour.OnShooter = WeaponBehaviourOnOnShooter;

        RefreshLevel ();

        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
       
        return this;
    }



    public void SetPositionAnimation ()
    {
        IsReady = false;
        Vector3 _position = transform.position;
        base.transform.position = new Vector3(_position.x, _position.y + 3, 0);
        base.transform.DOComplete(true);
        base.transform.DOMoveY(_position.y, Durations.DurationDrop).SetEase(Ease.OutBack).OnComplete(() =>
        {
            IsReady = true;
            this.PlayAudioSound(AudioEnums.SoundId.BoxAppear);
        });

    }

    public override void SetZoom(bool IsZoom)
    {
        sprite_renderer.transform.localScale = IsZoom ? new Vector3(0.8f, 0.8f) : Vector3.one;
    }

    private void WeaponBehaviourOnOnShooter ()
    {
        GameManager.Instance.FxTapNode (BoxTransform, null);
    }

    public override NodeComponent SetIndex (int xColumn, int yRow)
    {
        IsIdleMinerGold = !GameManager.Instance.IsBaseActiveWeapon (yRow);

        RefreshIndexActiveWeapon (yRow);

        

        return base.SetIndex (xColumn, yRow);
    }

    public override NodeComponent SetBusy (bool IsBusy)
    {
        IsMinerGold = IsBusy;

        return base.SetBusy (IsBusy);
    }

    public override NodeComponent SetEnable ()
    {
        IdleRegister ();

        weapon_behaviour.Active ();

        sprite_renderer.enabled = true;

        return base.SetEnable ();
    }

    public override NodeComponent SetDisable ()
    {
        IdleUnRegister ();

        weapon_behaviour.DeActive ();

        sprite_renderer.enabled = false;

        return base.SetDisable ();
    }

    public void SetPlaneMoving (BasePlaneMoving basePlaneMoving)
    {
        _BasePlaneMoving = basePlaneMoving;
    }

    public void RefreshIndexActiveWeapon (int row)
    {
        if (GameManager.Instance.IsBaseActiveWeapon (row))
        {
            weapon_behaviour.Resume ();
        }
        else
        {
            weapon_behaviour.Pause ();
        }
    }

    public override void RefreshLevel ()
    {
        base.RefreshLevel ();

        weapon_behaviour.RefreshLevelUpdated (level_upgrade);
    }

    #region Action

    public override NodeComponent TouchBusy ()
    {
        if (_BasePlaneMoving != null)
        {
            _BasePlaneMoving.Stop ();

            GameManager.Instance.SetItemBackToNode (GetIndexX (), GetIndexY (), _BasePlaneMoving, this);
        }

        _BasePlaneMoving = null;

        return this;
    }

    public override NodeComponent TouchHit ()
    {

        this.PlayAudioSound (AudioEnums.SoundId.TapOnItem);

        return this;
    }

    public void EarnCoins ()
    {
        if (!IsIdleMinerGold)
            return;


    }

    public void IdleRegister ()
    {
        if (IsIdleGold == true)
            return;

        IsIdleGold = true;

        GameIdleAction.Instance.RegisterIdle (this, _ItemData.PerCircleTime);
        EarningManager.Instance.RegisterData (_ItemData);
       
    }

    public void IdleUnRegister ()
    {
        if (IsIdleGold == false)
            return;

        IsIdleGold = false;

        GameIdleAction.Instance.UnRegisterIdle (this);
        EarningManager.Instance.UnRegisterData (_ItemData);
      
    }

    public override void SetStatePause (bool state, bool force_resume_state = false)
    {
        if (force_resume_state)
        {
            weapon_behaviour.Resume ();

            IsIdlePause = false;

            return;
        }

        if (state != IsIdlePause)
        {
            IsIdlePause = state;

            if (IsIdlePause)
            {
                RefreshIndexActiveWeapon (_YRow);
            }
            else
            {
                weapon_behaviour.Pause ();
            }
        }
    }


    public override void SetDragStatus(bool isDrag)
    {
        //sprite_renderer.sortingOrder = 5;
        foreach (var _sprite in _spriteRenderers)
        {
            _sprite.sortingOrder += isDrag ? 5 : -5;
        }

        if (_YRow != 0)
        {
           SetZoom(!isDrag);
        }
        

    }

    #endregion

    #region Helper

    public override bool IsBusy ()
    {
        return IsMinerGold;
    }

    public bool IsStop ()
    {
        return !IsIdleGold;
    }

    #endregion
}