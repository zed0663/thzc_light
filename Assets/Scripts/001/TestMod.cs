using System.Collections;
using Monster.Common;
using TMPro;
using UnityEngine;


public class TestMod : MonoBehaviour
{
    public TMP_InputField _InputLevel;
    public TMP_InputField _InputField;
    public TMP_InputField _InputCarAmount;

    public void SetLevel()
    {
        int _level = int.Parse(_InputLevel.text);
       
        {
            GameManager.Instance. RefreshStateShooter(false);

            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.RemoveLevel();
                EnemyManager.Instance.RefreshSateGame(false);
            }

            PlayerData.LevelRound = _level;
            GameManager.Instance.EnableNextGame();
        }
    }

    public void BuyCar()
    {
        int _level = int.Parse(_InputField.text);
        if (_level<=81)
        {
            GameActionManager.Instance.CliamReward(ClaimRewardType.Car,1, _level);
        }
    }

    public void SetCarAmount()
    {
        int amount = int.Parse(_InputCarAmount.text);
        GameActionManager.Instance.CliamReward(ClaimRewardType.CarAmount, amount, 0);
    }
}
