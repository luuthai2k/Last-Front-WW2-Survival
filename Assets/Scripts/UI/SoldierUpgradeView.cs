using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoldierUpgradeView : MonoBehaviour
{
    public GameObject goUpgradeBtn, goAdsUpgradeBtn,goMaxLevel;
    public TextMeshProUGUI levelTxt, pirceTxt;
    public GameObject[] curentBars, updateBars;
    int upgradePirce;
    public void SetUpUpgradeView(int level, int pirce)
    {
        if (DataController.Instance.Cash >= pirce)
        {
            goUpgradeBtn.SetActive(true);
            goAdsUpgradeBtn.SetActive(false);
        }
        else
        {
            goUpgradeBtn.SetActive(false);
            goAdsUpgradeBtn.SetActive(true);
        }
        this.upgradePirce = pirce;
        levelTxt.text = "Lv." + level.ToString();
        if (pirce == 0)
        {
           UpdateFreeTxt();
        }
        else
        {
            pirceTxt.text = ToolHelper.FormatLong2(pirce);
        }
      
        SetUpBar(level);
    }
    public async void UpdateFreeTxt()
    {
        pirceTxt.text = await LocalizationManager.Instance.GetLocalizedText("Free");
    }
  
    public void MaxLevel(int maxLevel=20)
    {
        goMaxLevel.SetActive(true);
        levelTxt.transform.GetChild(0).gameObject.SetActive(false);
        pirceTxt.gameObject.SetActive(false);
        levelTxt.text = "Lv." + maxLevel.ToString();
        for (int i = 0; i < 5; i++)
        {
           
                curentBars[i].SetActive(true);
                updateBars[i].SetActive(true);
          
        }
    }
    public void SetUpBar(int level)
    {
        int value = level % 5;
        for (int i = 0; i < 5; i++)
        {
            if (i < value)
            {
                curentBars[i].SetActive(true);
                updateBars[i].SetActive(false);
            }
            else if (i == value)
            {
                curentBars[i].SetActive(false);
                updateBars[i].SetActive(true);
            }
            else
            {
                curentBars[i].SetActive(false);
                updateBars[i].SetActive(false);
            }
        }
    }
    public int GetPirce()
    {
        
        return upgradePirce;
    }
}
