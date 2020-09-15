﻿using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using MEC;
using Monster.Common;
using Monster.Core;
using Monster.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] [Header ("Config")] private Transform _TransformNode;

    [SerializeField] private Transform _TransformRewardItem,
                                       _TransformStartPosition,
                                       _TransformStartAlignPosition,
                                       _TransformIdlePosition;

    [SerializeField]
    [Header("CarItem")]
    private float CarItemSize=1f;

    [SerializeField] [Header ("Grid")] private int _MaxRow,
                                                   _MaxColumn;

    [SerializeField] [Header ("Cell")] private float _DistanceOffset;

    [SerializeField] private float _WidthOffset,
                                   _HeightOffset;

    [Header ("Data")] [SerializeField] private ItemNodeGroupData _ItemNodeGroupData;
    

    [SerializeField] private LevelData _LevelData;

    [Header ("Delete House")] 

    public Transform transform_delete_house;

    [SerializeField] private Animation animation_delete_house;

    [Header ("Animation Camera")] [SerializeField]
    private Animation animation_camera;

    [SerializeField] private AnimationClip animation_clip_shake;

    [Header ("Box Death")] [SerializeField]
    private TriggerCollider trigger_collider_box_death;

    [Header("Background")]
    [SerializeField]
    private Transform BackgroundTransform;
    #region Variables

    private Vector3 _DefaultPosition;
    private Vector3 _SetPosition;
    private Vector3 _DragPosition;
    private Vector3 _OffsetPosition;

    private Camera _MainCamera;

    private bool _IsDrag;
    private bool _IsBeginDrag;
    private bool _IsReady;
    private bool _IsRenderDrag;
    private bool _IsTouchReady;
    private bool _IsNodeInBusy;
    private bool _IsEnableOutline;
    private bool _IsInteractGame;

    private Vector2Int _PreviousIndex;
    private Vector2Int _LastIndex;
    private Vector2Int _FreeIndex;

    private NodeComponent[][] _ItemNode;
    private NodeComponent[][] _GridNode;
    private NodeComponent     _NodeComponentInDrag;
    private NodeComponent     _NodeComponentMerge;

    private List<int> _FreeIndexXColumn;
    private List<int> _FreeIndexYColumn;

    private INodeGrid[][] _INodeGrids;

    private Transform _TransformInDrag;

    private int _Row,
                _Column,
                _MaxNodeActive;

    private System.Action<object> HandleStateInteractGame;

    //private float   height_delete_house;
    //private float   width_delete_house;
    private Vector3 position_delete_house;

    private bool is_enable_delete_house;

    private float YPosition_Range_Camera;
    private float XPosition_Range_Camera;

    private Transform transform_delete;

    private string tag_enemy;

    private bool Is_game_over_loading;

    private bool MergeAnimation;
    #endregion

    #region Systems

    private void Start ()
    {
        InitConfig ();
        InitNodes ();
        InitGrids ();
        InitParameters ();

        InstanceBase ();

        InitFreeList ();
        LoadBase ();
        //LoadRevenueOffline ();

        LoadBackgroundMusic ();

        RegisterAction ();



        if (!this.IsTutorialCompleted (TutorialEnums.TutorialId.HowToPlayGame))
        {
            this.ExecuteTutorial (TutorialEnums.TutorialId.HowToPlayGame);
        }


        ApplicationManager.IsGameReady = true;

        // BackgroundTransform.
        
    }

    private void Update ()
    {

        if (!_IsReady || !_IsTouchReady)
        {
            if (Input.GetMouseButtonDown (0))
            {
                this.PostTapAnyWhere ();
            }

            return;
        }

        if (!_IsBeginDrag && Input.GetMouseButtonDown (0))//&& !Is_game_over_loading
        {
            // =============================== START DRAG ================================ //
          //  UIGameManager.Instance.TouchMainMenuUI();
            this.PostTapAnyWhere ();

            _SetPosition = ConvertScreenTouchToPosition ();

            _PreviousIndex.x = GetIndexXWithPositionX (_SetPosition.x);
            _PreviousIndex.y = GetIndexYWithPositionY (_SetPosition.y);

            //if (BonusManager.Instance.PostPositionBonusCoins (_SetPosition))
            //{
            //    return;
            //}

            _NodeComponentInDrag = GetNodeInGrid (_PreviousIndex.x, _PreviousIndex.y);

            LogGame.Log (string.Format ("[Game Manager] Get Input Touch Index {0} : {1}", _PreviousIndex.x.ToString (), _PreviousIndex.y.ToString ()));
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
#elif UNITY_ANDROID || UNITY_IPHONE
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
            {
                return;
            }
            if (ReferenceEquals (_NodeComponentInDrag, null))
            {
                return;
            }

            _IsBeginDrag  = true;
            _IsNodeInBusy = _NodeComponentInDrag.IsBusy ();

            _TransformInDrag = _NodeComponentInDrag.transform;
            _OffsetPosition  = _TransformInDrag.position;

            _OffsetPosition.x = _SetPosition.x - _OffsetPosition.x;
            _OffsetPosition.y = _SetPosition.y - _OffsetPosition.y;
            _OffsetPosition.z = 0;

            _NodeComponentInDrag.SetStatePause (true, true);

            if (_NodeComponentInDrag as BasePlaneComponent)
            {
                _NodeComponentInDrag.SetDragStatus(true);
                UIGameManager.Instance.SwitchBuyCarButton(_IsBeginDrag);
            }
           
            return;
        }

        if (_IsBeginDrag && Input.GetMouseButtonUp (0))
        {
            LogGame.Log (string.Format ("[Game Manager] Get Input Touch Index {0} : {1}", _LastIndex.x.ToString (), _LastIndex.y.ToString ()));

            // =============================== DRAG COMPLETED ================================ //


            _IsBeginDrag = false;
            if (_IsEnableOutline)
            {
                DisableOutlineSelected ();
            }

            _NodeComponentInDrag.SetStatePause (true);

            if (_IsDrag == false)
            {
                if (_IsNodeInBusy)
                {
                    _NodeComponentInDrag.TouchBusy ();
                }
                else
                {
                    _NodeComponentInDrag.SetDragStatus(false);
                    _NodeComponentInDrag.TouchHit ();
                }
                UIGameManager.Instance.SwitchBuyCarButton(false);
                Instance.PostCompletedTutorial (TutorialEnums.TutorialId.HowToPlayGame);

                return;
            }
            else if (_IsDrag && _IsNodeInBusy)
            {
                _IsDrag = false;

                return;
            }

            _IsDrag = false;

            _SetPosition = ConvertScreenTouchToPosition ();

            _SetPosition.x = _SetPosition.x - _OffsetPosition.x;
            _SetPosition.y = _SetPosition.y - _OffsetPosition.y;
            _SetPosition.z = _SetPosition.z - _OffsetPosition.z;

            _LastIndex.x = GetIndexXWithPositionX (_SetPosition.x);
            _LastIndex.y = GetIndexYWithPositionY (_SetPosition.y);

            _NodeComponentMerge = GetNodeInGrid (_LastIndex.x, _LastIndex.y);

            if (!ReferenceEquals (_NodeComponentMerge, null) && _NodeComponentMerge.IsBusy () == false && _IsInteractGame)
            {
                if (_NodeComponentMerge.GetLevel () < GameConfig.TotalItem && _NodeComponentMerge.GetLevel () == _NodeComponentInDrag.GetLevel () && _NodeComponentMerge.GetId () != _NodeComponentInDrag.GetId ())
                {
                    _NodeComponentInDrag.SetZoom(false);
                    _NodeComponentMerge.SetZoom(false);
                    if (!MergeAnimation)
                    {
                        MergeAnimation = true;
                        SetNodeInGrid(_NodeComponentInDrag.GetIndexX(), _NodeComponentInDrag.GetIndexY(), null);
                        SetFreeIndexGrid(_NodeComponentInDrag.GetIndexX(), _NodeComponentInDrag.GetIndexY());
                    }
                   

                    _IsReady = false;

                    FxMergeTwo (GetPositionWithIndex (_LastIndex.x, _LastIndex.y), _NodeComponentMerge.transform, _NodeComponentInDrag.transform, () =>
                    {
                        if (Instance == null) return;
                        
                        var level = Instance._NodeComponentMerge.GetLevel ();
                        var Exp   = Instance._NodeComponentMerge.GetExp ();

                        Instance._NodeComponentInDrag.ReturnToPool ();
                        Instance._NodeComponentInDrag.SetDisable ();

                        Instance._NodeComponentInDrag = null;

                        Instance._NodeComponentMerge.ReturnToPool ();
                        Instance._NodeComponentMerge.SetDisable ();

                        SetBaseItemGrid (
                            GameDataManager.Instance._CarNodeGroupData.GetProperties(level + 1),
                            Instance._LastIndex.x,
                            Instance._LastIndex.y,
                            () => { Instance._IsReady = true;
                                MergeAnimation = false;

                                GameActionManager.Instance.CliamReward(ClaimRewardType.RedPack,1,0, GetPositionWithIndex(Instance._LastIndex.x, Instance._LastIndex.y));
                            });

                        Instance.PlayAudioSound (AudioEnums.SoundId.MergeCompleted);

                        var position = GetPositionWithIndex (Instance._LastIndex.x, Instance._LastIndex.y);

                        FxStarsExp (position, UIGameManager.Instance.GetPositionHubExp (), () =>
                        {
                            if (GameActionManager.Instance != null)
                            {
                                GameActionManager.Instance.SetExp (Exp);
                            }

                            if (Instance != null) Instance.PlayAudioSound (AudioEnums.SoundId.Stars);
                        });

                        if (level + 1 > PlayerData.LastLevelUnlocked)
                        {
                            SetUnlockHighItem (level + 1);
                        }

                      

                        Instance.FXSunshine (position);

                        Instance.PostCompletedTutorial (TutorialEnums.TutorialId.HowToPlayGame);

                        GameActionManager.Instance.InstanceFxTapFlower (position);

                        if (Random.Range (0.00f, 1.00f) < 0.4f)
                        {
                            this.PlayAudioSound (AudioEnums.SoundId.ItemTouchTalk);
                        }

                        UIGameManager.Instance.RefreshRandomRewardCondition ();
                        UpdateCarMinLevel(GetBuyItemMinLevel());
                    });

                    SetDisableNodeIconItem (_PreviousIndex.x, _PreviousIndex.y);

                    this.PostMissionEvent (MissionEnums.MissionId.Merge);

                    DisableDeleteHouse ();

                    
                    UIGameManager.Instance.SwitchBuyCarButton(false);
                    return;
                }
                else if (_NodeComponentMerge.GetId () != _NodeComponentInDrag.GetId ())
                {
                    _IsReady = false;

                    SetNodeInGrid (_PreviousIndex.x, _PreviousIndex.y, _NodeComponentMerge);
                    SetUnFreeIndexGrid (_PreviousIndex.x, _PreviousIndex.y);

                    SetNodeInGrid (_LastIndex.x, _LastIndex.y, _NodeComponentInDrag);
                    SetUnFreeIndexGrid (_LastIndex.x, _LastIndex.y);

                    _NodeComponentInDrag
                       .SetIndex (_LastIndex.x,
                                  _LastIndex.y)
                       .SetPosition (Instance.GetPositionWithIndex (_LastIndex.x,
                                                                    _LastIndex.y));

                    FxMoveNode (GetPositionWithIndex (_PreviousIndex.x, _PreviousIndex.y),
                                Instance._NodeComponentMerge.transform, () =>
                                {
                                    if (Instance != null)
                                    {
                                        Instance._NodeComponentMerge
                                                .SetIndex (Instance._PreviousIndex.x,
                                                           Instance._PreviousIndex.y)
                                                .SetPosition (Instance.GetPositionWithIndex (Instance._PreviousIndex.x,
                                                                                             Instance._PreviousIndex.y));

                                        Instance.SetNodeInGrid (Instance._PreviousIndex.x,
                                                                Instance._PreviousIndex.y,
                                                                Instance._NodeComponentMerge);

                                        Instance._IsReady = true;
                                    }
                                });

                    SetDisableNodeIconItem (_PreviousIndex.x, _PreviousIndex.y);
                    SetDisableNodeIconItem (_LastIndex.x, _LastIndex.y);
                }
            }

            if (_NodeComponentInDrag != null)
            {
                if (is_enable_delete_house && IsDeleteBtnUp () && this.IsTutorialCompleted (TutorialEnums.TutorialId.HowToPlayGame))
                {
                    SetNodeInGrid (_NodeComponentInDrag.GetIndexX (), _NodeComponentInDrag.GetIndexY (), null);
                    SetFreeIndexGrid (_NodeComponentInDrag.GetIndexX (), _NodeComponentInDrag.GetIndexY ());

                    _NodeComponentInDrag.ReturnToPool ();

                    _NodeComponentInDrag.SetDisable ();

                    _NodeComponentInDrag = null;

                    SetDisableNodeIconItem (_PreviousIndex.x, _PreviousIndex.y);

                    GameActionManager.Instance.InstanceFxTapFlower (position_delete_house);

                    this.PlayAudioSound (AudioEnums.SoundId.Backward);

                    UIGameManager.Instance.RefreshRandomRewardCondition ();
                }
                else if (!IsOutOfMaxBase (_LastIndex.x, _LastIndex.y) && IsOutOfGrid (_LastIndex.x, _LastIndex.y) == false && !IsExistsNodeInGrid (_LastIndex.x, _LastIndex.y))
                {
                    SetNodeInGrid (_NodeComponentInDrag.GetIndexX (), _NodeComponentInDrag.GetIndexY (), null);
                    SetFreeIndexGrid (_NodeComponentInDrag.GetIndexX (), _NodeComponentInDrag.GetIndexY ());

                    _NodeComponentInDrag
                       .SetIndex (_LastIndex.x,
                                  _LastIndex.y)
                       .SetPosition (Instance.GetPositionWithIndex (_LastIndex.x,
                                                                    _LastIndex.y));

                    FxPutNode (_NodeComponentInDrag.transform, null);

                    SetNodeInGrid (_LastIndex.x, _LastIndex.y, _NodeComponentInDrag);
                    SetUnFreeIndexGrid (_LastIndex.x, _LastIndex.y);

                    SetDisableNodeIconItem (_PreviousIndex.x, _PreviousIndex.y);

                    this.PlayAudioSound (AudioEnums.SoundId.Backward);
                }
                else
                {
                    _IsReady = false;

                    SetDisableNodeIconItem (_PreviousIndex.x, _PreviousIndex.y);

                    FxMoveNode (GetPositionWithIndex (_NodeComponentInDrag.GetIndexX (), _NodeComponentInDrag.GetIndexY ()),
                                _NodeComponentInDrag.transform,
                                () =>
                                {
                                    Instance._IsReady = true;
                                    Instance._NodeComponentInDrag.SetPosition (Instance.GetPositionWithIndex (Instance._NodeComponentInDrag.GetIndexX (), Instance._NodeComponentInDrag.GetIndexY ()));
                                });

                    this.PlayAudioSound (AudioEnums.SoundId.Backward);
                }
            }

            _NodeComponentInDrag?.SetDragStatus(false);
            UIGameManager.Instance.SwitchBuyCarButton(false);

            DisableDeleteHouse ();

            // =============================== DOING COMPLETED DRAG ================================ //

            return;
        }

        if (!_IsBeginDrag) return;

        // =============================== DOING SOMETHING WHEN DRAG ================================ //

        _DragPosition   = ConvertScreenTouchToPosition ();
        _DragPosition.z = 0;

        if (_IsDrag == false)
        {
            if (Vector2.Distance (_SetPosition, _DragPosition) < 0.1f)
            {
                return;
            }

            _IsDrag = true;

            if (!ReferenceEquals (_NodeComponentInDrag, null) && !_NodeComponentInDrag.IsBusy ())
            {
                SetEnableNodeIconItem (_NodeComponentInDrag.GetLevel (), _PreviousIndex.x, _PreviousIndex.y, _NodeComponentInDrag.GetKey ());
            }

            if (_IsNodeInBusy)
            {
                return;
            }

            if (!ReferenceEquals (_NodeComponentInDrag, null))
            {
                EnableOutlineSelected (_NodeComponentInDrag.GetId (),
                                       _NodeComponentInDrag.GetLevel ());

                EnableDeleteHouse ();
            }
        }

        _IsRenderDrag = true;
    }

    private void LateUpdate ()
    {
        if (_IsReady)
        {
            if (_IsRenderDrag)
            {
                if (!_IsNodeInBusy)
                {
                    _SetPosition.x = _DragPosition.x - _OffsetPosition.x;
                    _SetPosition.y = _DragPosition.y - _OffsetPosition.y;
                    _SetPosition.z = _DragPosition.z - _OffsetPosition.z;

                    _TransformInDrag.position = _SetPosition;
                    _IsRenderDrag             = false;
                }
            }
        }

    }

    protected override void OnDestroy ()
    {
        UnRegisterAction ();

        PlayerData.SaveTotalTimeMultiRewardCoins ();

        base.OnDestroy ();
    }

    #endregion

    #region Controller

    private void CarGridAlignPosition()
    {
        _TransformStartPosition.position= new Vector3(_TransformStartPosition.position.x,_TransformStartAlignPosition.position.y+3.9f,0f);
    }

    private void RegisterAction ()
    {
        HandleStateInteractGame = param => Instance.OnInteractGame ((bool) param);

        this.RegisterActionEvent (ActionEnums.ActionID.SetStateInteractGame, HandleStateInteractGame);
        this.RegisterActionEvent (ActionEnums.ActionID.RefreshUpgradeItems, param => OnRefreshUpgradeItems ((int) param));
        this.RegisterActionEvent (ActionEnums.ActionID.RefreshEquipmentData, OnRefreshUpgradeEquipment);

        OnRefreshUpgradeEquipment (null);

        trigger_collider_box_death._OnTriggerEnter += TriggerColliderBoxDeathOnOnTriggerEnter;
    }

    private void UnRegisterAction ()
    {
        this.RemoveActionEvent (ActionEnums.ActionID.SetStateInteractGame, HandleStateInteractGame);
        this.RemoveActionEvent (ActionEnums.ActionID.RefreshUpgradeItems, param => OnRefreshUpgradeItems ((int) param));
        this.RemoveActionEvent (ActionEnums.ActionID.RefreshEquipmentData, OnRefreshUpgradeEquipment);

        trigger_collider_box_death._OnTriggerEnter -= TriggerColliderBoxDeathOnOnTriggerEnter;
    }

    private void LoadBase ()
    {
        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                if (IsOutOfMaxBase (i, j))
                    break;

                var level = PlayerData.LoadLevelItemStatic (i, j);

                if (level == -1)
                {
                    continue;
                }

                var node = GameDataManager.Instance._CarNodeGroupData.GetProperties(level);


                SetBaseItemGrid (node, i, j);
                SetUnFreeIndexGrid (i, j);
            }
        }
    }

    private void LoadRevenueOffline ()
    {
        if (OfflineManager.InstanceAwake () != null &&
            EarningManager.InstanceAwake () != null)
        {
            OfflineManager.Instance.EnableOfflineProfit (EarningManager.Instance.ProfitPerSec,
                                                         EarningManager.Instance.ProfitUnit);
        }
    }

    private void LoadBackgroundMusic ()
    {
        this.PlayAudioMusic (AudioEnums.MusicId.Background, true);
    }

    public void InstanceBase ()
    {
        _MaxNodeActive = _LevelData.GetMaxItemWithLevel (PlayerData.Level);

        //if (_MaxNodeActive < 4)
        //    _MaxNodeActive = 4;
        _MaxNodeActive = 15;

        _Column = _MaxColumn;
        _Row    = Mathf.Clamp ((_MaxNodeActive - _MaxNodeActive % _Column) / _Column, 2, _MaxRow);

        if (_MaxNodeActive - _Row * _Column > 0)
        {
            _Row = Mathf.Clamp (_Row + 1, 0, _MaxRow);
        }

        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                if (IsOutOfMaxBase (i, j))
                    break;

                _INodeGrids[i][j].Enable ();

                  var position = GetPositionWithIndex (i, j);


                if (_ItemNode[i][j] != null)
                {
                    _ItemNode[i][j].SetPosition (position);
                }

                _GridNode[i][j].SetPosition (position);

                if (j != 0)
                {
                    //transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    
                }
            }
        }
    }

    private void InitConfig ()
    {
        CarGridAlignPosition();
        _DefaultPosition = _TransformStartPosition.position;
        _MainCamera      = Camera.main;

        _IsReady        = true;
        _IsTouchReady   = true;
        _IsInteractGame = true;
    }

    private void InitParameters ()
    {
        //var size = transform_delete_house.size;

        //height_delete_house = size.y * transform_delete_house.transform.localScale.y;
        //width_delete_house  = size.x * transform_delete_house.transform.localScale.x;

        position_delete_house = transform_delete_house.transform.position;

        transform_delete = transform_delete_house.transform;

        YPosition_Range_Camera = _MainCamera.orthographicSize;
        XPosition_Range_Camera = _MainCamera.aspect * _MainCamera.orthographicSize;

        tag_enemy = TagEnums.GetKey (TagEnums.TagId.Enemy);

        Is_game_over_loading = false;
    }

    private void InitGrids ()
    {
        for (int i = 0; i < _MaxColumn; i++)
        {
            for (int j = 0; j < _MaxRow; j++)
            {
                var node      = Instantiate (_TransformNode.gameObject, j==0? _TransformStartPosition:_TransformIdlePosition);
                var component = node.GetComponent<NodeComponent> ();

                component
                   .Init (null)
                   .SetIndex (i, j)
                   .SetPosition (GetPositionWithIndex(i, j))
                   .SetEnable ();

                var iNodeGrid = node.GetComponent<INodeGrid> ();
               
                _INodeGrids[i][j] = iNodeGrid;
                _GridNode[i][j]   = component;

                iNodeGrid.DisableLevel ();
                iNodeGrid.DisableIconBack ();
                iNodeGrid.Disable ();
            }
        }
    }

    private void InitNodes ()
    {
        _ItemNode   = new NodeComponent[_MaxColumn][];
        _GridNode   = new NodeComponent[_MaxColumn][];
        _INodeGrids = new INodeGrid[_MaxColumn][];

        for (int i = 0; i < _MaxColumn; i++)
        {
            _ItemNode[i]   = new NodeComponent[_MaxRow];
            _GridNode[i]   = new NodeComponent[_MaxRow];
            _INodeGrids[i] = new INodeGrid[_MaxRow];
        }
    }

    private void InitFreeList ()
    {
        _FreeIndexXColumn = new List<int> ();
        _FreeIndexYColumn = new List<int> ();

        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                if (IsOutOfMaxBase (i, j))
                    break;

                if (ReferenceEquals (_ItemNode[i][j], null))
                {
                    _FreeIndexXColumn.Add (i);
                    _FreeIndexYColumn.Add (j);
                }
            }
        }
    }


   public void UpdateCarMinLevel(ObscuredInt minLevel)
    {
        foreach (var item_x in _ItemNode)
        {
            foreach (var item_y in item_x)
            {
                if (item_y!=null&& item_y.IsUnbox() && item_y.GetLevel()!=-1&&item_y.GetLevel()< minLevel)
                {
                    //SetNodeInGrid(item_y.GetIndexX(), item_y.GetIndexY(), null);
                    //SetFreeIndexGrid(item_y.GetIndexX(), item_y.GetIndexY());
                    item_y.ReturnToPool();
                    item_y.SetDisable();

                    var node = GameDataManager.Instance._CarNodeGroupData.GetProperties(minLevel);
                    SetBaseItemGrid(node, item_y.GetIndexX(), item_y.GetIndexY());
                    Instance.FXSunshine(GetPositionWithIndex(item_y.GetIndexX(), item_y.GetIndexY()));
                }
            }
        }
       
    }

    #endregion

    #region Action

    public void UpdateFreeList ()
    {
        _FreeIndexXColumn.Clear ();
        _FreeIndexYColumn.Clear ();

        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                if (IsOutOfMaxBase (i, j))
                    break;

                if (ReferenceEquals (_ItemNode[i][j], null))
                {
                    _FreeIndexXColumn.Add (i);
                    _FreeIndexYColumn.Add (j);
                }
            }
        }
    }

    public void SetUnlockHighItem (int level)
    {
        PlayerData.LastLevelUnlocked = level;
        PlayerData.SaveUnlockItemLevel ();

        //if (UnlockManager.Instance != null)
        //{
        //    UnlockManager.Instance.UnlockItemLevel (level);
        //}
        if (PlayerData.LastLevelUnlocked>=8)
        {
            SingletonMonobehaviour<UIViewManager>.Instance.RequestPopup(new UnlockCarViewRequest(PlayerData.LastLevelUnlocked, PlayerData.LastLevelUnlocked-4,Random.Range(4,9)));
        }


        UpdateCarMinLevel(GetBuyItemMinLevel());
        UIGameManager.Instance.SetBuyItem_Icon();

    }

    public void EnableOutlineSelected (int instanceId, int level)
    {
        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                if (IsOutOfMaxBase (i, j))
                    break;

                _INodeGrids[i][j].EnableOutline (instanceId, level);
            }
        }

        _IsEnableOutline = true;
    }

    public void DisableOutlineSelected ()
    {
        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                if (IsOutOfMaxBase (i, j))
                    break;

                _INodeGrids[i][j].DisableOutline ();
            }
        }

        _IsEnableOutline = false;
    }

    public void EnableTouch ()
    {
        _IsTouchReady = true;
    }

    public void DisableTouch ()
    {
        _IsTouchReady = false;
    }


    void SetRewardBox(BoxEnums.BoxId BoxType,int Level)
    {
        _FreeIndex = GetFreeIndexGrid();

        if (IsOutOfGrid(_FreeIndex.x, _FreeIndex.y))
        {
            return;
        }

        SetBaseItemGrid(GameDataManager.Instance._CarNodeGroupData.GetProperties(Level), _FreeIndex.x, _FreeIndex.y);
    }

    void CreateRewardBox(BoxEnums.BoxId BoxType, int Level)
    {
        var reward = PoolExtension.GetPool(PoolEnums.PoolId.BaseItemBox);
        var rewardComponent = reward.GetComponent<NodeComponent>();

        rewardComponent
            .SetBusy(false)
            .SetIndex(_FreeIndex.x, _FreeIndex.y)
            .SetPosition(GetPositionWithIndex(_FreeIndex.x, _FreeIndex.y))
            .Init(GameDataManager.Instance._CarNodeGroupData.GetProperties(Level))
            .SetEnable();

        rewardComponent.SetUnbox(false);

        reward.GetComponent<BoxBehaviour>()
            .SetIcon(BoxType);

        SetNodeInGrid(_FreeIndex.x, _FreeIndex.y, rewardComponent);


    }

    public void SetRandomReward ()
    {
        // =============================== RANDOM THE REWARD ================================ //
        SetRewardBox(BoxEnums.BoxId.RewardBox, ApplicationManager.Instance.GetRandomItemReward());
    }

    public void SetRandomTouchBoxReward ()
    {
        // =============================== Random the reward from the touch box ================================ //

         //SetRewardBox(BoxEnums.BoxId.RandomBox, ApplicationManager.Instance.GetRandomItemTouchBox());

         //_FreeIndex = GetFreeIndexGrid();

         //if (IsOutOfGrid(_FreeIndex.x, _FreeIndex.y))
         //{
         //    return;
         //}

         //if (_FreeIndex.y != 0 && Random.Range(0f, 1f) <= 0.55f)
         //{
         //    SetBoxRedPack(Random.Range(0.1f, 1.1f));
         //    return;
         //}
       //  CreateRewardBox(BoxEnums.BoxId.RandomBox, ApplicationManager.Instance.GetRandomItemTouchBox());

    }

    public void SetBoxReward (int item_level)
    {
        SetRewardBox(BoxEnums.BoxId.ShopBox, item_level);
    }

    public void SetBoxRedPack(float Amount)
    {
        var reward = PoolManager.Instance.PoolySpawn("[Box] RedPack");// PoolExtension.GetPool(PoolEnums.PoolId.RedPackBox);
        var rewardComponent = reward.GetComponent<NodeComponent>();

        rewardComponent
            .SetBusy(false)
            .SetIndex(_FreeIndex.x, _FreeIndex.y)
            .SetPosition(GetPositionWithIndex(_FreeIndex.x, _FreeIndex.y))
            .Init(null)
            .SetEnable();

        rewardComponent.SetUnbox(false);

        reward.GetComponent<BoxRedPack>()
            .SetAmount(Amount);

        SetNodeInGrid(_FreeIndex.x, _FreeIndex.y, rewardComponent);
    }

    //public void SetBaseItemGrid (ItemNodeData itemNodeData, int xColumn, int yRow, System.Action OnCompleted = null)
    //{
    //    var item          = PoolExtension.GetPool (itemNodeData.ItemPoolId);
    //    var itemComponent = item.GetComponent<NodeComponent> ();

    //    itemComponent
    //       .SetIndex (xColumn, yRow)
    //       .SetPosition (GetPositionWithIndex (xColumn, yRow))
    //       .SetBusy (false)
    //       .Init (itemNodeData)
    //       .SetEnable ();

    //    itemComponent.SetUnbox (true);

    //    SetNodeInGrid (xColumn, yRow, itemComponent);
    //    SetStateInGrid (xColumn, yRow, true);
    //    FxAppear (item, OnCompleted);
    //}

    public void SetBaseItemGrid(CarDataProperties itemNodeData, int xColumn, int yRow, System.Action OnCompleted = null)
    {
        var item = PoolManager.Instance.PoolySpawn(itemNodeData.PrefabName);
        var itemComponent = item.GetComponent<NodeComponent>();

        itemComponent
            .SetIndex(xColumn, yRow)
            .SetPosition(GetPositionWithIndex(xColumn, yRow))
            .SetBusy(false)
            .Init(itemNodeData)
            .SetEnable();
        
        

        itemComponent.SetUnbox(true);
       
        SetNodeInGrid(xColumn, yRow, itemComponent);
        SetStateInGrid(xColumn, yRow, true);
        FxAppear(item, OnCompleted);
        
        (itemComponent as BasePlaneComponent).SetPositionAnimation();
    }

    public void SetEnableNodeIconItem (int level, int xColumn, int yRow, string key)
    {
        if (IsOutOfGrid (xColumn, yRow))
            return;

        _INodeGrids[xColumn][yRow].EnableIconItem (level, key);
    }

    public void SetDisableNodeIconItem (int xColumn, int yRow)
    {
        if (IsOutOfGrid (xColumn, yRow))
            return;

        _INodeGrids[xColumn][yRow].DisableIconItem ();
    }

    public void SetEnableNodeIconBack (int xColumn, int yRow)
    {
        if (IsOutOfGrid (xColumn, yRow))
            return;

        _INodeGrids[xColumn][yRow].EnableIconBack ();
    }

    public void SetDisableNodeIconBack (int xColumn, int yRow)
    {
        if (IsOutOfGrid (xColumn, yRow))
            return;

        _INodeGrids[xColumn][yRow].DisableIconBack ();
    }

    public void SetItemBackToNode (int indexX, int indexY, BasePlaneMoving basePlaneMoving, NodeComponent nodeComponent)
    {
        var tf = basePlaneMoving.transform;

        _IsReady = false;

        tf.DOMove (GetPositionWithIndex (indexX, indexY), Durations.DurationMovingMerge).OnComplete (() =>
        {
            Instance.SetDisableNodeIconBack (indexX, indexY);
            Instance.SetDisableNodeIconItem (indexX, indexY);

            Instance._IsReady = true;

            nodeComponent.SetEnable ();
            nodeComponent.SetBusy (false);
            nodeComponent.SetPosition (Instance.GetPositionWithIndex (indexX, indexY));

            basePlaneMoving.ReturnToPools ();

            Instance.PlayAudioSound (AudioEnums.SoundId.PutItemsBack);
        });
    }

    public void ForceUnlockAllBoxes ()
    {
        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                var item = _ItemNode[i][j];

                if (!ReferenceEquals (item, null) && item.GetPoolId () == PoolEnums.PoolId.BaseItemBox)
                    item.TouchBusy ();
            }
        }
    }

    private void EnableDeleteHouse ()
    {
        animation_delete_house.Play ();

        is_enable_delete_house = true;
    }

    private void DisableDeleteHouse ()
    {
        animation_delete_house.Stop ();

        is_enable_delete_house = false;
    }

    public void EnableGameOver ()
    {
        if (Is_game_over_loading)
            return;

        Is_game_over_loading = true;

        Timing.RunCoroutine (Enumerator_Animation_EndGame ());
    }

    public void EnableNextGame ()
    {
        if (Is_game_over_loading)
            return;

        Is_game_over_loading = true;

        Timing.RunCoroutine (Enumerator_Animation_NextWave (true));
    }

    public void RefreshStateShooter (bool state)
    {
        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                var item = _ItemNode[i][j];

                if (!ReferenceEquals (item, null))
                    item.SetStatePause (state);
            }
        }
    }

    #endregion

    #region Enumerator

    private IEnumerator<float> Enumerator_Animation_EndGame ()
    {
        RefreshStateShooter (false);

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RemoveLevel ();
            EnemyManager.Instance.RefreshSateGame (false);
        }

        yield return Timing.WaitForOneFrame;



        this.PostActionEvent (ActionEnums.ActionID.RefreshUICoins);

        //if (UIGameManager.Instance != null)
        //    yield return Timing.WaitUntilDone (Timing.RunCoroutine (UIGameManager.Instance.EnableGameOver (string.Format (ApplicationLanguage.Text_description_wave_failed, PlayerData.LevelRound.ToString ()),
        //                                                                                                   string.Format (ApplicationLanguage.Text_description_wave_lost, 2, ApplicationManager.Instance.AppendFromCashUnit (total_coins_lost, PlayerData.CoinUnit)))));


        yield return Timing.WaitForSeconds(1.5f);

      //  yield return Timing.WaitForOneFrame;

        PlayerData.LevelRound = PlayerData.HighLevelRound - 1;

        if (PlayerData.LevelRound < 1)
            PlayerData.LevelRound = 1;
        
        PlayerData.SaveLevelRound ();

        yield return Timing.WaitUntilDone (Timing.RunCoroutine (Enumerator_Animation_NextWave (false)));

        Is_game_over_loading = false;
    }

    private IEnumerator<float> Enumerator_Animation_NextWave (bool _Success)
    {
        RefreshStateShooter (false);

        yield return Timing.WaitForOneFrame;

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RefreshSateGame (false);
        }

        //if (UIGameManager.Instance != null)
        //    yield return Timing.WaitUntilDone (Timing.RunCoroutine (UIGameManager.Instance.EnableNewWave (string.Format (ApplicationLanguage.Text_description_new_wave, PlayerData.LevelRound.ToString ()))));
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(UIGameManager.Instance.ShowGameLevelHub(_Success)));

        yield return Timing.WaitForOneFrame;

        EnemyManager.Instance.ReloadLevel (PlayerData.LevelRound);

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RefreshSateGame (true);
        }

        RefreshStateShooter (true);

        Is_game_over_loading = false;
    }

    #endregion

    #region Callback

    private void OnInteractGame (bool state)
    {
        _IsInteractGame = state;
    }

    private void OnRefreshUpgradeItems (int level)
    {
        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                var item = _ItemNode[i][j];

                if (item != null && item.GetLevel () == level)
                {
                    item.RefreshLevel ();
                }
            }
        }
    }

    public void OnRefreshUpgradeEquipment (object data)
    {
        Contains.EquipmentPercentIncreaseDamage     = GameData.Instance.GetEquipmentPercents (EquipmentEnums.AbilityId.IncreaseDamage) / 100;
        Contains.EquipmentPercentIncreaseCritAmount = GameData.Instance.GetEquipmentPercents (EquipmentEnums.AbilityId.IncreaseCritAmount) / 100;
        Contains.EquipmentPercentIncreaseCritRate   = GameData.Instance.GetEquipmentPercents (EquipmentEnums.AbilityId.IncreaseCritRate) / 100;
    }


    private void TriggerColliderBoxDeathOnOnTriggerEnter (Collider2D other)
    {
        if (!other.CompareTag (tag_enemy))
        {
            return;
        }

        var enemy_transform = other.transform;
        var script          = enemy_transform.GetComponent<EnemyBehaviour> ();

        script.AlterDestroy ();

        CameraManager.Instance.FxShakeCamera ();
        GameActionManager.Instance.InstanceFxExplodeTrap (enemy_transform.position);
        this.PlayAudioSound (AudioEnums.SoundId.FxTrapEnemyExplode);

        EnableGameOver ();
    }

    #endregion

    #region Helper

    public Vector3 ConvertScreenTouchToPosition ()
    {
        return _MainCamera.ScreenToWorldPoint (Input.mousePosition);
    }

    public Vector3 GetPositionWithIndex (int xColumn, int yRow)
    {

        float newWidthOffset = _WidthOffset;
        float newHeightOffset = _HeightOffset;
        if (yRow != 0)
        {
            newWidthOffset -= 0.25f;
            newHeightOffset -= 0.2f;
        }

        // =============================== GET POSITION WITH INDEX ================================ //

        _SetPosition.x = _DefaultPosition.x + (xColumn - (_Column - 1) / 2f) * (newWidthOffset + _DistanceOffset);
        _SetPosition.y = _DefaultPosition.y - (yRow - (_Row - 1) / 2f) * (newHeightOffset + _DistanceOffset);
        _SetPosition.z = 0;

        return _SetPosition;
    }

    public int GetIndexXWithPositionX (float xPosition)
    {
        var index = (xPosition + _WidthOffset / 2 - _DefaultPosition.x) / (_WidthOffset + _DistanceOffset) + (_Column - 1) / 2f;

        return index < 0 ? -1 : (int) index;
    }

    public int GetIndexYWithPositionY (float yPosition)
    {
        var index = (_DefaultPosition.y - yPosition + _HeightOffset) / (_HeightOffset + _DistanceOffset) + (_Row - 1) / 2f;

        return index < 0 ? -1 : (int) index;
    }

    public bool IsDeleteBtnUp ()
    {
        GameObject overUI = UIGameManager.Instance.TouchMainMenuUI();
        bool deleteCar = overUI != null && overUI.tag == "Delete";
        if (deleteCar)
        {
            return true;
        }
        return false;
    }

   

    public bool IsOutOfGrid (int xColumn, int yRow)
    {
        return xColumn < 0 || yRow < 0 || xColumn >= _Column || yRow >= _Row;
    }

    public bool IsExistsNodeInGrid (int xColumn, int yRow)
    {
        return _ItemNode[xColumn][yRow] != null;
    }

    public void SetNodeInGrid (int xColumn, int yRow, NodeComponent node)
    {
        _ItemNode[xColumn][yRow] = node;

        if (node != null)
        {
            _INodeGrids[xColumn][yRow].SetInfo (node.GetId (), node.GetLevel ());

            if (node.IsUnbox ())
            {
                _INodeGrids[xColumn][yRow].EnableLevel (node.GetLevel ());
            }
            else
            {
                _INodeGrids[xColumn][yRow].DisableLevel ();
            }

            PlayerData.SaveItemStatic (node.GetLevel (), xColumn, yRow);
        }
        else
        {
            _INodeGrids[xColumn][yRow].SetInfo (-1, -1);
            _INodeGrids[xColumn][yRow].DisableLevel ();
            PlayerData.SaveItemStatic (-1, xColumn, yRow);
        }
    }

    public void EnableLevelNode (int xColumn, int yRow, int level)
    {
        _INodeGrids[xColumn][yRow].EnableLevel (level);
    }

    public void DisableLevelNode (int xColumn, int yRow)
    {
        _INodeGrids[xColumn][yRow].DisableLevel ();
    }

    public void SetStateInGrid (int xColumn, int yRow, bool isActive)
    {
        if (IsOutOfGrid (xColumn, yRow))
            return;

        _INodeGrids[xColumn][yRow].SetState (isActive);
    }

    public NodeComponent GetNodeInGrid (int xColumn, int yRow)
    {
        return IsOutOfGrid (xColumn, yRow) ? null : _ItemNode[xColumn][yRow];
    }

    public Vector2Int GetFreeIndexGrid ()
    {
        _FreeIndex.x = -1;
        _FreeIndex.y = -1;

        if (_FreeIndexXColumn.Count == 0)
            return _FreeIndex;

        var index = Random.Range (0, _FreeIndexXColumn.Count);

        _FreeIndex.x = _FreeIndexXColumn[index];
        _FreeIndex.y = _FreeIndexYColumn[index];

        _FreeIndexXColumn.RemoveAt (index);
        _FreeIndexYColumn.RemoveAt (index);
       // Debug.Log("移除:" + _FreeIndexXColumn.Count);
        return _FreeIndex;
    }

    public void SetUnFreeIndexGrid (int xColumn, int yRow)
    {
        for (int i = 0; i < _FreeIndexXColumn.Count; i++)
        {
            if (_FreeIndexXColumn[i] == xColumn && _FreeIndexYColumn[i] == yRow)
            {
                _FreeIndexXColumn.RemoveAt (i);
                _FreeIndexYColumn.RemoveAt (i);
               // Debug.Log("移除:" + _FreeIndexXColumn.Count);
                break;
            }
        }
       
    }

    public void SetFreeIndexGrid (int xColumn, int yRow)
    {
        _FreeIndexXColumn.Add (xColumn);
        _FreeIndexYColumn.Add (yRow);
      //  Debug.Log("添加:" + _FreeIndexXColumn.Count);
    }

    public bool IsFreeIndexGrid ()
    {
        //Debug.Log("剩余:"+ _FreeIndexXColumn.Count);
        return _FreeIndexXColumn.Count > 0;
    }

    public bool IsOutOfMaxBase (int xColumn, int yRow)
    {
        return _Column * yRow + xColumn > _MaxNodeActive - 1;
    }

    public Vector3 GetPositionBusyItemNode ()
    {
        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                var item = _ItemNode[i][j];

                if (ReferenceEquals (item, null))
                    continue;

                if (!item.IsBusy ())
                    continue;

                return _GridNode[i][j].GetPosition ();
            }
        }

        return Vector.Vector3Null;
    }

    public Vector3 GetPositionFreeItemNode ()
    {
        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                var item = _ItemNode[i][j];

                if (ReferenceEquals (item, null))
                    continue;

                if (item.IsBusy ())
                    continue;

                return item.GetPosition ();
            }
        }

        return Vector.Vector3Null;
    }

    public List<Vector3> GetRandomFreeToUseItemNode ()
    {
        List<Vector3> Position = new List<Vector3> ();

        for (int i = 0; i < _Column; i++)
        {
            for (int j = 0; j < _Row; j++)
            {
                var item = _ItemNode[i][j];

                // =============================== Loop To Find ================================ //

                if (item == null)
                    continue;

                if (item.IsBusy ())
                    continue;

                for (int h = 0; h < _Column; h++)
                {
                    for (int k = 0; k < _Row; k++)
                    {
                        var item_b = _ItemNode[h][k];

                        if (ReferenceEquals (item_b, null))
                            continue;

                        if (item_b.IsBusy ())
                            continue;

                        if (item.GetId () == item_b.GetId ())
                            continue;

                        if (item.GetLevel () == item_b.GetLevel ())
                        {
                            Position.Add (item.GetPosition ());
                            Position.Add (item_b.GetPosition ());

                            return Position;
                        }
                    }
                }
            }
        }

        return Position;
    }

    public bool IsBaseActiveWeapon (int row)
    {
        return row == 0;
    }

    public bool IsOutSiteLeft (float xPosition)
    {
        return xPosition < -XPosition_Range_Camera;
    }

    public bool IsOutSiteRight (float xPosition)
    {
        return xPosition > XPosition_Range_Camera;
    }

    public bool IsOutSiteDown (float yPosition)
    {
        return yPosition < -YPosition_Range_Camera;
    }

    public bool IsActiveGetHit (float yPosition)
    {
        return yPosition < YPosition_Range_Camera;
    }


    public ObscuredInt GetBuyItemMinLevel()
    {
        //  Debug.Log("解锁："+ PlayerData.LastLevelUnlocked);
        ObscuredVector2Int _offset = GameDataManager.Instance._BuyCarRangeData.GetBuyRange(PlayerData.LastLevelUnlocked);
      //  ObscuredInt _maxLevel = Math.Max(1, PlayerData.LastLevelUnlocked - _offset.x);
        ObscuredInt _minLevel = Math.Max(1, PlayerData.LastLevelUnlocked - _offset.x - _offset.y+1);

        return _minLevel;
    }
    #endregion

    #region FX

    public void FxDisplayEarnCoin (double profit_earn, int profit_unit_earn, Vector3 position)
    {
        FxDisplayGold (position,
                       ApplicationManager.Instance.AppendFromUnit (profit_earn, profit_unit_earn));

       // Helper.AddValue (ref PlayerData.Coins, ref PlayerData.CoinUnit, profit_earn, profit_unit_earn);

        this.PostActionEvent (ActionEnums.ActionID.RefreshUICoins);
    }

    public void FxEarnCoin (double profit_earn, int profit_unit_earn, Vector3 position)
    {
        if (GameActionManager.Instance != null)
        {
            //GameActionManager.Instance.InstanceFxCoins (position,
            //                                            UIGameManager.Instance.GetPositionHubCoins (),
            //                                            profit_earn,
            //                                            profit_unit_earn);

            GameActionManager.Instance.FxDisplayGold (position,
                                                      string.Format ("+{0}", ApplicationManager.Instance.AppendFromUnit (profit_earn, profit_unit_earn)));
        }
        else
        {
          //  Helper.AddValue (ref PlayerData.Coins, ref PlayerData.CoinUnit, profit_earn, profit_unit_earn);
            PlayerData.SaveCoins ();

            this.PostActionEvent (ActionEnums.ActionID.RefreshUICoins);
        }
    }

    public void FxMoveNode (Vector3 position, Transform tf1, System.Action OnCompleted)
    {
        tf1.DOComplete (true);
        var tween = tf1.DOMove (position, Durations.DurationMoving);

        if (OnCompleted != null)
        {
            tween.OnComplete (() => OnCompleted ());
        }
    }

    public void FxPutNode (Transform tf1, System.Action OnCompleted)
    {
        tf1.DOComplete (true);
        tf1.localScale = new Vector3(CarItemSize + 0.2f, CarItemSize + 0.2f, CarItemSize + 0.2f);

        var tween = tf1.DOScale (CarItemSize, Durations.DurationTimeScale).SetEase (Ease.OutBack);

        if (OnCompleted != null)
        {
            tween.OnComplete (() => OnCompleted ());
        }
    }

    public void FxTapNode (Transform tf1, System.Action OnCompleted)
    {
        tf1.DOComplete (true);

        tf1.localScale = new Vector3 (CarItemSize + 0.2f, CarItemSize + 0.2f, CarItemSize + 0.2f);

        var tween = tf1.DOScale (CarItemSize, Durations.DurationTimeScale).SetEase (Ease.OutBack);

        if (OnCompleted != null)
        {
            tween.OnComplete (() => OnCompleted ());
        }
    }

    public void FxAppear (Transform tf, System.Action OnCompleted)
    {
        tf.DOComplete (true);

        tf.localScale = Vector3.zero;

        var tween = tf.DOScale (CarItemSize, Durations.DurationTimeScale).SetEase (Ease.OutBack);

        if (OnCompleted != null)
        {
            tween.OnComplete (() => OnCompleted ());
        }
    }

    public void FxDeAppear (Transform tf, System.Action OnCompleted)
    {
        tf.DOComplete (true);

        var tween = tf.DOScale (0, Durations.DurationTimeScale).SetEase (Ease.InBack);

        if (OnCompleted != null)
        {
            tween.OnComplete (() => OnCompleted ());
        }
    }

    public void FxMergeTwo (Vector3 position, Transform tf1, Transform tf2, System.Action OnCompleted)
    {
        tf1.DOComplete (true);
        tf2.DOComplete (true);

        tf1.localPosition = position;
        tf2.localPosition = position;

        var tween1 = tf1.DOLocalMoveX (position.x - 1f, Durations.DurationMovingMerge);
        var tween2 = tf2.DOLocalMoveX (position.x + 1f, Durations.DurationMovingMerge);

        tween1.OnComplete (() => { tf1.DOLocalMoveX (position.x, Durations.DurationMovingMerge); });
        tween2.OnComplete (() =>
        {
            var tween = tf2.DOLocalMoveX (position.x, Durations.DurationMovingMerge);

            if (OnCompleted != null)
            {
                tween.OnComplete (() => { OnCompleted (); });
            }
        });
    }

    public void FxDisplayGold (Vector3 position, string value)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxRaiseGold);

        if (fx == null)
            return;

        fx.GetComponent<FXCoin> ().Enable (position, value);
    }

    public void FXSunshine (Vector3 position)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxSunshine, false);

        if (fx == null)
            return;

        fx.position = position;

        fx.gameObject.SetActive (true);
    }

    public void FxStarsExp (Vector3 start_position, Vector3 end_position, System.Action OnCompleted)
    {
        var fx = PoolExtension.GetPool (PoolEnums.PoolId.FxStarExp, false);

        if (fx == null)
        {
            if (OnCompleted != null)
            {
                OnCompleted ();
            }

            return;
        }

        fx.DOComplete ();

        fx.position = start_position;
        fx.gameObject.SetActive (true);

        var tween = fx.DOMove (end_position, Durations.DurationMovingLine);

        tween.OnComplete (() =>
        {
            if (OnCompleted != null)
            {
                OnCompleted ();
            }

            PoolExtension.SetPool (PoolEnums.PoolId.FxStarExp, fx);

            GameActionManager.Instance.InstanceFxFireWork (end_position);
        });
    }

    public void FxShakeScale (Transform fx_transform)
    {
        fx_transform.DOComplete (fx_transform);
        fx_transform.DOPunchScale (Vector.Vector3PunchScale, Durations.DurationScale);
    }

    public void FxShareCamera ()
    {
        if (animation_camera.isPlaying)
            animation_camera.Stop ();

        animation_camera.Play (animation_clip_shake.name);
    }

    #endregion
}