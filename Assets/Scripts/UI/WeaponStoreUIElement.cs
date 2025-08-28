using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStoreUIElement : MonoBehaviour
{
    public WeaponType weaponType;
    public int indexInData;
    public StatBar damageBar;
    public StatBar magazineBar;
    public StatBar fireRateBar;
    public void Start()
    {
        UpdateStatBar();
    }
    public void UpdateStatBar()
    {
        var wigd = DataController.Instance.GetWeaponIngameData(weaponType, indexInData);
        var cwigd = DataController.Instance.GetWeaponIngameData(weaponType, DataController.Instance.gameData.GetWeaponSelectID(weaponType));
        int currentDame = cwigd.specification.damage;
        int updateDame = wigd.specification.damage;
        int currentMegazine = cwigd.specification.magazine;
        int updateMegazine = wigd.specification.magazine;
        int currentFireRate = cwigd.specification.fireRate;
        int updateFireRate = wigd.specification.fireRate;
        damageBar.SetUp( currentDame, updateDame, GameConstain.MAX_DAMAGE);
        magazineBar.SetUp(currentMegazine, updateMegazine, GameConstain.MAX_MAGAZINE);
        fireRateBar.SetUp( currentFireRate, updateFireRate, GameConstain.MAX_FIRE_RATE);
    }
   
}
