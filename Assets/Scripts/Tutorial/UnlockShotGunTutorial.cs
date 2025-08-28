using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockShotGunTutorial : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        MainMenuUIManager.Instance.OnSelectArmoryTab();
    }
    public void OnClickWeaponTabBtn()
    {
        MainMenuUIManager.Instance.armoryPanelController.OnBtnWeaponTabClick(1);
    }
    public void OnClickBuyBtn()
    {
        MainMenuUIManager.Instance.armoryPanelController.OnBtnBuyClick();
    }
    public void OnClickMissonBtn()
    {
        MainMenuUIManager.Instance.OnSelectMissionTab();
    }
    public void OnClickPlayBtn()
    {
        MainMenuUIManager.Instance.OnPlayBtnClick();
    }
}
