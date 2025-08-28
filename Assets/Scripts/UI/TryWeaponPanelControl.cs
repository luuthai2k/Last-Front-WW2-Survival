using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TryWeaponPanelControl : MonoBehaviour
{
    public Image weaponIcon;
    private WeaponInGameData data;
    public GameObject goPremium, goOffer;
    public StatBar damageBar;
    public StatBar magazineBar;
    public StatBar fireRateBar;
    public TextMeshProUGUI weaponNameTxt;
    public void SetUp(Sprite icon, WeaponInGameData weaponInGameData, int tagId)
    {
        data = weaponInGameData;
        UpdateStatBar();
        weaponIcon.sprite = icon;
        LoadNameText();
        if (tagId == 1)
        {
            goOffer.SetActive(true);
        }
        else if (tagId == 2)
        {
            goPremium.SetActive(true);
        }
    }
    async void LoadNameText()
    {
        weaponNameTxt.text = await LocalizationManager.Instance.GetLocalizedText(data.ID);
    }
    public void UpdateStatBar()
    {
        var cwigd = DataController.Instance.GetWeaponIngameData(data.weaponType, DataController.Instance.gameData.GetWeaponSelectID(data.weaponType));
        int currentDame = cwigd.specification.damage;
        int updateDame = data.specification.damage;
        int currentMegazine = cwigd.specification.magazine;
        int updateMegazine = data.specification.magazine;
        int currentFireRate = cwigd.specification.fireRate;
        int updateFireRate = data.specification.fireRate;
        damageBar.SetUp(currentDame, updateDame, GameConstain.MAX_DAMAGE);
        magazineBar.SetUp(currentMegazine, updateMegazine, GameConstain.MAX_MAGAZINE);
        fireRateBar.SetUp(currentFireRate, updateFireRate, GameConstain.MAX_FIRE_RATE);
    }
    public void OnClickTryWeaponBtn()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
            GameManager.Instance.levelControl.StartWaveWithTryWeapon(data.ID);
            OnClosePopUp();
            FirebaseServiceController.Instance.LogEvent($"REWARD_TRYGUN_{DataController.Instance.Level}");
        });
       
    }
    public void OnClickSkipBtn()
    {
        GameManager.Instance.levelControl.StartWave();
        GameManager.Instance.DontTryWeapon();
        OnClosePopUp();
    }
    public void OnClosePopUp()
    {
       
        gameObject.SetActive(false);
    }

}
