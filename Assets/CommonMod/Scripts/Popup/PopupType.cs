using UnityEngine;
using System.Collections;

public enum PopupType
{
    Pause,
    Settings,
    Shop,
    Complete,
    Revival,
    DailyLogin,
    LevelReward,
    Gift,
    Over,
    Leaderboard,
    YesNo,
    Ok,
    MoreCoin,
    LastPopup,
    Customize
}

public enum PopupShowType
{
    DontShowIfOthersShowing,
    ReplaceCurrent,
    Stack,
    ShowPrevious,
    OverCurrent
}
