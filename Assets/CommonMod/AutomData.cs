using System;
using System.Collections.Generic;
using Ez.Pooly;
using UnityEngine;

public class AutomData : MonoBehaviour
{
    public CarData _CarData;

    public BulletDataGroup _BulletsData;
    public ItemNodeImage _ItemNodeImage;
    public ItemNodeGroupData _ItemNodeDatal;
    public WeaponData _WeaponData;
    public ItemShopDetail _ItemShopDetail;

    public BasePlaneComponent[] _BasePlaneComponents;

    public PoolRegister _PoolRegister;
    public PoolyExtension _PoolyExtension;

    public void Start()
    {
        for (int i = 0; i < 81; i++)
        {
            double baseDamage = 6;
            int baseDamageUnit = 0;
            if (i!=0&&i<30)
            {
               
                float _rate= Mathf.Lerp(0.75f, 0.55f,(i / 29f));
                baseDamage = _CarData.GetProperties(i).Damage*(1+ _rate);
                baseDamageUnit = _CarData.GetProperties(i).DamageUnit;
                Helper.FixUnit(ref baseDamage, ref baseDamageUnit);
            }

            if (i >= 30)
            {
                float _rate = Mathf.Lerp(0.4f, 0.01f, (i / 80f));
                baseDamage = _CarData.GetProperties(i).Damage * (1 + _rate);
                baseDamageUnit = _CarData.GetProperties(i).DamageUnit;
                Helper.FixUnit(ref baseDamage, ref baseDamageUnit);
            }
            
            _CarData.GetProperties(i + 1).Damage = Math.Round(baseDamage, 2);
            _CarData.GetProperties(i + 1).DamageUnit = baseDamageUnit;
        }
    }
    public void Start2()
    {
        //List<PoolRegister.PoolData> _datas=new List<PoolRegister.PoolData>();
        //for (int i = 0; i < 51; i++)
        //{
        //    PoolRegister.PoolData _data = new PoolRegister.PoolData();
        //    _data.poolID = (PoolEnums.PoolId) (123 + (i));
        //    _data.poolPrefab = _BasePlaneComponents[i].transform;
        //    _data.quantity = 1;
        //    _data.IsExpand = true;
        //    _datas.Add(_data);
        //}

        //_PoolRegister.SetPoolData(_datas);
        //List<Transform> _list=new List<Transform>();
        //_list= _PoolRegister.GetPoolData();
        //for (int i = 0; i < _list.Count; i++)
        //{
        //    Pooly.Item _item = new Pooly.Item();
        //    _PoolyExtension.items.Add(_item);
        //}


        //Debug.Log("数量2:" + _PoolyExtension.items.Count);
        //for (int i = 0; i < _list.Count; i++)
        //{
        //    _PoolyExtension.items[i].prefab = _list[i].transform;
        //}


        for (int i = 0; i < _BasePlaneComponents.Length; i++)
        {
            Pooly.Item _item = new Pooly.Item();
            _PoolyExtension.items.Add(_item);
        }


        Debug.Log("数量2:" + _PoolyExtension.items.Count);
        for (int i = 0; i < _BasePlaneComponents.Length; i++)
        {
            _PoolyExtension.items[i].prefab = _BasePlaneComponents[i].transform;
        }
    }
    public void Start1()
    {
        List< CarDataProperties > _carData=new List<CarDataProperties>();
        for (int i = 0; i < 81; i++)
        {
            CarDataProperties _Properties=new CarDataProperties();

            int _index = 0;
            int _level = 0;
            if (i<_BulletsData.GetBulletDatas().Length)
            {
                _level = _BulletsData.GetBulletDatas()[i].Level;
                   _index = i;
            }
            else
            {
                _level = i+1;
                   _index = 29;
            }
            _Properties.BulletId = _BulletsData.GetBulletDatas()[_index].BulletId;
            _Properties.Level = _level;
            _Properties.PrefabName = "[Node] Item " + (i+1);
            _Properties.Damage = _BulletsData.GetBulletDatas()[_index].Damage;
            _Properties.DamageUnit = _BulletsData.GetBulletDatas()[_index].DamageUnit;
            _Properties.SpeedMoving = _BulletsData.GetBulletDatas()[_index].SpeedMoving;

            _Properties.CritChange = _BulletsData.GetBulletDatas()[_index].CritChange;
            _Properties.CritAmount = _BulletsData.GetBulletDatas()[_index].CritAmount;
            _Properties.DamageMissRange = _BulletsData.GetBulletDatas()[_index].DamageMissRange;
            _Properties.DamageCoefficient = _BulletsData.GetBulletDatas()[_index].DamageCoefficient;



            if (i < _ItemNodeImage.GetIcons().Length)
            {
                _Properties.Icon = _ItemNodeImage.GetIcons()[i].Icon;

            }
            else
            {
               
                _Properties.Icon = _BasePlaneComponents[i]._Sprite();

            }
           

            PoolEnums.PoolId ItemPoolId = 0;
            if (i < _ItemNodeDatal.GetItemNodeData().Length)
            {
                ItemPoolId = _ItemNodeDatal.GetItemNodeData()[_index].ItemPoolId;
                _index = i;
            }
            else
            {
                ItemPoolId = (PoolEnums.PoolId)(123+(i-30));
                _index = 29;
            }
            _Properties.ItemPoolId = ItemPoolId;
            _Properties.PerCircleTime = _ItemNodeDatal.GetItemNodeData()[_index].PerCircleTime;
            _Properties.ProfitPerSec = _ItemNodeDatal.GetItemNodeData()[_index].ProfitPerSec;
            _Properties.ProfitPerSecUnit = _ItemNodeDatal.GetItemNodeData()[_index].ProfitPerSecUnit;
            _Properties.Exp = _ItemNodeDatal.GetItemNodeData()[_index].Exp;
            _Properties.Prices = _ItemNodeDatal.GetItemNodeData()[_index].Prices;
            _Properties.PricesUnit = _ItemNodeDatal.GetItemNodeData()[_index].PricesUnit;
            _Properties.PricesUpgrade = _ItemNodeDatal.GetItemNodeData()[_index].PricesUpgrade;
            _Properties.PricesUnitUpgrade = _ItemNodeDatal.GetItemNodeData()[_index].PricesUnitUpgrade;
            _Properties.PricesCoefficient = _ItemNodeDatal.GetItemNodeData()[_index].PricesCoefficient;
            _Properties.PriceUpgradeCoefficient = _ItemNodeDatal.GetItemNodeData()[_index].PriceUpgradeCoefficient;
            _Properties.ProfitPerUpgradeCoefficient = _ItemNodeDatal.GetItemNodeData()[_index].ProfitPerUpgradeCoefficient;
            _Properties.BuyFromLevel = _ItemNodeDatal.GetItemNodeData()[_index].BuyFromLevel;


            if (i < _WeaponData.GetWeaponProperties().Length)
            {
                _index = i;
            }
            else
            {
                _index = 29;
            }
            _Properties.FireRate = _WeaponData.GetWeaponProperties()[_index].FireRate;
            _Properties.NumberBullets = _WeaponData.GetWeaponProperties()[_index].NumberBullets;


            if (i < _ItemShopDetail.GetItemProperties().Length)
            {
                _index = i;
            }
            else
            {
                _index = 29;
            }
            _Properties.VSpeed = _ItemShopDetail.GetItemProperties()[_index].Speed;
            _Properties.VEarning = _ItemShopDetail.GetItemProperties()[_index].Earning;
            _Properties.VDamage = _ItemShopDetail.GetItemProperties()[_index].Damage;

            _carData.Add(_Properties);
        }
        _CarData.SetCarDataPropertie(_carData.ToArray());
        
        /*
       public float  Speed;
        public float  Earning;
        public float  Damage;
     */
    }
}
