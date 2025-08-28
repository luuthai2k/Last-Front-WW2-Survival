using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIEquipment : MonoBehaviour,IMessageHandle
{
    public WeaponType weaponType;
    public TextMeshProUGUI ammoTxt;
    public Image weaponIcon;
    public GameObject goEquip,goBase;
    public bool isEquip = false;
   public void SetUp(WeaponInGameData data,Sprite sprite)
    {
        weaponType = data.weaponType;
        ammoTxt.text = data.specification.magazine.ToString();
        weaponIcon.sprite = sprite;
    }
    public void Start()
    {
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnAmmoChange, this);
    }
    public void Handle(Message message)
    {
        if ((WeaponType)message.data[0] == weaponType)
        {
            ammoTxt.text = message.data[1].ToString();
        }
       
    }
    public void SetEquip(bool isEquip)
    {
        this.isEquip = isEquip;
        goEquip.SetActive(isEquip);
        goBase.SetActive(!isEquip);


    }
    

    public void OnDestroy()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnAmmoChange, this);
    }
}
