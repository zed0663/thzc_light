using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPackComponent : NodeComponent, IIdle
{

    public override NodeComponent Init(CarDataProperties _item_node_data)
    {

     //   _ItemNodeData = _item_node_data;
        _IndexId = GetInstanceID();

        SetStatePause(true);

        return this;
    }




    public void IdleRegister()
    {
        throw new System.NotImplementedException();
    }

    public void IdleUnRegister()
    {
        throw new System.NotImplementedException();
    }

    public void EarnCoins()
    {
        throw new System.NotImplementedException();
    }

    public bool IsStop()
    {
        throw new System.NotImplementedException();
    }
}
