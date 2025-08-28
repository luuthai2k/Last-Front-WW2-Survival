using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePassTutorial : MonoBehaviour
{
   public void OnClickBattlePassBtn()
    {
        MainMenuUIManager.Instance.OnShowBattlePassPanel();
    }
    public void OnClickClaimBtn()
    {
        MainMenuUIManager.Instance.battlePassPanelController.battlePassRewardManager.battlePassLevels[0].ClamFreeReward();
    }
}
