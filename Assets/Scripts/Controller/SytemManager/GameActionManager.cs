﻿using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using Monster.Common;
using UnityEngine;

public class GameActionManager : Singleton<GameActionManager>
{
    #region Action

    public void SetExp (int exp)
    {
        if (PlayerData.Level == GameConfig._MaxLevel)
            return;

        PlayerData.Exp += exp;

        if (PlayerData.Exp >= Contains.ExpNeedReach)
        {
            PlayerData.Exp = PlayerData.Exp - Contains.ExpNeedReach;
            LevelUp ();
        }

        PlayerData.SaveExp ();
        UIGameManager.Instance.UpdateTextExp ();
    }

    public void LevelUp ()
    {
        PlayerData.Level = Mathf.Clamp (PlayerData.Level + 1, 0, GameConfig._MaxLevel);

        PlayerData.SaveLevel ();

        UIGameManager.Instance.UpdateTextLevel ();

        ApplicationManager.Instance.LoadLevelParameter ();
       // GameManager.Instance.InstanceBase ();
        GameManager.Instance.UpdateFreeList ();

        //LevelPlayerManager.Instance.Init (PlayerData.Level);

        this.PostMissionEvent (MissionEnums.MissionId.ReachLevel);
    }

    public void SetMultiRewardCoins (int time)
    {
        if (PlayerData.TotalTimeMultiRewardCoins == 0)
        {
            PlayerData._LastTimeMultiRewardCoins = Helper.GetUtcTimeString ();
            PlayerData.SaveLastTimeMultiRewardCoins ();
        }

        PlayerData.TotalTimeMultiRewardCoins = Mathf.Clamp (PlayerData.TotalTimeMultiRewardCoins + time, 0, GameConfig.MaxTimeMultiRewardCoins);
        PlayerData.SaveTotalTimeMultiRewardCoins ();

        if (PlayerData.TotalTimeMultiRewardCoins > 0)
        {
            Contains.MultiRewardFromLuckyWheel = GameData.Instance.MultiRewardData.UpgradeValue + 1;
        }
        else
        {
            Contains.MultiRewardFromLuckyWheel = GameConfig.ValueEarnCoinsMultiTime;
        }

        this.PostActionEvent (ActionEnums.ActionID.RefreshUIEquipments, EquipmentEnums.GetKey (EquipmentEnums.AbilityId.MultiRewardCoins));
    }

    public void SetTextRedPackCash(float amount)
    {
        Monster.Data.GameData.RedPackCash += amount;
        this.PostActionEvent(ActionEnums.ActionID.RefreshUIRedPackCash);
    }

    #endregion

    #region FX

    public void FxDisplayGold (Vector3 position, string value)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxUIRaiseGold);

        if (fx == null)
            return;

        fx.GetComponent<FXCoin> ().Enable (position, value);
    }

    //public void InstanceFxDiamonds (Vector3 start_position, Vector3 end_position, int diamonds)
    //{
    //    var quantity   = 10;
    //    var balance    = diamonds % 10;
    //    var real_value = (diamonds - balance) / 10;

    //    if (diamonds > quantity && balance > 0)
    //    {
    //        PlayerData.Diamonds += balance;
    //    }
    //    else
    //    {
    //        if (diamonds <= quantity)
    //        {
    //            real_value = 1;
    //            quantity   = diamonds;
    //        }
    //    }

    //    for (int i = 0; i < quantity; i++)
    //    {
    //        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxItem_Diamond);

    //        if (fx == null)
    //            continue;

    //        fx.transform.position = new Vector3 (start_position.x + Random.Range (2f, -2f),
    //                                             start_position.y + Random.Range (2f, -2f));

    //        fx.transform.localScale = Vector3.zero;

    //        var tween = fx.DOScale (1, Durations.DurationScale).SetEase (Ease.OutBack).SetDelay (i * Durations.DurationScale / 3f);

    //        var index = i == quantity - 1;

    //        tween.OnComplete (() =>
    //        {
    //            var tween2 = fx.DOMove (end_position, Durations.DurationMovingBack).SetDelay (Durations.DurationScale).SetEase (Ease.InBack);

    //            tween2.OnComplete (() =>
    //            {
    //                PoolExtension.SetPool (PoolEnums.PoolId.FxItem_Diamond, fx);

    //                PlayerData.Diamonds += real_value;

    //                Instance.InstanceFxFireWork (fx.position);
    //                Instance.PlayAudioSound (AudioEnums.SoundId.Diamonds);
    //                Instance.PostActionEvent (ActionEnums.ActionID.RefreshUIDiamonds);


    //                if (index == true)
    //                {
    //                    PlayerData.SaveDiamonds ();
    //                }
    //            });
    //        }).SetEase (Ease.InBack);
    //    }
    //}

    //public void InstanceFxDiamonds (Vector3 startPosition, Vector3 endPosition, int quantity, System.Action OnCompleted = null)
    //{
    //    var max_quantity = Mathf.Clamp (quantity, 0, 10);

    //    for (int i = 0; i < max_quantity; i++)
    //    {
    //        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxItem_Diamond);

    //        if (fx == null)
    //            continue;

    //        fx.transform.position = new Vector3 (startPosition.x + Random.Range (2f, -2f),
    //                                             startPosition.y + Random.Range (2f, -2f));

    //        fx.transform.localScale = Vector3.zero;

    //        var tween = fx.DOScale (1, Durations.DurationScale).SetEase (Ease.OutBack).SetDelay (i * Durations.DurationScale / 3f);

    //        var index = i == max_quantity - 1;

    //        tween.OnComplete (() =>
    //        {
    //            var tween2 = fx.DOMove (endPosition, Durations.DurationMovingBack).SetDelay (Durations.DurationScale).SetEase (Ease.InBack);

    //            tween2.OnComplete (() =>
    //            {
    //                PoolExtension.SetPool (PoolEnums.PoolId.FxItem_Diamond, fx);

    //                Instance.InstanceFxFireWork (fx.position);

    //                Instance.PlayAudioSound (AudioEnums.SoundId.Diamonds);

    //                if (index == true)
    //                {
    //                    if (OnCompleted != null)
    //                    {
    //                        OnCompleted ();
    //                    }
    //                }
    //            });
    //        }).SetEase (Ease.InBack);
    //    }
    //}

    //public void InstanceFxCoins (Vector3 start_position, Vector3 end_position, double coins, int unit)
    //{
    //    var quantity = 6;

    //    if (coins < 6 && unit == 0)
    //    {
    //        quantity = (int) coins;
    //    }

    //    var real_coins = coins / quantity;
    //    var real_unit  = unit;

    //    Helper.FixNumber (ref real_coins, ref real_unit);

    //    for (int i = 0; i < quantity; i++)
    //    {
    //        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxItem_Coins);

    //        if (fx == null)
    //            continue;

    //        fx.transform.position = new Vector3 (start_position.x + Random.Range (2f, -2f),
    //                                             start_position.y + Random.Range (2f, -2f));

    //        fx.transform.localScale = Vector3.zero;

    //        var index = i == quantity - 1;

    //        var tween = fx.DOScale (1, Durations.DurationScale / 2f).SetEase (Ease.OutBack).SetDelay (i * Durations.DurationScale / 3f);

    //        tween.OnComplete (() =>
    //        {
    //            var tween2 = fx.DOMove (end_position, Durations.DurationMovingLine).SetDelay (Durations.DurationScale / 2f).SetEase (Ease.InBack);

    //            tween2.OnComplete (() =>
    //            {
    //                PoolExtension.SetPool (PoolEnums.PoolId.FxItem_Coins, fx);

    //                Helper.AddValue (ref PlayerData.Coins, ref PlayerData.CoinUnit, real_coins, real_unit);

    //                Instance.InstanceFxFireWork (fx.position);
    //                Instance.PostActionEvent (ActionEnums.ActionID.RefreshUICoins);
    //                Instance.PlayAudioSound (AudioEnums.SoundId.Coins);

    //                if (index == true)
    //                {
    //                    PlayerData.SaveCoins ();
    //                }
    //            });
    //        }).SetEase (Ease.InBack);
    //    }
    //}

    //public void InstanceFxCoins (Vector3 startPosition, Vector3 endPosition, int quantity, System.Action OnCompleted)
    //{
    //    var value = Mathf.Clamp (quantity, 0, 6);

    //    for (int i = 0; i < value; i++)
    //    {
    //        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxItem_Coins);

    //        if (fx == null)
    //            continue;

    //        fx.transform.position = new Vector3 (startPosition.x + Random.Range (2f, -2f),
    //                                             startPosition.y + Random.Range (2f, -2f));

    //        fx.transform.localScale = Vector3.zero;

    //        var tween = fx.DOScale (1, Durations.DurationScale / 2f).SetEase (Ease.OutBack).SetDelay (i * Durations.DurationScale / 3f);

    //        var index = i == value - 1;

    //        tween.OnComplete (() =>
    //        {
    //            var tween2 = fx.DOMove (endPosition, Durations.DurationMovingLine).SetDelay (Durations.DurationScale / 2f).SetEase (Ease.InBack);

    //            tween2.OnComplete (() =>
    //            {
    //                PoolExtension.SetPool (PoolEnums.PoolId.FxItem_Coins, fx);

    //                Instance.InstanceFxFireWork (fx.position);

    //                if (index == true)
    //                {
    //                    if (OnCompleted != null)
    //                    {
    //                        OnCompleted ();
    //                    }
    //                }

    //                Instance.PlayAudioSound (AudioEnums.SoundId.Coins);
    //            });
    //        }).SetEase (Ease.InBack);
    //    }
    //}

    public void InstanceFxFireWork (Vector3 position)
    {
        var fxFirework = PoolExtension.GetPool (PoolEnums.PoolId.FxExplode_Firework, false);

        if (fxFirework == null)
            return;

        fxFirework.transform.position = position;
        fxFirework.gameObject.SetActive (true);
    }

    public void InstanceFxTap (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxTapUpgrade, false);

        if (fx == null)
            return;

        fx.transform.position = position;
        fx.gameObject.SetActive (true);
    }

    public void InstanceFxTapFlower (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxTapFlower, false);

        if (fx == null)
            return;

        fx.transform.position = position;
        fx.gameObject.SetActive (true);
    }

    public void InstanceFxTapCoins (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxTapCoins, false);

        if (fx == null)
            return;

        fx.transform.position = position;
        fx.gameObject.SetActive (true);
    }

    public void InstanceFxTapBox (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxTapBox, false);

        if (fx == null)
            return;

        fx.transform.position = position;
        fx.gameObject.SetActive (true);
    }

    public void InstanceFxTapDiamonds (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxTapDiamonds, false);

        if (fx == null)
            return;

        fx.transform.position = position;
        fx.gameObject.SetActive (true);
    }

    public void InstanceFxLevelUpItems (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxLevelUpItem, false);

        if (fx == null)
            return;

        var script = fx.GetComponent<FxLevelUp> ();

        script.Init (position);

        fx.gameObject.SetActive (true);
    }

    public void InstanceFxLevelUp (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxLevelUp, false);

        if (fx == null)
            return;

        var script = fx.GetComponent<FxLevelUp> ();

        script.Init (position);

        fx.gameObject.SetActive (true);
    }

    public void InstanceBullets (int level, CarDataProperties _carDataProperties, int level_weapon, Vector3 position_start, string tag_enemy, Vector3 angle)
    {
        var data    = GameData.Instance.BulletsData.GetBullets (level);
        var bullets = PoolExtension.GetPool (data.BulletId, false);

        if (ReferenceEquals (bullets, null))
            return;

        var script = bullets.GetComponent<BulletsBehaviour> ();

        script.Init (position_start, angle, tag_enemy, level_weapon, _carDataProperties);

        script.Register ();

        bullets.gameObject.SetActive (true);
    }

    public void InstanceMuzzle (Vector3 position, Vector3 localEulerAngles)
    {
        var muzzle = PoolExtension.GetPool (PoolEnums.PoolId.Muzzle, false);

        if (ReferenceEquals (muzzle, null))
            return;

        muzzle.position         = position;
        muzzle.localEulerAngles = localEulerAngles;
        muzzle.gameObject.SetActive (true);
    }

    public void InstanceFxTextDamage (Vector3 position, string value)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxTextDamage, false);

        if (ReferenceEquals (fx, null))
            return;

        var script = fx.GetComponent<FxTextDamage> ();

        script.Init (position, value);

        fx.gameObject.SetActive (true);
    }

    public void InstanceFxCritTextDamage (Vector3 position, string value)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxTextCritDamage, false);

        if (ReferenceEquals (fx, null))
            return;

        var script = fx.GetComponent<FxTextDamage> ();

        script.Init (position, value);

        fx.gameObject.SetActive (true);
    }

    public void InstanceFxHitDamage (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxExplodeNormalBullet, false);

        if (ReferenceEquals (fx, null))
            return;

        fx.position = position;
        fx.gameObject.SetActive (true);
    }

    public void InstanceFxExplodeVirus (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxVirusExplode, false);

        if (ReferenceEquals (fx, null))
            return;

        fx.position = position;
        fx.gameObject.SetActive (true);
    }

    public void InstanceFxExplodeTrap (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxTrapExplode, false);

        if (ReferenceEquals (fx, null))
            return;

        fx.position = position;
        fx.gameObject.SetActive (true);
    }

    public void InstanceFxExplodeRocket (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxRocketExplode, false);

        if (ReferenceEquals (fx, null))
            return;

        fx.position = position;
        fx.gameObject.SetActive (true);
    }

    //public void CliamReward(ClaimRewardType itemType, double amount, int amount_unit,Vector3 start_position)
    //{
    //    start_position=Vector3.zero;
    //}
    public void CliamReward(ClaimRewardType itemType, ObscuredDouble amount, ObscuredInt amount_unit)//GameRewardProperty data
    {
        CliamReward(itemType, amount, amount_unit, Vector3.zero);
    }

    public void CliamReward(ClaimRewardType itemType, ObscuredDouble amount, ObscuredInt amount_unit, Vector3 start_position)//GameRewardProperty data
    {
        switch (itemType)
        {
            case ClaimRewardType.Diamond:
                //  GameActionManager.Instance.InstanceFxDiamonds(Vector3.zero, UIGameManager.Instance.GetPositionHubDiamonds(), (int)amount);
                this.PostActionEvent(ActionEnums.ActionID.RefreshUIDiamonds);

                break;
            case ClaimRewardType.Coin:
                double _amount = amount;
                int _unit = amount_unit;

                Helper.FixUnit(ref _amount, ref _unit);
                // GameActionManager.Instance.InstanceFxCoins(Vector3.zero, UIGameManager.Instance.GetPositionHubCoins(), _amount, _unit);
                this.PostActionEvent(ActionEnums.ActionID.RefreshUICoins);
                break;
            case ClaimRewardType.Car:

                for (int i = 0; i < amount; i++)
                {
                    //int level = PlayerData.LastLevelUnlocked;
                    //level++;
                    //GameManager.Instance.SetUnlockHighItem(level);
                    GameManager.Instance.SetBoxReward(amount_unit);
                }

;
                break;
            case ClaimRewardType.CarAmount:
                PlayerData.CarCurrectAmount += (ObscuredInt)amount.GetDecrypted();
                break;
            case ClaimRewardType.Item:
                // get item
                break;
            case ClaimRewardType.Upgrade:
                //  EquipmentEnums.AbilityId equipmen = UpEquipment;



                //int level_upgrade = PlayerData.GetEquipmentUpgrade(UpEquipment);
                //level_upgrade++;
                //PlayerData.SaveEquipmentUpgrade(UpEquipment, level_upgrade);
                this.PostActionEvent(ActionEnums.ActionID.RefreshEquipmentData);

                //   OnUpgradeCompleted(UpEquipment);
                break;

            case ClaimRewardType.Buff:



                break;

            case ClaimRewardType.Wheel:
                int _num = PlayerData._LastNumberTurnSpin;
                _num++;
                PlayerData._LastNumberTurnSpin = Mathf.Min(_num, 3);

                break;

            case ClaimRewardType.WheelDouble:
                // WheelRedPackManager.Instance.isDouble = true;

                break;

            case ClaimRewardType.RedPack:

                InstanceFxRedPack(start_position, UIGameManager.Instance.GetPositionHubRedPack(), amount, 0);
                break;

        }
    }

    public void InstanceFxRedPack(Vector3 start_position, Vector3 end_position, double amount, int unit)
    {
        Monster.Data.GameData.RedPackCash += (float)amount;
        if (amount > 1)
        {
            Helper.FixUnit(ref amount, ref unit);
        }

        var quantity = 1;
        if (amount > 1)
        {

            quantity = (int)Mathf.Round((float)amount / 3f);
            quantity = Mathf.Clamp(quantity, 1, 10);
        }

        for (int i = 0; i < quantity; i++)
        {
            var fx = PoolManager.Instance.PoolySpawn("FxItem_RedPack");//PoolExtension.GetPool(PoolEnums.PoolId.FxItemRedPack);

            if (fx == null)
                continue;

            fx.transform.position = new Vector3(start_position.x ,
                                                 start_position.y);

            fx.transform.localScale = Vector3.zero;

            var index = i == quantity - 1;

            var tween = fx.DOScale(1, Durations.DurationScale / 2f).SetEase(Ease.OutBack).SetDelay(i * Durations.DurationScale / 3f);

            tween.OnComplete(() =>
            {
                var tween2 = fx.DOMove(end_position, Durations.DurationMovingLine).SetDelay(Durations.DurationScale / 2f).SetEase(Ease.InBack);

                tween2.OnComplete(() =>
                {
                    PoolExtension.SetPool(PoolEnums.PoolId.FxItemRedPack, fx);

                    //Helper.AddValue(ref PlayerData.Coins, ref PlayerData.CoinUnit, real_coins, real_unit);

                    Instance.InstanceFxFireWork(fx.position);
                    Instance.PostActionEvent(ActionEnums.ActionID.RefreshUIRedPackCash);
                    Instance.PlayAudioSound(AudioEnums.SoundId.Coins);
                    //if (index)
                    //{
                    //    Monster.Data.GameData.RedPackCash++;
                    //}
                });
            }).SetEase(Ease.InBack);
        }
    }
    #endregion
}