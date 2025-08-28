using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;


public class WeaponUpgradeView : MonoBehaviour
{
    public GameObject goUpgradeBtn, goAdsUpgradeBtn, goCardView,goMaxLevel;
    public TextMeshProUGUI levelTxt, pirceTxt,cardsTxt;
    public GameObject[] curentBars, updateBars;
    public Image cardFillImg;
    int upgradePirce;
   
    public void SetUpCardView(int level, int pirce,int cardsRepuier,int currentCards)
    {
        goCardView.SetActive(true);
        goUpgradeBtn.SetActive(false);
        goAdsUpgradeBtn.SetActive(false);
        goMaxLevel.SetActive(false);
        levelTxt.text = "Lv." + level.ToString();
        upgradePirce = pirce;
        if (pirce == 0)
        {
            UpdateFreeTxt();
        }
        else
        {
            pirceTxt.text = ToolHelper.FormatLong2(pirce);
        }
        LoadCardTxt(currentCards, cardsRepuier);
        cardFillImg.fillAmount = (float)currentCards / cardsRepuier;
        SetUpBar(level);
    }
    public async void UpdateFreeTxt()
    {
        pirceTxt.text = await LocalizationManager.Instance.GetLocalizedText("Free");
    }
    public void SetUpUpgradeView(int level,int pirce)
    {
        goCardView.SetActive(false);
        goMaxLevel.SetActive(false);
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
        levelTxt.text = "Lv." + level.ToString();
        upgradePirce = pirce;
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
    async void LoadCardTxt(int currentCards, int cardsRequire)
    {
        string card = await LocalizationManager.Instance.GetLocalizedText("Cards");
        cardsTxt.text = $"{currentCards}/{cardsRequire} {card}";
    }

    public void MaxLevel(int maxLevel=20)
    {
        Debug.Log("MaxLevel");
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
        for(int i = 0; i < 5; i++)
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
