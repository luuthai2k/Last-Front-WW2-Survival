using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompleteAlllMissionsAllItem : MonoBehaviour
{
    public BattlePassMissionUIManager battlePassMissionUIManager;
    public Image fillImg;
    public TextMeshProUGUI  processTxt;
    public GameObject goButton;
    public virtual void SetUpMissionData()
    {
        var value = DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete;
        processTxt.text = value.ToString() + "/" + 10;
        fillImg.fillAmount = (float)value / 10;
        if (value >= 10)
        {
            goButton.SetActive(true);
        }

    }
public void OnClickCollectBtn()
    {
        battlePassMissionUIManager.UpdateProcess(10);
        gameObject.SetActive(false);
        DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete = -1;
    }
    public void OnClickGetX2Btn()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
            if (x)
            {
                battlePassMissionUIManager.UpdateProcess(20);
                gameObject.SetActive(false);
                DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete = -1;
            }
        });

    }
}
