using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardPanelController : MonoBehaviour
{
    public DayReward[] dayRewards;
    public DayReward todayRewards;
    public int currentDay;
    public CanvasGroup buttonCanvasGroup;
    public Button claimBtn, claimX2Btn;
    void Start()
    {
        SetUp();
    }
    public void SetUp()
    {
        int lastday = PlayerPrefs.GetInt("LastDay", -1);
        DateTime yesterday = DateTime.Now.AddDays(-1);
        int yesterdayDayOfYear = yesterday.DayOfYear;
        if (lastday != yesterdayDayOfYear)
        {
            PlayerPrefs.SetInt("dayReward", 0);
        }
        PlayerPrefs.SetInt("LastDay", DateTime.Now.DayOfYear);
        currentDay = PlayerPrefs.GetInt("dayReward", 0) >= 7 ? 0 : PlayerPrefs.GetInt("dayReward", 0);
        for (int i = 0; i <= currentDay; i++)
        {
            if (i == currentDay)
            {
                dayRewards[i].IsToDay();
                todayRewards = dayRewards[i];
            }
            else
            {
                dayRewards[i].IsRevivedReward();
            }
        }
        currentDay++;
        PlayerPrefs.SetInt("dayReward", currentDay);
    }
  
    public void OnClickClaimBtn()
    {
        todayRewards.ClaimReward();
        claimBtn.interactable = false;
        claimX2Btn.interactable = false;
        buttonCanvasGroup.alpha = 0.5f;
        PlayerPrefs.SetInt(GameConstain.CLAIM_DAILY_REWARD, 1);
        gameObject.SetActive(false);
    }
    public void OnClickClaimX2Btn()
    {
       
        ManagerAds.ins.ShowRewarded((x) =>
        {
            if (x)
            {
                todayRewards.ClaimX2Reward();
                gameObject.SetActive(false);
                claimBtn.interactable = false;
                claimX2Btn.interactable = false;
                buttonCanvasGroup.alpha = 0.5f;
            }
           
        });
        PlayerPrefs.SetInt(GameConstain.CLAIM_DAILY_REWARD, 1);
    }
   
    
}
