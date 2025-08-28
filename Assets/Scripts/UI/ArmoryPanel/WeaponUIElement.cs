using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using UnityEngine.Localization.Components;
using UnityEngine.Purchasing;

public class WeaponUIElement : MonoBehaviour
{
    public WeaponInGameData weaponInGameData;
    public int indexInData;
    public Image iconImg,cardFillImg;
    public bool isOwned,isEquiped;
    public UnityEvent<WeaponUIElement> onSelect;
    public GameObject goLock, goOwned, goUnlock,goPirce,goOffer,goPremium,goSelect,goEquiped,goCards, goUpdate,goMaxLevel;
    public TextMeshProUGUI  levelTxt, pirceTxt,cardsTxt;
    public LocalizeStringEvent levelRewardLocalizeStringEvent;
    public ShopWeaponData shopWeaponData;
    string offerID;
    public void SetUp(WeaponType type,UnityAction<WeaponUIElement> onClick)
    {
        weaponInGameData = DataController.Instance.GetWeaponIngameData(type, indexInData);
        if (weaponInGameData.unlockLevel > DataController.Instance.Level)
        {
            goLock.gameObject.SetActive(true);
            LoadLevelRequierText();
            iconImg.color = Color.black;
        }
        else if (weaponInGameData.isOwned)
        {

            goOwned.SetActive(true);
            goUnlock.SetActive(false);
            levelTxt.text = weaponInGameData.level.ToString();
            transform.SetSiblingIndex(0);
            var updateData = DataController.Instance.GetWeaponUpdateData(weaponInGameData.weaponType,indexInData);
           
            if(weaponInGameData.level>= updateData.weaponLevelDatas.Length)
            {
                goMaxLevel.gameObject.SetActive(true);
                goUpdate.gameObject.SetActive(false);
                goCards.gameObject.SetActive(false);
            }
            else
            {
                int cardsRequire = updateData.weaponLevelDatas[weaponInGameData.level].cardsRequired;
                int currentCards = weaponInGameData.cards;
                goMaxLevel.gameObject.SetActive(false);
                if (currentCards >= cardsRequire)
                {

                    goUpdate.gameObject.SetActive(true);
                    goCards.gameObject.SetActive(false);
                }
                else
                {
                    goUpdate.gameObject.SetActive(false);
                    goCards.gameObject.SetActive(true);
                    cardFillImg.fillAmount = (float)currentCards / cardsRequire;
                    LoadCardTxt(cardsRequire);

                }
            }
           
        }
        else
        {
            goUnlock.SetActive(true);
            shopWeaponData = DataController.Instance.GetShopWeaponData(weaponInGameData.weaponType, indexInData);
            switch (shopWeaponData.tag) 
            {
                case BuyTag.Base:
                    goPirce.SetActive(true);
                    if (shopWeaponData.pirce == 0)
                    {
                        UpdateFreeTxt();
                    }
                    else
                    {
                        pirceTxt.text = ToolHelper.FormatLong2(shopWeaponData.pirce);
                    }
                    break;
                case BuyTag.Offer:
                    goOffer.SetActive(true);
                    offerID = shopWeaponData.packID;
                    break;
                case BuyTag.Premium:
                    goPremium.SetActive(true);
                    break;
            }
        }
        isOwned = weaponInGameData.isOwned;
        if (onClick != null)
        {
            this.onSelect.AddListener(onClick);
        }
      
    }
    public async void UpdateFreeTxt()
    {
        pirceTxt.text = await LocalizationManager.Instance.GetLocalizedText("Free");
    }
    async void LoadCardTxt(int cardsRequire)
    {
        string card = await LocalizationManager.Instance.GetLocalizedText("Cards");
        cardsTxt.text = $"{weaponInGameData.cards}/{cardsRequire} {card}";
    }
   
    void LoadLevelRequierText()
    {
        int index = weaponInGameData.unlockLevel - DataController.Instance.Level;
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
        DataController.Instance.gameData.SetWeaponSelectID(weaponInGameData.weaponType, indexInData);
    }
    public void IsBuy()
    {
        weaponInGameData.isOwned = true;
        goOwned.SetActive(true);
        goUnlock.SetActive(false);
        levelTxt.text = weaponInGameData.level.ToString();
        isOwned = true;
    }
    public void Upgrade(WeaponUpdateData updateData)
    {
       int cardReqiure= updateData.weaponLevelDatas[weaponInGameData.level].cardsRequired;
        int updateDame = updateData.weaponLevelDatas[weaponInGameData.level].weaponSpecifications.damage;
        int updateMegazine = updateData.weaponLevelDatas[weaponInGameData.level].weaponSpecifications.magazine;
        int updateFireRate = updateData.weaponLevelDatas[weaponInGameData.level].weaponSpecifications.fireRate;
        weaponInGameData.AddCard(-cardReqiure);
        weaponInGameData.specification = new WeaponSpecifications(updateDame, updateMegazine, updateFireRate);
        weaponInGameData.level++;
        levelTxt.text = weaponInGameData.level.ToString();
        cardReqiure = updateData.weaponLevelDatas[weaponInGameData.level].cardsRequired;
        int currentCards = weaponInGameData.cards;
        if (weaponInGameData.level >= updateData.weaponLevelDatas.Length)
        {
            goMaxLevel.SetActive(true);
            goUpdate.gameObject.SetActive(false);
            goCards.gameObject.SetActive(false);
        }
        else
        {
            goMaxLevel.gameObject.SetActive(false);
            if (currentCards >= cardReqiure)
            {

                goUpdate.gameObject.SetActive(true);
                goCards.gameObject.SetActive(false);
            }
            else
            {
                goUpdate.gameObject.SetActive(false);
                goCards.gameObject.SetActive(true);
                cardFillImg.fillAmount = (float)currentCards / cardReqiure;
                LoadCardTxt(cardReqiure);

            }
        }
        FirebaseServiceController.Instance.LogEvent($"UPGRADE_{weaponInGameData.ID}_{weaponInGameData.level}");

    }
    public string GetOfferID()
    {
        return offerID;
    }
    public string GetProcessAds()
    {
        int process = PlayerPrefs.GetInt($"{weaponInGameData.ID}_ads");
        return $"{process}/{shopWeaponData.numberAds}";
    }
    public bool UpdateProcessAds()
    {
        int process = PlayerPrefs.GetInt($"{weaponInGameData.ID}_ads");
        process++;
        PlayerPrefs.SetInt($"{weaponInGameData.ID}_ads",process);
        if (process >= shopWeaponData.numberAds)
        {
            weaponInGameData.isOwned = true;
            goOwned.SetActive(true);
            goUnlock.SetActive(false);
            levelTxt.text = weaponInGameData.level.ToString();
            isOwned = true;
            return true;
        }
        return false;
    }
}
