using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWeaponTutorial : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        MainMenuUIManager.Instance.OnSelectArmoryTab();
        MainMenuUIManager.Instance.armoryPanelController.FreeUpgradeWeaponTutorial();
    }
    public void OnClickUpgradeBtn()
    {
        MainMenuUIManager.Instance.armoryPanelController.OnBtnUpgradeClick();
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
