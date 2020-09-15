using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NormalBullet : BulletsBehaviour
{
    public override void OnHit (Vector3 position)
    {
        base.OnHit (position);

        damage      = _carDataProperties.Damage;
        damage_unit = _carDataProperties.DamageUnit;

        damage = damage * Math.Pow (_carDataProperties.DamageCoefficient, level_weapon);

        damage += damage * Contains.EquipmentPercentIncreaseDamage;

        if (Random.Range (0.0f, 1.0f) < _carDataProperties.CritChange + _carDataProperties.CritChange * Contains.EquipmentPercentIncreaseCritRate)
        {
            damage *= _carDataProperties.CritAmount + _carDataProperties.CritAmount * Contains.EquipmentPercentIncreaseCritAmount;

            IsCritDamage = true;
        }
        else
        {
            IsCritDamage = false;
        }

        Helper.FixUnit (ref damage, ref damage_unit);

       // GetRealDamage (ref damage, ref damage_unit);

        transform_enemy.GetComponent<IHit> ().OnHit (damage, damage_unit);

        if (IsCritDamage)
        {
            GameActionManager.Instance.InstanceFxCritTextDamage (position,
                                                                 ApplicationManager.Instance.AppendFromUnit (damage, damage_unit)
            );
        }
        else
        {
            GameActionManager.Instance.InstanceFxTextDamage (position,
                                                             ApplicationManager.Instance.AppendFromUnit (damage, damage_unit)
            );
        }

        GameActionManager.Instance.InstanceFxHitDamage (position);

        this.PlayAudioSound (AudioEnums.SoundId.FxHitEnemy);
    }

    public void GetRealDamage (ref double _damage, ref int _damage_unit)
    {
        _damage *= 1 - Random.Range (0.0f, _carDataProperties.DamageMissRange);

        Helper.FixNumber (ref damage, ref damage_unit);
    }
}