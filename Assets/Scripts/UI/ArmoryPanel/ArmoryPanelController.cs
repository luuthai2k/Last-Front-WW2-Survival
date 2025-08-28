using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEditor;
using UnityEngine.Purchasing;

public class ArmoryPanelController : MonoBehaviour
{
    public WeaponType currentTab;
    public WeaponViewPort[] weaponViewPorts;
    public WeaponUpgradeView weaponUpgradeView;
    public WeaponArea weaponArea;
    public WeaponTab[] weaponTabs;
    public GameObject[] weaponEffects;
    public StatBar damageBar;
    public StatBar magazineBar;
    public StatBar fireRateBar;
    public TextMeshProUGUI nameTxt,pirceBuyTxt,adsProcessTxt;
    public GameObject goEquipBtn,goBuyBtn,goAdsBtn,goOfferBtn,goPremiumBtn;
    public WeaponUIElement selectWeapon;
    public WeaponUpdateData updateData;
    private void OnEnable()
    {
        SetUp();
    }

    public void SetUp()
    {
        var levelData = DataController.Instance.GetCurrentLevelData();
        int index = (int)levelData.mainWeaponType;
        Debug.Log(index);
        currentTab = levelData.mainWeaponType;
        for (int i = 0; i < weaponViewPorts.Length; i++)
        {
            if (i == index)
            {
                weaponViewPorts[i].SetUp(OnSelectWeapon);
                weaponViewPorts[i].gameObject.SetActive(true);
                weaponTabs[i].SetActive(true);
            }
            else
            {
                weaponViewPorts[i].gameObject.SetActive(false);
                weaponTabs[i].SetActive(false);
            }
        }
        CheckNotification();
    }
    public void CheckNotification()
    {
      
        for(int i=0; i < weaponTabs.Length; i++)
        {
            weaponTabs[i].CheckNotification(i);
        }
    }
    public void OnBtnWeaponTabClick(int id)
    {
        currentTab = (WeaponType)id;
        for (int i = 0; i < weaponViewPorts.Length; i++)
        {
            if (i == id)
            {
                weaponViewPorts[i].SetUp(OnSelectWeapon);
                weaponViewPorts[i].gameObject.SetActive(true);
                weaponTabs[i].SetActive(true);
            }
            else
            {
                weaponViewPorts[i].gameObject.SetActive(false);
                weaponTabs[i].SetActive(false);
            }
        }
       
    }
    public void OnSelectWeapon(WeaponUIElement weaponUIElement)
    {
        if (selectWeapon != null)
        {
            selectWeapon.IsSelect(false);
        }
        selectWeapon = weaponUIElement;
        SetUpDisplay();

    }
    public void SetUpDisplay()
    {
        if (selectWeapon.weaponInGameData.unlockLevel > DataController.Instance.Level)
        {
            UIManager.Instance.SendNoti("missions_left", selectWeapon.weaponInGameData.unlockLevel - DataController.Instance.Level);
            return;
        }
         updateData = DataController.Instance.GetWeaponUpdateData(selectWeapon.weaponInGameData.weaponType, selectWeapon.indexInData);
        int level = selectWeapon.weaponInGameData.level;
        weaponArea.LoadWeapon(selectWeapon.weaponInGameData.weaponType, selectWeapon.weaponInGameData.ID);
        UpdateStatBar( level);
        HideAllBtn();
        SetUpBtn(level);
        LoadNameText();
    }
    async void LoadNameText()
    {
        nameTxt.text = await LocalizationManager.Instance.GetLocalizedText(selectWeapon.weaponInGameData.ID);
    }
    public void UpdateStatBar(int level)
    {
        if (level >= updateData.weaponLevelDatas.Length||! selectWeapon.isEquiped)
        {
            level--;
        }
        
        var wigd = weaponViewPorts[(int)currentTab].weaponEquipedUIElement.weaponInGameData;
        int currentDame = wigd.specification.damage ;
        int updateDame = updateData.weaponLevelDatas[level].weaponSpecifications.damage;
        int currentMegazine = wigd.specification.magazine;
        int updateMegazine = updateData.weaponLevelDatas[level].weaponSpecifications.magazine;
        int currentFireRate = wigd.specification.fireRate;
        int updateFireRate = updateData.weaponLevelDatas[level].weaponSpecifications.fireRate;
        damageBar.SetUp(currentDame, updateDame,GameConstain.MAX_DAMAGE);
        magazineBar.SetUp(currentMegazine, updateMegazine,GameConstain.MAX_MAGAZINE);
        fireRateBar.SetUp(currentFireRate, updateFireRate,GameConstain.MAX_FIRE_RATE);
    }
   
    public void SetUpBtn(int level)
    {
        Debug.Log("SetUpBtn");
        if (!selectWeapon.isOwned)
        {
            switch (selectWeapon.shopWeaponData.tag)
            {
                case BuyTag.Base:
                    goBuyBtn.SetActive(true);
                    pirceBuyTxt.text = selectWeapon.pirceTxt.text;
                    break;
                case BuyTag.Offer:
                    goOfferBtn.SetActive(true);
                    break;
                case BuyTag.Premium:
                    goPremiumBtn.SetActive(true);
                    break;
            }
            if (selectWeapon.shopWeaponData.numberAds > 0)
            {
                goAdsBtn.SetActive(true);
                adsProcessTxt.text = selectWeapon.GetProcessAds();
            }
        }
        else if (selectWeapon.isEquiped)
        {
            weaponUpgradeView.gameObject.SetActive(true);
            if (level >= updateData.weaponLevelDatas.Length)
            {
                weaponUpgradeView.MaxLevel();
            }
            else
            {
                int pirce = updateData.weaponLevelDatas[level].updatePirce;
                int cardsRequire = updateData.weaponLevelDatas[level].cardsRequired;
                int currentCards = selectWeapon.weaponInGameData.cards;
                if (currentCards >= cardsRequire)
                {

                    weaponUpgradeView.SetUpUpgradeView(level, pirce);
                }
                else
                {
                    weaponUpgradeView.SetUpCardView(level, pirce, cardsRequire, currentCards);
                }
            }
           
        }
        else
        {
            goEquipBtn.SetActive(true);
        }
    }
    public void FreeUpgradeWeaponTutorial()
    {
        weaponUpgradeView.SetUpUpgradeView(selectWeapon.weaponInGameData.level, 0);
    }
    public void OnBtnEquipedClick()
    {
        weaponViewPorts[(int)currentTab].weaponEquipedUIElement.IsEquiped(false);
        selectWeapon.IsEquiped(true);
        weaponViewPorts[(int)currentTab].weaponEquipedUIElement = selectWeapon;
        int level = selectWeapon.weaponInGameData.level;
        UpdateStatBar(level);
        HideAllBtn();
        SetUpBtn(level);
        AudioController.Instance.PlaySfx(GameConstain.WEAPON_SELECT);
    }
    public void OnBtnBuyClick()
    {
        int pirce = selectWeapon.shopWeaponData.pirce;
        if (DataController.Instance.Cash >= pirce)
        {
            DataController.Instance.AddCash(-pirce);
            TopBarController.Instance.UpdateText();
            selectWeapon.IsBuy();
            goBuyBtn.SetActive(false);
            goAdsBtn.SetActive(false);
            goEquipBtn.SetActive(true);
            FirebaseServiceController.Instance.LogEvent($"UNLOCK_{selectWeapon.weaponInGameData.ID}_CASH");
            var go=UIManager.Instance.SpawmWeapon(selectWeapon.weaponInGameData);
            //go.GetComponent<CallbackWhenClose>().SetActionAfterClose();
        }
        else
        {
            MainMenuUIManager.Instance.OnSelectStoreTab("Cash");
            return;
        }
      
    }
    public void OnBtnWatchAdsClick()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
           
            if (x)
            {
                if (selectWeapon.UpdateProcessAds())
                {
                    TopBarController.Instance.UpdateText();
                    goBuyBtn.SetActive(false);
                    goAdsBtn.SetActive(false);
                    goEquipBtn.SetActive(true);
                    FirebaseServiceController.Instance.LogEvent($"UNLOCK_{selectWeapon.weaponInGameData.ID}_ADS");
                }
                else
                {
                    adsProcessTxt.text = selectWeapon.GetProcessAds();
                }
            }
           
        });
    }
    public void OnBtnUpgradeClick()
    {
        int level = selectWeapon.weaponInGameData.level;
     
        if (DataController.Instance.Cash >= weaponUpgradeView.GetPirce())
        {
            PlayerPrefs.SetInt(GameConstain.UPDATE_WEAPON_TUT, 1);
            selectWeapon.Upgrade(updateData);
            DataController.Instance.AddCash(-weaponUpgradeView.GetPirce());
            TopBarController.Instance.UpdateText();
            if (level>= updateData.weaponLevelDatas.Length-1)
            {
                weaponUpgradeView.MaxLevel();
                UpdateStatBar(level);
            }
            else
            {
                UpdateStatBar(level + 1);
                SetUpBtn(level + 1);
            }
            AudioController.Instance.PlaySfx(GameConstain.WEAPON_SELECT);
            CheckNotification();
            weaponArea.PlayUpgradeFx();
        }
        else
        {
            MainMenuUIManager.Instance.OnSelectStoreTab("Cash");
            return;
        }
      

    }
    public void OnBtnAdsUpgradeClick()
    {
     
        ManagerAds.ins.ShowRewarded((x) =>
        {
            PlayerPrefs.SetInt(GameConstain.UPDATE_WEAPON_TUT, 1);
            int level = selectWeapon.weaponInGameData.level;
            selectWeapon.Upgrade(updateData);
            DataController.Instance.AddCash(0);
            if (level >= updateData.weaponLevelDatas.Length-1)
            {
                weaponUpgradeView.MaxLevel();
                UpdateStatBar(level);
            }
            else
            {
                UpdateStatBar(level + 1);
                SetUpBtn(level + 1);
            }
            AudioController.Instance.PlaySfx(GameConstain.WEAPON_SELECT);
            CheckNotification();
            weaponArea.PlayUpgradeFx();
        });


    }
    public void OnBtnShowPremiumClick()
    {
        MainMenuUIManager.Instance.OnSelectStoreTab("PremiumWeapons");
    }
    public void OnBtnShowOfferClick()
    {
        MainMenuUIManager.Instance.OnSelectMissionTab();
        _=UIManager.Instance.GetOfferPanel(selectWeapon.GetOfferID());
    }
    public void OnClickAdsFreeCrateBtn()
    {

        Debug.LogWarning("OnClickAdsBtn");
        ManagerAds.ins.ShowRewarded((x) =>
        {
            GetCrate();
            FirebaseServiceController.Instance.LogEvent($"REWARD_CHEST_FREE_UPGRADE");
        });


    }
    public void GetCrate()
    {
        MainMenuUIManager.Instance.gameObject.SetActive(false);
        gameObject.SetActive(false);
        GameObject go = UIManager.Instance.SpawnLootBox("SmallCrate");
        go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
        {
            MainMenuUIManager.Instance.gameObject.SetActive(true);
            gameObject.SetActive(true);
        });
    }
    public void HideAllBtn()
    {
        goEquipBtn.SetActive(false);
        goBuyBtn.SetActive(false);
        goOfferBtn.SetActive(false);
        goPremiumBtn.SetActive(false);
        weaponUpgradeView.gameObject.SetActive(false);
        goAdsBtn.SetActive(false);
    }
}
