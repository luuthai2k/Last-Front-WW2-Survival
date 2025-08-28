using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using UnityEngine.Localization.Components;

public class SoldierUIElement : MonoBehaviour
{
    public SoldierInGameData soldierInGameData;
    public int indexInData;
    public bool isOwned, isEquiped;
    public Image iconImg;
    public UnityEvent<SoldierUIElement> onSelect;
    public GameObject goLock,goUnlock,goUpdate, goUpLevel, goMaxLevel, goPirce, goOffer, goSelect, goEquiped;
    public TextMeshProUGUI levelTxt, pirceTxt;
    public LocalizeStringEvent levelRewardLocalizeStringEvent;

    public void SetUp( UnityAction<SoldierUIElement> onClick)
    {
        soldierInGameData = DataController.Instance.GetSoldierIngameData(indexInData);
     
        if (soldierInGameData.unlockLevel > DataController.Instance.Level)
        {
            goLock.gameObject.SetActive(true);
            iconImg.color = Color.black;
            LoadLevelRequierText();
        }
        else if (soldierInGameData.isOwned)
        {
            goUpdate.SetActive(true);
            var updateData = DataController.Instance.GetSoldierUpdateData(indexInData);
            if (soldierInGameData.level >= updateData.soldierLevelDatas.Length)
            {
                goMaxLevel.SetActive(true);
                goUpLevel.SetActive(false);
            }
            else
            {
                goUpLevel.SetActive(true);
                goMaxLevel.SetActive(false);
            }
            levelTxt.text = soldierInGameData.level.ToString();
            transform.SetSiblingIndex(0);
        }
        else
        {
            goUnlock.SetActive(true);
        }
        isOwned = soldierInGameData.isOwned;
        this.onSelect.AddListener(onClick);
    }
    void LoadLevelRequierText()
    {
        int index = soldierInGameData.unlockLevel - DataController.Instance.Level;
        var localizedString = levelRewardLocalizeStringEvent.StringReference;
        localizedString.Arguments = new object[] { index };
        levelRewardLocalizeStringEvent.RefreshString();
    }
    public void OnClickSelect()
    {
        IsSelect(true);

    }
    public void IsSelect(bool isSelect)
    {
        goSelect.SetActive(isSelect);
        if (isSelect)
        {
            onSelect?.Invoke(this);
        }
    }

    public void IsEquiped(bool isEquiped)
    {
        goEquiped.SetActive(isEquiped);
        this.isEquiped = isEquiped;
    }
    public void IsBuy()
    {
        soldierInGameData.isOwned = true;
        levelTxt.text = soldierInGameData.level.ToString();
    }
    public void Upgrade(SoldierUpdateData updateData)
    {
        SoldierStat updateSoldierStat = updateData.soldierLevelDatas[soldierInGameData.level].soldierStat;
        soldierInGameData.soldierStat = updateSoldierStat;
        soldierInGameData.level++;
        levelTxt.text = soldierInGameData.level.ToString();
        if (soldierInGameData.level >= updateData.soldierLevelDatas.Length)
        {
            goMaxLevel.SetActive(true);
            goUpLevel.SetActive(false);
        }
        FirebaseServiceController.Instance.LogEvent($"UPGRADE_{soldierInGameData.ID}_{soldierInGameData.level}");

    }
}
