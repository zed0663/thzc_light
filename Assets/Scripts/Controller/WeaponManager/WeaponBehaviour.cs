using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using UnityEngine;

[System.Serializable]
public struct WeaponGun
{
    public bool isRotation;
    public Transform transform_gun;
    public Transform[] transform_holder;
    public float minRotation;
    public float maxRotation;
    public float RotationRate;
}


public class WeaponBehaviour : MonoBehaviour, IShooter
{
    [Header ("Handle")] [SerializeField] private Transform[] transform_holder;

    [Header ("Config")] [SerializeField] private float max_angle_each_bullet = 15;
    [SerializeField]                     private float min_angle_each_bullet = 5;

    public System.Action OnShooter;
    
    #region Variables

    private bool IsActiveShooter;
    private bool IsPauseShooter;

    private string enemy_Tag;

    private CarDataProperties WeaponProperty;

    private CoroutineHandle handle_shooter;

    private Vector3 position_shooter;

    private int weapon_level_updated;

    public WeaponGun[] GunTransform;
    private Sequence _sequence;
    #endregion

    #region Action

    public void Init (CarDataProperties data, string tag_enemy)
    {
        WeaponProperty = data;
        enemy_Tag      = tag_enemy;
    }

    public void Active ()
    {
        if (IsActiveShooter)
            return;

        IsActiveShooter = true;

        handle_shooter = Timing.RunCoroutine (Enumerator_Fire ());
        GunRotation();
    }

    public void DeActive ()
    {
        if (!IsActiveShooter)
            return;

        IsActiveShooter = false;

        Timing.KillCoroutines (handle_shooter);

        if (_sequence!=null)
        {
            _sequence.Kill(false);
            _sequence = null;
        }
    }

    public void RefreshLevelUpdated (int level_upgraded)
    {
        weapon_level_updated = level_upgraded;
    }

    public void Pause ()
    {
        IsPauseShooter = true;
    }

    public void Resume ()
    {
        IsPauseShooter = false;
    }

    public void Fire ()
    {
        if (GunTransform!=null&& GunTransform.Length>0)
        {
            foreach (var gun in GunTransform)
            {
                for (int i = 0; i < gun.transform_holder.Length; i++)
                {
                    position_shooter = gun.transform_holder[i].position;

                    var angle = Random.Range(min_angle_each_bullet, max_angle_each_bullet);
                    var angle_start = gun.transform_holder[i].eulerAngles.z + 0 - (WeaponProperty.NumberBullets - 1) / 2f * angle;

                    for (int j = 0; j < WeaponProperty.NumberBullets; j++)
                    {
                        GameActionManager.Instance.InstanceBullets(WeaponProperty.Level, WeaponProperty, weapon_level_updated, position_shooter, enemy_Tag, new Vector3(0, 0, angle_start + angle * j));

                    }
                    GameActionManager.Instance.InstanceMuzzle(position_shooter, gun.transform_holder[i].eulerAngles);
                }
            }
        }
        else
        {
            for (int i = 0; i < transform_holder.Length; i++)
            {
                position_shooter = transform_holder[i].position;

                var angle = Random.Range(min_angle_each_bullet, max_angle_each_bullet);
                var angle_start = transform_holder[i].localEulerAngles.z + 0 - (WeaponProperty.NumberBullets - 1) / 2f * angle;

                for (int j = 0; j < WeaponProperty.NumberBullets; j++)
                {
                    GameActionManager.Instance.InstanceBullets(WeaponProperty.Level, WeaponProperty, weapon_level_updated, position_shooter, enemy_Tag, new Vector3(0, 0, angle_start + angle * j));

                }

                GameActionManager.Instance.InstanceMuzzle(position_shooter, transform_holder[i].localEulerAngles);
            }
        }
       

        if (OnShooter != null)
        {
            OnShooter ();
        }
        
        this.PlayAudioSound (AudioEnums.SoundId.Shoot);
    }

    #endregion

    #region Enumerator

    private IEnumerator<float> Enumerator_Fire ()
    {
        while (IsActiveShooter)
        {
            yield return Timing.WaitForSeconds (WeaponProperty.FireRate - Contains.SpeedUpTimes * WeaponProperty.FireRate);

            if (!IsPauseShooter)
            {
                Fire ();

            }
        }

        

    }

    void GunRotation()
    {
        if (IsActiveShooter)
        {
            foreach (var gun in GunTransform)
            {
                if (gun.isRotation)
                {
                    //float _rate = Mathf.Abs(gun.maxRotation - gun.minRotation) / gun.RotationRate;

                    Vector3 _DefaultAngle = gun.transform_gun.localRotation.eulerAngles;
                     _sequence = DOTween.Sequence();
                    _sequence.Append(gun.transform_gun.DORotate(new Vector3(0, 0, gun.maxRotation), RotationDuration(gun.transform_gun.localRotation.z, gun.maxRotation, gun.RotationRate))
                            .SetEase(Ease.Linear))
                        .Append(gun.transform_gun.DORotate(new Vector3(0, 0, gun.minRotation), RotationDuration(gun.maxRotation, gun.minRotation, gun.RotationRate))
                            .SetEase(Ease.Linear))
                        .Append(gun.transform_gun.DORotate(_DefaultAngle, RotationDuration(gun.minRotation, _DefaultAngle.z, gun.RotationRate))
                        .SetEase(Ease.Linear));
                    _sequence.SetLoops(-1);


                }
            }
        }
    }

    float RotationDuration(float currect_z, float target_z,float rate)
    {
        float _rate = Mathf.Abs(currect_z - target_z) / rate;
       //Debug.Log("速率:"+ _rate);
        return _rate;
    }


    #endregion
}