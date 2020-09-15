using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using UnityEngine.UI;

public class CashingButton : MonoBehaviour
{
    [SerializeField] private ObscuredInt Index;
    public GameObject ToggleOff;
    public GameObject ToggleOn;
    public GameObject ToggleInClaim;
    public bool CommonBtn;
    private bool InClaim;

    public void SetButtonStatus(bool inClaim)
    {
        InClaim = inClaim;
        if (ToggleInClaim!=null)
        {
            ToggleInClaim.gameObject.SetActive(InClaim);
        }
       
        ToggleOff.gameObject.SetActive(false);
        ToggleOn.gameObject.SetActive(false);
    }

    public void SwitchButtonStatus(bool status)
    {
        if (!InClaim)
        {
            ToggleOff.gameObject.SetActive(!status);
            ToggleOn.gameObject.SetActive(status);
        }
    }
    public int GetIndex()
    {
        return Index;
    }
    public bool GetClaimStatus()
    {
        if (CommonBtn)
        {
            return false;
        }
        return InClaim;
    }

}
