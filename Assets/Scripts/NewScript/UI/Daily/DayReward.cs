using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayReward : MonoBehaviour
{
    public Pack rewardPacks;
    public GameObject goToday;
    public GameObject goRevived;

    public void ClaimReward()
    {
        UIManager.Instance.GetPackReward(rewardPacks);
        IsRevivedReward();
    }
    public void ClaimX2Reward()
    {
        UIManager.Instance.GetPackReward(rewardPacks,2);
        IsRevivedReward();
    }
    public void IsToDay()
    {

        goToday.SetActive(true);
    }
    public void IsRevivedReward()
    {
        goRevived.SetActive(true);
        goToday.SetActive(false);
    }

}
