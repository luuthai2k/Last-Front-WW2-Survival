using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSoldierTutorial : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        MainMenuUIManager.Instance.OnSelectSoldierTab();
        MainMenuUIManager.Instance.soldierPanelController.FreeUpgradeSoldierTutorial();
    }
    public void OnClickUpgradeBtn()
    {
        MainMenuUIManager.Instance.soldierPanelController.OnBtnUpgradeClick();
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
