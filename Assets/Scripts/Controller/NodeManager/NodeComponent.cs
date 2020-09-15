using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using UnityEngine;

public class NodeComponent : MonoBehaviour
{
    #region Variables

    #endregion

    #region Variables

    //protected ItemNodeData _ItemNodeData;
    protected CarDataProperties _ItemData;

    protected int _IndexId;
    protected int _XColumn;
    protected int _YRow;

    private bool is_unbox;

    protected int level_upgrade;

    #endregion

    #region Action

    //public virtual NodeComponent Init (ItemNodeData item_node_data)
    //{
    //    _ItemNodeData = item_node_data;
    //    _IndexId      = GetInstanceID ();
        
    //    return this;
    //}

    public virtual NodeComponent Init(CarDataProperties item_node_data)
    {
        _ItemData = item_node_data;
        _IndexId = GetInstanceID();

        return this;
    }
    public virtual void SetZoom(bool IsZoom)
    {
       
    }

    public virtual NodeComponent TouchBusy ()
    {
        return this;
    }

    public virtual NodeComponent TouchHit ()
    {
        return this;
    }

    #endregion

    public Vector3 GetPosition ()
    {
        return transform.position;
    }

    public virtual NodeComponent SetEnable ()
    {
        return this;
    }

    public virtual NodeComponent SetPosition (Vector3 position)
    {
        transform.position = position;

        return this;
    }

    public virtual NodeComponent SetIndex (int xColumn, int yRow)
    {
        if (yRow != 0)
        {
            SetZoom(true);
        }
        _XColumn = xColumn;
        _YRow    = yRow;
        return this;
    }

    public virtual NodeComponent SetDisable ()
    {
        return this;
    }

    public virtual NodeComponent SetBusy (bool isBusy)
    {
        return this;
    }

    public virtual NodeComponent SetUnbox (bool isUnbox)
    {
        is_unbox = isUnbox;

        return this;
    }

    public virtual void SetDragStatus(bool isDrag)
    {

    }

    public virtual void SetStatePause (bool state, bool force_resume_state = false) { }

    public virtual void RefreshLevel ()
    {
        level_upgrade = PlayerData.GetNumberUpgradeItemProfitCoefficient (_ItemData.Level);
    }

    public virtual void ReturnToPool ()
    {
        //if (_ItemData == null)
        //{
        //    PoolExtension.SetPool(PoolEnums.PoolId.RedPackBox, transform);
        //    return;
        //}
        PoolExtension.SetPool (_ItemData.ItemPoolId, transform);
    }

    #region Helper

    public int GetIndexX ()
    {
        return _XColumn;
    }

    public int GetIndexY ()
    {
        return _YRow;
    }

    public ObscuredInt GetLevel ()
    {
        if (_ItemData == null)
        {
            return -1;
        }
        return _ItemData.Level;
    }

    public int GetExp ()
    {
        if (_ItemData == null)
        {
            return 0;
        }
        return _ItemData.Exp;
    }

    public int GetId ()
    {
        return _IndexId;
    }

    public virtual bool IsBusy ()
    {
        return true;
    }

    public virtual PoolEnums.PoolId GetPoolId ()
    {
        if (_ItemData == null)
        {
            return PoolEnums.PoolId.RedPackBox;
        }
        return _ItemData.ItemPoolId;
    }

    public virtual string GetKey ()
    {
        return string.Empty;
    }

    public virtual bool IsUnbox ()
    {
        return is_unbox;
    }

    #endregion
}