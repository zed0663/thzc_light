using System.Collections.Generic;
using DG.Tweening;
using MEC;
using UnityEngine.UI;
using UnityEngine;

public class GameLevelHub : MonoBehaviour
{
    public Transform _TransformHub;
    public Text[] levelTexts;
    public Transform[] levelTransform;
    Vector3[] levelVe3 = new Vector3[4];
    private float[] levelScale = new float[4];
    void Awake()
    {
        for (int i = 0; i < levelTransform.Length; i++)
        {
            levelVe3[i] = levelTransform[i].transform.localPosition;
            levelScale[i] = levelTransform[i].localScale.x;


        }
        //defaultPosition = levelScales[0].localPosition.x;
        //defaultSize = levelScales[0].localScale;
    }

    public void SetCurrect(bool isSwitch = true, float delaySwitch = 0f)
    {
        int currectLevel = PlayerData.LevelRound;
        if (isSwitch)
        {
            currectLevel--;
        }

        DefaultValue();

      //  Debug.Log("当前" + currectLevel);

        if (isSwitch)
        {

            if (currectLevel > 0)
            {
                if (currectLevel == 1)
                {
                    levelTexts[0].transform.parent.gameObject.SetActive(false);
                }


                for (int i = 0; i < levelTexts.Length; i++)
                {
                    levelTexts[i].text = (currectLevel + i - 1).ToString();//按顺序 应该再减一。因为 第二个框显示的应该是 胜利时的当前关卡，而不是胜利后的下一关，第三个框时新关卡
                }

            }
            else
            {
                //当传入==1时 ，四个方框的第一个方框应该不显示， 第二个框是1 第三个框是2 ，但是界面不能滑动
                isSwitch = false;
            }


        }

        if (!isSwitch)
        {

            if (currectLevel == 1)
            {
                levelTexts[0].transform.parent.gameObject.SetActive(false);
            }
            for (int i = 0; i < levelTexts.Length; i++)
            {
                levelTexts[i].text = (currectLevel + i - 1).ToString();//按顺序 应该再减一。因为 第二个框显示的应该是 胜利时的当前关卡，而不是胜利后的下一关，第三个框时新关卡
            }
        }

        OnOpen();
        if (isSwitch)
        {
            Timing.RunCoroutine(SwitchAnimation(delaySwitch));

        }
    }

    public void SetCurrect(int currectLevel, bool isSwitch = true, float delaySwitch = 0f)
    {
        DefaultValue();
        Debug.Log("当前" + currectLevel);
        levelTexts[0].transform.parent.gameObject.SetActive(true);
        if (isSwitch)
        {

            if (currectLevel > 1)
            {
                currectLevel -= 1;
                if (currectLevel == 1)
                {
                    levelTexts[0].transform.parent.gameObject.SetActive(false);
                }


                for (int i = 0; i < levelTexts.Length; i++)
                {
                    levelTexts[i].text = (currectLevel + i - 1).ToString();
                }

            }
            else
            {

                isSwitch = false;
            }


        }

        if (!isSwitch)
        {

            if (currectLevel == 1)
            {
                levelTexts[0].transform.parent.gameObject.SetActive(false);
            }
            for (int i = 0; i < levelTexts.Length; i++)
            {
                levelTexts[i].text = (currectLevel + i - 1).ToString();//按顺序 应该再减一。因为 第二个框显示的应该是 胜利时的当前关卡，而不是胜利后的下一关，第三个框时新关卡
            }
        }

        OnOpen();
        if (isSwitch)
        {
            Timing.RunCoroutine(SwitchAnimation(delaySwitch));

        }
    }

    private IEnumerator<float> SwitchAnimation(float delaySwitch)
    {
        yield return Timing.WaitForSeconds(delaySwitch);

        levelTransform[0].transform.DOLocalMoveX(levelVe3[0].x - 40f, 0.5f);
        levelTransform[0].transform.DOScale(0, 0.5f);

        levelTransform[1].transform.DOLocalMoveX(levelVe3[0].x, 0.5f);
        levelTransform[1].transform.DOScale(0.7f, 0.5f);

        levelTransform[2].transform.DOLocalMoveX(levelVe3[1].x, 0.5f);
        levelTransform[2].transform.DOScale(1f, 0.5f);

        levelTransform[3].transform.DOLocalMoveX(levelVe3[2].x, 0.5f);
        levelTransform[3].transform.DOScale(0.7f, 0.5f);


    }

    void DefaultValue()
    {
        OnClose();
        for (int i = 0; i < levelTransform.Length; i++)
        {
            levelTransform[i].transform.localPosition = levelVe3[i];
            levelTransform[i].localScale = new Vector3(levelScale[i], levelScale[i]);


        }

    }



    void OnOpen()
    {
        _TransformHub.gameObject.SetActive(true);
    }

    public void OnClose()
    {
        _TransformHub.gameObject.SetActive(false);
    }
}
