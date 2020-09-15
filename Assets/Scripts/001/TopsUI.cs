using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TopsUI : MonoBehaviour
{

    [Header("Player Level Hub")]
    [SerializeField] private Text PlayerLevel;
    [SerializeField] private Image PlayerLevelProcess; 
    [SerializeField] private Text PlayerTextExp;

    [Header("Red Pack Cash")]
    [SerializeField] private Text RedPackCashValue;

    [Header("Game Level Hub")]
    [SerializeField] private Text GameLevel;


    public void UpdateTextRedPackCash()
    {
        RedPackCashValue.text = Monster.Data.GameData.RedPackCash.ToString()+" 元";
    }

    public void UpdateTextExp()
    {
        if (Contains.ExpNeedReach > 0)
        {
            if (PlayerLevelProcess != null)
            {
                PlayerLevelProcess.DOComplete();
                PlayerLevelProcess.DOFillAmount((float)PlayerData.Exp / Contains.ExpNeedReach, Durations.DurationFillAmount);
            }

            PlayerTextExp.text = string.Format("{0}/{1}", PlayerData.Exp.ToString(), Contains.ExpNeedReach.ToString());
        }
        else
        {
            if (PlayerLevelProcess != null) PlayerLevelProcess.fillAmount = 1;

            PlayerTextExp.text = "MAX";
        }
    }

    public void UpdateTextLevel()
    {
        if (PlayerLevel != null) PlayerLevel.text = PlayerData.Level.ToString();
    }
    public Vector3 GetPositionHubExp()
    {
        return PlayerLevel.transform.position;
    }

    public void UpdateGameLevel_Wave()
    {
        if (GameLevel != null)
            GameLevel.text =
                string.Format("关卡.{0}-{1}", PlayerData.LevelRound, EnemyManager.Instance.CurrectLevel_Wave);
    }

}
