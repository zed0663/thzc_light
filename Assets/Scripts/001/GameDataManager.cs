using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    [Header("CarData")] public CarData _CarNodeGroupData;
    public BuyCarRangeData _BuyCarRangeData;

    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
