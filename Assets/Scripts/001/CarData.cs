using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class CarData : ScriptableObject
{
    [Header("Data")] [SerializeField] private CarDataProperties[] carDatas;

    public void SetCarDataPropertie(CarDataProperties[] _data)
    {
        carDatas = _data;
    }

    public CarDataProperties GetProperties(ObscuredInt level)
    {
        for (int i = 0; i < carDatas.Length; i++)
        {
            if (carDatas[i].Level == level)
            {
                return carDatas[i];
            }
        }
        return carDatas[0];
    }

    public Sprite GetIcon(ObscuredInt level)
    {
        for (int i = 0; i < carDatas.Length; i++)
        {
            if (carDatas[i].Level == level)
            {
                return carDatas[i].Icon;
            }
        }
        return carDatas[0].Icon;
    }

    public CarDataProperties GetDataItemWithLevel(ObscuredInt level)
    {
        for (int i = 0; i < carDatas.Length; i++)
        {
            if (carDatas[i].Level == level)
            {
                return carDatas[i];
            }
        }

        return carDatas.Length > 0 ? carDatas[0] : null;
    }
}
[System.Serializable]
public class CarDataProperties
{

    #region base
    [Header("Base")]
    public ObscuredInt Level;
    public string PrefabName;
    public PoolEnums.PoolId ItemPoolId;
    public float PerCircleTime;
    public float ProfitPerSec;
    public int ProfitPerSecUnit;
    public int Exp;
    public float Prices;
    public int PricesUnit;
    public float PricesUpgrade;
    public int PricesUnitUpgrade;
    public float PricesCoefficient;
    public float PriceUpgradeCoefficient;
    public float ProfitPerUpgradeCoefficient;
    public ObscuredInt BuyFromLevel;

    #endregion

    #region View
    [Header("View")]
    public Sprite Icon;
    public float VSpeed;
    public float VEarning;
    public float VDamage;
    #endregion

    #region WeaponProperty
    [Header("WeaponProperty")]
    public float FireRate;
    public int NumberBullets;

    

    #endregion
    #region BulletsData
    [Header("BulletsData")]
    public PoolEnums.PoolId BulletId;
    public ObscuredDouble Damage;
    public ObscuredInt DamageUnit;
    public ObscuredFloat SpeedMoving;
    public ObscuredFloat CritChange;
    public ObscuredFloat CritAmount;
    [Range(0, 1f)]
    public ObscuredFloat DamageMissRange;
    public ObscuredFloat DamageCoefficient;

    #endregion

  
}