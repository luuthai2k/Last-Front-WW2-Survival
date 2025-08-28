using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoldierPanelController : MonoBehaviour
{
    public SoldierUpgradeView soldierUpgradeView;
    public SoldierArea soldierArea;
    public StatBar healthBar;
    public StatBar handStabilityBar;
    public StatBar reloadSpeedBar;
    public SoldierUIElement[] soldierUIElements;
    public TextMeshProUGUI nameTxt;
    public GameObject goEquipBtn, goBuyBtn, goOfferBtn, goPremiumBtn;
    public SoldierUIElement soldierSelect, soldierEquiped;
    public SoldierUpdateData updateData;
    private void OnEnable()
    {
        SetUp();
    }
    public void SetUp()
    {
       
        for(int i=0;i< soldierUIElements.Length; i++)
        {
            soldierUIElements[i].SetUp(OnSelectSoldier);
            if (DataController.Instance.gameData.soldierEquipedID == soldierUIElements[i].indexInData) 
            {
                soldierEquiped = soldierUIElements[i];
                soldierEquiped.IsEquiped(true);
                soldierEquiped.IsSelect(true);
            }
        }
    }
    public void OnSelectSoldier(SoldierUIElement soldierUIElement)
    {
        if (soldierSelect != null)
        {
            soldierSelect.IsSelect(false);
        }
        soldierSelect = soldierUIElement;
        SetUpDisplay();
    }
    public void SetUpDisplay()
    {

        if (soldierSelect.soldierInGameData.unlockLevel > DataController.Instance.Level)
        {
            return;
        }
        updateData = DataController.Instance.GetSoldierUpdateData(soldierSelect.indexInData);
        int level = soldierSelect.soldierInGameData.level;
        soldierArea.LoadSoldier(soldierSelect.soldierInGameData.ID);
        UpdateStatBar(level);
        HideAllBtn();
        SetUpBtn(level);
        LoadNameText();
    }
    async void LoadNameText()
    {
        nameTxt.text = await LocalizationManager.Instance.GetLocalizedText(soldierSelect.soldierInGameData.ID);
    }
    public void UpdateStatBar(int level)
    {
        if (level >= updateData.soldierLevelDatas.Length||!soldierSelect.isEquiped)
        {
            level--;
        }
        var sigd = soldierEquiped.soldierInGameData;
        int currentHealth = sigd.soldierStat.health;
        int updateHealth = updateData.soldierLevelDatas[level].soldierStat.health;
        int currentHandStability = sigd.soldierStat.handStability;
        int updateHandStability = updateData.soldierLevelDatas[level].soldierStat.handStability;
        int currentReloadSpeed = sigd.soldierStat.reloadSpeed;
        int updateReloadSpeed = updateData.soldierLevelDatas[level].soldierStat.reloadSpeed;
        healthBar.SetUp(currentHealth, updateHealth, GameConstain.MAX_HEALTH);
        handStabilityBar.SetUp(currentHandStability, updateHandStability, GameConstain.MAX_HAND_STABILITY);
        reloadSpeedBar.SetUp(currentReloadSpeed, updateReloadSpeed, GameConstain.MAX_RELOAD_SPEED);
    }

    public void SetUpBtn(int level)
    {
        if (!soldierSelect.isOwned)
        {
            if (soldierSelect.goPirce.activeSelf)
            {
                goBuyBtn.SetActive(true);
            }
            else if (soldierSelect.goOffer.activeSelf)
            {
                goOfferBtn.SetActive(true);
            }
           
        }
        else if (soldierSelect.isEquiped)
        {
            soldierUpgradeView.gameObject.SetActive(true);
            if (level >= updateData.soldierLevelDatas.Length)
            {
                soldierUpgradeView.MaxLevel();
            }
            else
            {
                int pirce = updateData.soldierLevelDatas[level].updatePirce;
                soldierUpgradeView.SetUpUpgradeView(level, pirce);
            }
          
        }
        else
        {
            goEquipBtn.SetActive(true);
        }
    }
    public void FreeUpgradeSoldierTutorial()
    {
        soldierUpgradeView.SetUpUpgradeView(soldierSelect.soldierInGameData.level, 0);
    }
    public void OnBtnEquipedClick()
    {
        //weaponViewPorts[(int)currentTab].weaponEquipedUIElement.IsEquiped(false);
        //weaponViewPorts[(int)currentTab].weaponEquipedUIElement = selectWeapon;
        //SetUpDisplay();
    }
    public void OnBtnBuyClick()
    {
        //DataController.Instance.Cash -= weaponUpgradeView.GetPirce();
        //selectWeapon.IsBuy();
        //int level = selectWeapon.weaponInGameData.level;
        //HideAllBtn();
        //SetUpBtn(level);
    }
    public void OnBtnUpgradeClick()
    {
        int level = soldierSelect.soldierInGameData.level;
        if (DataController.Instance.Cash >= soldierUpgradeView.GetPirce())
        {
            TopBarController.Instance.UpdateText();
            soldierSelect.Upgrade(updateData);
            DataController.Instance.AddCash(-soldierUpgradeView.GetPirce());
            TopBarController.Instance.UpdateText();
            if (level >= updateData.soldierLevelDatas.Length-1)
            {
                soldierUpgradeView.MaxLevel();
                UpdateStatBar(level);
            }
            else
            {
                UpdateStatBar(level + 1);
                SetUpBtn(level + 1);
            }
          
            AudioController.Instance.PlaySfx(GameConstain.WEAPON_SELECT);

        }
        else
        {
            MainMenuUIManager.Instance.OnSelectStoreTab("Cash");
            return;
        }

    }
    public void OnBtnAdsUpgradeClick()
    {
        Debug.LogWarning("OnBtnAdsUpgradeClick");
        ManagerAds.ins.ShowRewarded((x) =>
        {
            int level = soldierSelect.soldierInGameData.level;
            soldierSelect.Upgrade(updateData);
            UpdateStatBar(level + 1);
            SetUpBtn(level + 1);
            AudioController.Instance.PlaySfx(GameConstain.WEAPON_SELECT);
            if (level >= updateData.soldierLevelDatas.Length-1)
            {
                soldierUpgradeView.MaxLevel();
                UpdateStatBar(level);
            }
            else
            {
                UpdateStatBar(level + 1);
                SetUpBtn(level + 1);
            }
            FirebaseServiceController.Instance.LogEvent($"REWARD_UPGRADE_PLAYER");
        });


    }
    public void HideAllBtn()
    {
        goEquipBtn.SetActive(false);
        goBuyBtn.SetActive(false);
        goOfferBtn.SetActive(false);
        goPremiumBtn.SetActive(false);
        soldierUpgradeView.gameObject.SetActive(false);
    }
}
