using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTutorial : MonoBehaviour
{
    public void OnClickStoreBtn()
    {
        MainMenuUIManager.Instance.OnSelectStoreTab("Crates");

       
    }
    public void OnClickFreeCrateBtn()
    {
        MainMenuUIManager.Instance.storePanelController.OnClickFreeCrateBtn();
    }
    public void OnClickMissonBtn()
    {
        MainMenuUIManager.Instance.OnSelectMissionTab();
    }
}
