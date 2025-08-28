using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    public GameObject equipBtn;

    public GameObject unlockBtn;

    public ShopItem[] itemList;

    public Sprite[] gunIconList;

    public int currentGunIndex;

    public List<int> gunSlotData = new List<int>();

    public TextMeshProUGUI priceText, coinText, equipText, slotText;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Coin") == 0)
        PlayerPrefs.SetInt("Coin", 1000000);
        currentGunIndex = 0;
        LoadSupply();
        LoadShopUI();
        UpdateCoinText();
        if(gunSlotData.Count <= 0)
        {
            SelectGun(0);
            UnlockGun();
            ForceEquip();
        }
        else
        {
            SelectGun(0);
        }
        UpdateGunSlotText();
    }

    public void LoadSupply()
    {

        LoadSlotData();
        PlayerPrefs.GetInt("Shield");
        PlayerPrefs.GetInt("Hat");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowSlot()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("Slot : " +(i+1) + " "  +PlayerPrefs.GetInt("Slot" + (i + 1).ToString()));
               
        }
    }

    void SaveSlot()
    {
        for(int i = 0; i < gunSlotData.Count; i++)
        {
            PlayerPrefs.SetInt("Slot" + i.ToString(), gunSlotData[i]);
        }
    }

    void LoadSlotData()
    {
        gunSlotData.Add(PlayerPrefs.GetInt("Slot0"));
        for (int i = 1; i < 4; i++)
        {
            if (PlayerPrefs.GetInt("Slot" + i.ToString()) != 0)              
              gunSlotData.Add(PlayerPrefs.GetInt("Slot" + i.ToString()));
        }
    }

    int CheckRemainSlotGun()
    { 

        return 4 - gunSlotData.Count;
    }

    public void UpdateCoinText()
    {
        int coin = PlayerPrefs.GetInt("Coin");
        coinText.text = coin.ToString();
    }

    void UpdateGunSlotText()
    {
        slotText.text =  (4- CheckRemainSlotGun()).ToString() + "/4";
    }

    public void LoadShopUI()
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            itemList[i].icon.sprite = gunIconList[i];
        }
    }

    public void SelectGun(int index)
    {
        for(int i = 0; i < itemList.Length; i++)
            itemList[i].selectObject.SetActive(false);

        currentGunIndex = index;
        itemList[index].selectObject.SetActive(true);

        if(itemList[index].Unlock == 1)
        {
            unlockBtn.SetActive(false);
            equipBtn.SetActive(true);

            if (itemList[currentGunIndex].Equip == 0)
            {

                equipText.text = "EQUIP";
            }

            else
            { 
                equipText.text = "EQUIPED";
            }
        }
        else
        {
            unlockBtn.SetActive(true);
            equipBtn.SetActive(false);
            priceText.text = itemList[index].price.ToString();

           
        }

        if(currentGunIndex == 0)
        {
            unlockBtn.SetActive(false);
            equipBtn.SetActive(true);
            equipText.text = "EQUIPED";
        }
    }

    public void UnlockGun()
    {
        int coin = PlayerPrefs.GetInt("Coin");
        if(coin >= itemList[currentGunIndex].price)
        {
            coin -= itemList[currentGunIndex].price;
            PlayerPrefs.SetInt("Coin", coin);
            UpdateCoinText();
            itemList[currentGunIndex].Unlock = 1;
            unlockBtn.SetActive(false);
            equipBtn.SetActive(true);
            if (itemList[currentGunIndex].Equip == 0)
            {

                equipText.text = "EQUIP";
            }

            else
            {
                equipText.text = "EQUIPED";
            }
        }
    }

    public void ForceEquip()
    {
        unlockBtn.SetActive(false);
        equipBtn.SetActive(true);
        if (itemList[currentGunIndex].Equip == 0)
        {
            if (CheckRemainSlotGun() > 0)
            {
               
                itemList[currentGunIndex].Equip = 1;
                equipText.text = "EQUIPED";
                gunSlotData.Add(itemList[currentGunIndex].index);
                //PlayerPrefs.SetInt("Slot" + (5 - CheckRemainSlotGun()).ToString(), itemList[currentGunIndex].index);
            }

        }
        else
        {
            itemList[currentGunIndex].Equip = 1;
            equipText.text = "EQUIPED";
        }
        UpdateGunSlotText();
    }

    public void EquipGun()
    {
        if (currentGunIndex == 0 && gunSlotData.Count < 4)
            return;

        unlockBtn.SetActive(false);
        equipBtn.SetActive(true);
       
        if (itemList[currentGunIndex].Equip == 0)
        {
            if(CheckRemainSlotGun() > 0)
            {
               
                itemList[currentGunIndex].Equip = 1;
                equipText.text = "EQUIPED";              
                gunSlotData.Add(itemList[currentGunIndex].index);
                // PlayerPrefs.SetInt("Slot" + (5 - CheckRemainSlotGun()).ToString(), itemList[currentGunIndex].index);
            }
           
        }

        else
        {

            itemList[currentGunIndex].Equip = 0;
            equipText.text = "EQUIP";
            PlayerPrefs.SetInt("Slot" + (gunSlotData.Count - 1).ToString(), 0);
            gunSlotData.Remove(itemList[currentGunIndex].index);
            //PlayerPrefs.SetInt("Slot" + (4 - CheckRemainSlotGun()).ToString(), -1);
        }
        UpdateGunSlotText();
        SaveSlot();
    }

    [Serializable]

    public class ShopItem
    {
        public GameObject selectObject;

        public Image icon;

        public int price;

        public int index;



        public int Unlock
        {
            get { return PlayerPrefs.GetInt("Gun" + index); }

            set { PlayerPrefs.SetInt("Gun" + index, value); }
        }

        public int Equip
        {
            get { return PlayerPrefs.GetInt("GunEquip" + index); }

            set { PlayerPrefs.SetInt("GunEquip" + index, value); }
        }
    }
}
