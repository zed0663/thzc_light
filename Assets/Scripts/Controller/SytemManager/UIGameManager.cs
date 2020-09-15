using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using com.ootii.Messages;
using DG.Tweening;
using MEC;
using Monster.Core;
using Monster.Events;
using Monster.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIGameManager : Singleton<UIGameManager>
{
    public TopsUI topsUI;
    public BotUI botUI;

    [Header ("UI")]
    [SerializeField] private Transform transform_box;
    [SerializeField] private Transform transform_RedPack;



    [Header ("Fx")] [SerializeField] private Transform      _FxMissionAura;
    [SerializeField]                 private Transform      icon_new_mission;
    [SerializeField]                 private ParticleSystem FxMultiCoins;
    [SerializeField]                 private ParticleSystem FxSpeedUp;
    [SerializeField]                 private ParticleSystem FxX5Coins;

    [Header ("Lucky Wheel")] [SerializeField]
    private Transform transform_icon_lucky_wheel;

    [SerializeField] private Animation animation_control_icon_lucky_wheel;

    [SerializeField] private AnimationClip animation_clip_icon_lucky_wheel;

    [Header ("Button")] [SerializeField] private Transform transform_shop_button;
    [SerializeField] private Transform BuyCarButton, DeleteButton;

    [Header ("Notice")] [SerializeField] private Transform transform_notice_shop;
    [Header ("Items")] 
    [SerializeField]                    private Image item_icon;
    [Header("Game Panel")] [SerializeField]
    public GameLevelHub _GameLevelHub;
    [SerializeField]
    private Transform SceneTopTransform;


    #region Variables
    private double _CoinDisplay;
    private float _CurrentProcessTimeGetReward;
    private float _MaxTimeGetReward;
    private bool _IsReady;
    private bool _IsRunningAnimation;
    private bool _IsInteractReady;
    private int _LastDiamondUpdate;
    private Vector3 position_box;
    private bool is_max_time_touch;
    private double price;
    private int    price_unit;
    private double price_data;
    private int    price_unit_data;
    private float price_buy_coefficient;
    private double discount_prices;
    private int number_buy_times;
    private ObscuredInt level_buying;
    #endregion

    #region Handle
    private System.Action         HandleOnUpdateTime;
    private System.Action<object> HandleStateInteractUi;
    private System.Action<object> HandleStateMissionComplete;
    private System.Action<object> HandleSpeedUp;
    private System.Action<object> HandleMoreCash;
    private System.Action HandleUIRedPackCash;
    private Action TopUIGame_Wave;

    #endregion

    #region System

    private void Start ()
    {
        InitConfig ();
        InitParameter ();
        SetBuyItem_Icon(); 

        RegisterTimeUpdate ();


        UpdateTextLevel ();
        UpdateTextExp ();
        UpdateTextRedPackCash();

        RegisterAction ();
        OnUpdateCarAmount(null);

        OnMissionCompleted (null);
        this.PostActionEvent(ActionEnums.ActionID.TopUIGame_Wave);
    }

    protected override void OnDestroy ()
    {
        UnRegisterTimeUpdate ();
        UnRegisterAction ();

        base.OnDestroy ();
    }

    #endregion

    #region CallBack

    private void OnUpdateTime ()
    {
        OnUpdateCarProcess();
        if (is_max_time_touch)
            return;

        _CurrentProcessTimeGetReward -= Time.deltaTime;

        if (_CurrentProcessTimeGetReward < 0.1f)
        {
            _CurrentProcessTimeGetReward = 0;

            if (!is_max_time_touch)
            {
                GetRewardCraft ();
            }
        }
    }

    private ObscuredFloat CarTotalTime = 30f;
    private ObscuredFloat CarLeftTime = 0f;
    public void OnUpdateCarProcess()
    {
        if (PlayerData.CarCurrectAmount<PlayerData.CarTotalAmount)
        {
            CarLeftTime += Time.deltaTime;
            botUI.SetCarProcess(CarLeftTime/ CarTotalTime);
            if (CarLeftTime>= CarTotalTime)
            {
                PlayerData.CarCurrectAmount++;
                CarLeftTime = 0;

            }
        }
    }

    public void OnUpdateCarAmount(IMessage rMessage)
    {
        botUI.SetCarAmounText(PlayerData.CarCurrectAmount, PlayerData.CarTotalAmount);
    }

  
    public void OnRefreshInteractUi (bool state)
    {
        _IsInteractReady = state;
    }

    private void OnMissionCompleted (object param)
    {
        var isCompleted = MissionManager.Instance.IsHaveMissionCompleted ();

        if (_FxMissionAura != null && _FxMissionAura.gameObject.activeSelf != isCompleted)
        {
            _FxMissionAura.gameObject.SetActive (isCompleted);
        }

        if (icon_new_mission != null && icon_new_mission.gameObject.activeSelf != isCompleted)
        {
            icon_new_mission.gameObject.SetActive (isCompleted);
        }
    }

    private void OnSpeedUp (bool state)
    {
        SetStateFxSpeedUp (state);
    }

    private void OnMoreCash (bool state)
    {
        SetStateMoreCash (state);
    }

    #endregion

    #region Controller

    private void RegisterAction ()
    {

        HandleStateInteractUi      = param => Instance.OnRefreshInteractUi ((bool) param);
        HandleStateMissionComplete = OnMissionCompleted;
        HandleSpeedUp              = param => Instance.OnSpeedUp ((bool) param);
        HandleMoreCash             = param => Instance.OnMoreCash ((bool) param);

        HandleUIRedPackCash = UpdateTextRedPackCash;
        TopUIGame_Wave = delegate { topsUI.UpdateGameLevel_Wave(); };


        this.RegisterActionEvent (ActionEnums.ActionID.SetStateInteractUI, HandleStateInteractUi);
        this.RegisterActionEvent (ActionEnums.ActionID.RefreshUICompleteMission, HandleStateMissionComplete);
        this.RegisterActionEvent (ActionEnums.ActionID.SpeedUp, HandleSpeedUp);
        this.RegisterActionEvent (ActionEnums.ActionID.MoreCash, HandleMoreCash);
        this.RegisterActionEvent(ActionEnums.ActionID.TopUIGame_Wave, TopUIGame_Wave);
        this.RegisterActionEvent(ActionEnums.ActionID.RefreshUIRedPackCash, HandleUIRedPackCash);


        MessageDispatcher.AddListener("UpdateCarAmount", OnUpdateCarAmount);

    }


    private void UnRegisterAction ()
    {

        this.RemoveActionEvent (ActionEnums.ActionID.SetStateInteractUI, HandleStateInteractUi);
        this.RemoveActionEvent (ActionEnums.ActionID.RefreshUICompleteMission, HandleStateMissionComplete);
        this.RemoveActionEvent (ActionEnums.ActionID.SpeedUp, HandleSpeedUp);
        this.RemoveActionEvent (ActionEnums.ActionID.MoreCash, HandleMoreCash);
        this.RemoveActionEvent(ActionEnums.ActionID.TopUIGame_Wave, HandleStateInteractUi);
        this.RemoveActionEvent(ActionEnums.ActionID.RefreshUIRedPackCash, HandleUIRedPackCash);

        MessageDispatcher.RemoveListener("UpdateCarAmount", OnUpdateCarAmount);
    }

    #endregion

    #region Action

    private void InitConfig ()
    {
        _MaxTimeGetReward            = 12f;
        _CurrentProcessTimeGetReward = _MaxTimeGetReward;
    }

    private void InitParameter ()
    {
       // _CoinDisplay = PlayerData.Coins;

        _IsReady            = true;
        _IsInteractReady    = true;
        _IsRunningAnimation = false;

        position_box = botUI.BuyCarButtonPosition();

        level_buying = 0;
    }

    private void RegisterTimeUpdate ()
    {
        HandleOnUpdateTime = OnUpdateTime;
        

        if (TimeManager.InstanceAwake () != null)
            TimeManager.Instance.RegisterTimeUpdate (HandleOnUpdateTime);
    }

    private void UnRegisterTimeUpdate ()
    {
        if (TimeManager.InstanceAwake () != null)
            TimeManager.Instance.UnregisterTimeUpdate (HandleOnUpdateTime);
    }

    public void UpdateTextLevel ()
    {
        topsUI.UpdateTextLevel();
    }

    public void UpdateTextExp ()
    {
        topsUI.UpdateTextExp();
    }

    public void UpdateTextRedPackCash()
    {
        topsUI.UpdateTextRedPackCash();
    }

    public void EnableLuckyWheel ()
    {
        animation_control_icon_lucky_wheel.Play (animation_clip_icon_lucky_wheel.name);
    }

    public void DisableLuckyWheel ()
    {
        transform_icon_lucky_wheel.eulerAngles = Vector.Vector3Zero;
        animation_control_icon_lucky_wheel.Stop ();
    }


    public void SetStateMoreCash (bool state)
    {
        if (state)
        {
            FxMultiCoins.Play ();
        }
        else
        {
            FxMultiCoins.Stop ();
        }
    }

    public void SetStateFxXCoins (bool state)
    {
        if (state)
        {
            FxX5Coins.Play ();
        }
        else
        {
            FxX5Coins.Stop ();
        }
    }

    public void SetStateFxSpeedUp (bool state)
    {
        if (state)
        {
            FxSpeedUp.Play ();
        }
        else
        {
            FxSpeedUp.Stop ();
        }
    }

    

    public void SetStateNoticeShop (bool state)
    {
        if (transform_notice_shop.gameObject.activeSelf == !state)
        {
            transform_notice_shop.gameObject.SetActive (state);
        }
    }

    public void InitDataBuy (ObscuredInt level)
    {
        if (level == 0)
            level = 1;

        if (level != level_buying)
        {
            level_buying = level;

            var data = GameDataManager.Instance._CarNodeGroupData.GetDataItemWithLevel (level_buying);

            price_data      = data.Prices;
            price_unit_data = data.PricesUnit;

            price_buy_coefficient = data.PricesCoefficient;

            item_icon.sprite = GameDataManager.Instance._CarNodeGroupData.GetIcon (level_buying);
        }

        RefreshPriceBuy ();
        UpdatePriceBuy ();
    }

    public void UpdatePriceBuy ()
    {
      //  item_price_buy.text = ApplicationManager.Instance.AppendFromCashUnit (price, price_unit);
    }

    public void RefreshPriceBuy ()
    {
        var UpgradeData   = GameData.Instance.EquipmentData.GetData (EquipmentEnums.AbilityId.DiscountBuy);
        var level_upgrade = PlayerData.GetEquipmentUpgrade (EquipmentEnums.AbilityId.DiscountBuy);

        discount_prices = UpgradeData.UpgradeValue * level_upgrade / 100f;

        number_buy_times = PlayerData.GetNumberBuyItemProfitCoefficient (level_buying);

        if (level_upgrade == 0)
        {
            discount_prices = 0;
        }

        price = price_data - price_data * discount_prices;
        price = price * Math.Pow (price_buy_coefficient, number_buy_times);

        price_unit = price_unit_data;

        Helper.FixUnit (ref price, ref price_unit);
    }

    public void RefreshRandomRewardCondition ()
    {
        if (!is_max_time_touch)
            return;

        is_max_time_touch = GameManager.Instance.IsFreeIndexGrid () == false;
    }

    public IEnumerator<float> ShowGameLevelHub(bool _Success)
    {
        _GameLevelHub.SetCurrect(_Success);
        yield return Timing.WaitForSeconds(1.5f);
        _GameLevelHub.OnClose();
        yield return Timing.WaitForOneFrame;
    }

    public void SwitchBuyCarButton(bool isDrag=false)
    {
        if (!isDrag)
        {
            BuyCarButton.gameObject.SetActive(true);
            DeleteButton.gameObject.SetActive(false);
        }
        else
        {
            BuyCarButton.gameObject.SetActive(false);
            DeleteButton.gameObject.SetActive(true);
            DeleteButton.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).SetEase(Ease.OutBack).SetLoops(-1);
        }
    }


    #endregion

    #region Callbacks

    private void OnBuyCompleted ()
    {
        OnIncreaseNumberTimeBuy ();

        GameManager.Instance.SetBoxReward (level_buying);

        this.PostCompletedTutorial (TutorialEnums.TutorialId.HowToPlayGame);
        this.PostMissionEvent (MissionEnums.MissionId.TapOnBox);
        
    }

    private void OnIncreaseNumberTimeBuy ()
    {
        number_buy_times++;

        price = price * price_buy_coefficient;

        Helper.FixUnit (ref price, ref price_unit);

        PlayerData.SaveItemProfitCoefficient (level_buying, number_buy_times);

        UpdatePriceBuy ();
    }

    #endregion

    #region Emulator

    private IEnumerator<float> _AnimationLoadCoins ()
    {
        yield break;
    }
    #endregion

    #region Helper

    public Vector3 GetSceneTopTransform()
    {
        return SceneTopTransform.position;
    }

    public Vector3 GetPositionShopButton ()
    {
        return transform_shop_button.position;
    }

    public Vector3 GetPositionHubExp ()
    {
        return topsUI.GetPositionHubExp();
    }
    public Vector3 GetPositionTouchBox ()
    {
        return item_icon.transform.position;
    }
    public Vector3 GetPositionHubRedPack()
    {
        return transform_RedPack.position;
    }

    

    public void SetBuyItem_Icon()
    {
      //  Debug.Log("解锁："+ PlayerData.LastLevelUnlocked);
        ObscuredVector2Int _offset =GameDataManager.Instance._BuyCarRangeData.GetBuyRange(PlayerData.LastLevelUnlocked);
        ObscuredInt _maxLevel = Math.Max(1, PlayerData.LastLevelUnlocked - _offset.x);
        ObscuredInt _minLevel = Math.Max(1, PlayerData.LastLevelUnlocked - _offset.x - _offset.y);

        ObscuredInt _random = Random.Range(_maxLevel, _minLevel);

        InitDataBuy(_random);
    }

    public GameObject TouchMainMenuUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);


        //GraphicRaycaster component = MainMenuCanvas.GetComponent<GraphicRaycaster>();
        //List<RaycastResult> list = new List<RaycastResult>();
        //component.Raycast(pointerEventData, list);
        bool flag = raycastResults.Count != 0;
        GameObject result;
        if (flag)
        {
            result = raycastResults[0].gameObject;
        }
        else
        {
            result = null;
        }
        return result;
    }


    #endregion

    #region Fx

    private void FxShakeBox ()
    {
        transform_box.DOComplete ();
        transform_box.DOShakeScale (Durations.DurationShake, 0.2f);
    }



    #endregion

    #region Interface Interact

    public void InteractBuyItems ()
    {
        // =============================== Buy the cats ================================ //

        this.PlayAudioSound (AudioEnums.SoundId.TapOnCraft);

        GameActionManager.Instance.InstanceFxTapBox (position_box);

        FxShakeBox ();

        if (GameManager.Instance.IsFreeIndexGrid () == false)
        {
            ApplicationManager.Instance.AlertNoMorePark ();

            return;
        }


        if (PlayerData.CarCurrectAmount<=0)
        {
            //提示不足
           // ApplicationManager.Instance.AlertNotEnoughCurrency (CurrencyEnums.CurrencyId.Cash);
           botUI.ButtonFlash();

            return;
        }

        PlayerData.CarCurrectAmount--;
        OnBuyCompleted ();
       
        this.PostActionEvent (ActionEnums.ActionID.RefreshUICoins);
        SetBuyItem_Icon();
    }

    private bool GetRewardCraft ()
    {
        if (!GameManager.Instance.IsFreeIndexGrid ())
        {
            is_max_time_touch = true;

            return false;
        }

        GameManager.Instance.SetRandomTouchBoxReward ();
        _CurrentProcessTimeGetReward = _MaxTimeGetReward;

        is_max_time_touch = false;

        return true;
    }

    public void OpenShop ()
    {
        if (!_IsInteractReady)
            return;

        this.PlayAudioSound (AudioEnums.SoundId.TapOnButton);

        // =============================== OPEN THE SHOP ================================ //
        if (ShopManager.Instance != null) ShopManager.Instance.EnableHud ();

    }

    public void OpenKittyShop ()
    {
        if (!_IsInteractReady)
            return;

        this.PlayAudioSound (AudioEnums.SoundId.TapOnButton);

        // =============================== OPEN THE SHOP ================================ //
        if (ShopManager.Instance != null) ShopManager.Instance.EnableItemShop ();


        this.PostCompletedTutorial (TutorialEnums.TutorialId.HowToPlayGame);

        SetStateNoticeShop (false);
    }


    public void OpenWheelLucky ()
    {
        if (!_IsInteractReady)
            return;

        this.PlayAudioSound (AudioEnums.SoundId.TapOnButton);

        //if (ApplicationManager.Instance.AlertLevelRequire (GameConfig.UnlockLuckyWheelLevel))
        //{
        //    return;
        //}

        // =============================== OPEN WHEEL LUCKY ================================ //
        // if (WheelLuckyManager.Instance != null) WheelLuckyManager.Instance.EnableWheelLuckyManager ();

        SingletonMonobehaviour<UIViewManager>.Instance.RequestPopup(new WheelLuckyViewRequest(true));
    }

    public void OpenSettings ()
    {
        if (!_IsInteractReady)
            return;

        this.PlayAudioSound (AudioEnums.SoundId.TapOnButton);

        // =============================== OPEN SETTINGS ================================ //
        if (SettingManager.Instance != null) SettingManager.Instance.EnableHud ();

    }

    public void OpenMission ()
    {
        if (!_IsInteractReady)
            return;

        this.PlayAudioSound (AudioEnums.SoundId.TapOnButton);

        if (ApplicationManager.Instance.AlertLevelRequire (GameConfig.UnlockMissionLevel))
        {
            return;
        }

        // =============================== Open the mission hud ================================ //

        if (UIMissionManager.Instance != null) UIMissionManager.Instance.EnableHud ();

    }



    public void InteractLeaderBoard ()
    {
        if (!_IsInteractReady)
            return;

        this.PlayAudioSound (AudioEnums.SoundId.TapOnButton);
    }


    public void OpenDailyLogin()
    {
        SingletonMonobehaviour<UIViewManager>.Instance.RequestPopup(new SigninViewRequest(true));
    }

    public void OpenPhoneSignin()
    {
        SingletonMonobehaviour<UIViewManager>.Instance.RequestPopup(new PhoneSigninViewRequest(true));
    }

    public void OpenClaimRedPack(double _amount)
    {
        SingletonMonobehaviour<UIViewManager>.Instance.RequestPopup(new ClaimRedPackViewRequest(_amount));
    }

    public void OpenCashing_2()
    {
       SingletonMonobehaviour<UIViewManager>.Instance.RequestPopup(new CashingTwoViewRequest());
    }

    public void OpenCashing()
    {
        SingletonMonobehaviour<UIViewManager>.Instance.RequestPopup(new CashingViewRequest());
    }

    #endregion
}