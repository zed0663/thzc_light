using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockBullet : BulletsBehaviour
{
    [Header ("Config")] [SerializeField] private float size_explode;

    public override void OnHit (Vector3 position)
    {
        base.OnHit (position);

        var physic = Physics2D.OverlapCircleAll (position, size_explode);

        damage      = _carDataProperties.Damage;
        damage_unit = _carDataProperties.DamageUnit;

        damage = damage * Math.Pow (_carDataProperties.DamageCoefficient, level_weapon);

        damage += damage * Contains.EquipmentPercentIncreaseDamage;

        if (Random.Range (0.00f, 1.00f) < _carDataProperties.CritChange + _carDataProperties.CritChange * Contains.EquipmentPercentIncreaseCritRate)
        {
            damage *= _carDataProperties.CritAmount + _carDataProperties.CritAmount * Contains.EquipmentPercentIncreaseCritAmount;

            IsCritDamage = true;
        }
        else
        {
            IsCritDamage = false;
        }

        Helper.FixUnit (ref damage, ref damage_unit);

        //GetRealDamage (ref damage, ref damage_unit);

        for (int i = 0; i < physic.Length; i++)
        {
            var transform_tag = physic[i].transform;

            if (transform_tag.CompareTag (enemy_tag))
            {
                transform_tag.GetComponent<IHit> ().OnHit (damage, damage_unit);

                if (IsCritDamage)
                {
                    GameActionManager.Instance.InstanceFxCritTextDamage (transform_tag.position,
                                                                         ApplicationManager.Instance.AppendFromUnit (damage, damage_unit)
                    );
                }
                else
                {
                    GameActionManager.Instance.InstanceFxTextDamage (transform_tag.position,
                                                                     ApplicationManager.Instance.AppendFromUnit (damage, damage_unit)
                    );
                }
            }
        }

        GameActionManager.Instance.InstanceFxExplodeRocket (position);

        this.PlayAudioSound (AudioEnums.SoundId.FxRocketExplode);
    }

    public void GetRealDamage (ref double _damage, ref int _damage_unit)
    {
        _damage *= 1 - Random.Range (0.0f, _carDataProperties.DamageMissRange);

        Helper.FixNumber (ref damage, ref damage_unit);
    }
}