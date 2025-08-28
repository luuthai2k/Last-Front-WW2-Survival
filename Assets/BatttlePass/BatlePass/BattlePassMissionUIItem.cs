using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePassMissionUIItem : MonoBehaviour
{
    public int id;
    public BattlePassMissionUIManager battlePassMissionUIManager;
    public Image fillImg;
    public TextMeshProUGUI missonTxt, rewardTxt,processTxt;
    protected BattlePassMissionsData data;
    public GameObject goButton;
    public KeyValue mission;
    public virtual void SetUpMissionData(KeyValue dt)
    {
        mission = dt;
        this.id = dt.GetKeyToInt();
        data = BattlePassController.Instance.GetBattlePassMissionsData(this.id);
        var value = dt.GetValueToInt();
        rewardTxt.text = data.xp.ToString();
        processTxt.text = value.ToString() + "/" + data.amount;
        fillImg.fillAmount = (float)value / data.amount;
      
        if (dt.GetValueToInt() >= data.amount)
        {
            goButton.SetActive(true);
        }
        LoadText();
    }
    async void LoadText()
    {
        missonTxt.text = await LocalizationManager.Instance.GetLocalizedText($"BattlePass_Mission_{data.id}");
    }
    public void OnClickCollectBtn()
    {
        battlePassMissionUIManager.UpdateProcess(data.xp, mission);
      
        gameObject.SetActive(false);
    }
    public void OnClickGetX2Btn()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
            if (x)
            {
                battlePassMissionUIManager.UpdateProcess(data.xp * 2, mission);
                gameObject.SetActive(false);
            }
        });

    }

}

