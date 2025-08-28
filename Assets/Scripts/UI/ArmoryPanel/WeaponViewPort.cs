using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class WeaponViewPort : MonoBehaviour
{
    public WeaponType weaponType;
    private bool isInited=false;
    public WeaponUIElement[] weaponUIElements;
    public WeaponUIElement weaponEquipedUIElement;
    public void SetUp(UnityAction<WeaponUIElement>onClick)
    {
      
        if (isInited) 
        {
            for (int i = weaponUIElements.Length - 1; i >= 0; i--)
            {
                weaponUIElements[i].SetUp(weaponType,null);
            }
            weaponEquipedUIElement.IsSelect(true);
            return; 
        }
        int equipedWeaponId = DataController.Instance.gameData.GetWeaponSelectID(weaponType);
        for (int i = weaponUIElements.Length-1; i>=0 ; i--)
        {
            weaponUIElements[i].SetUp(weaponType, onClick);
            if (weaponUIElements[i].indexInData == equipedWeaponId)
            {
                weaponEquipedUIElement = weaponUIElements[i];
                weaponUIElements[i].IsEquiped(true);
                weaponUIElements[i].IsSelect(true);
            }
        }
        isInited = true;
    }
 
}
