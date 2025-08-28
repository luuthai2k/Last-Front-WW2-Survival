using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePassBtn : MonoBehaviour
{
    public Image fillImg;
    public TextMeshProUGUI levelTxt;
    public GameObject goNotification;
  
    private void OnEnable()
    {
        if (DataController.Instance.Level < 8)
        {
            gameObject.SetActive(false);
            return;
        }
        SetUp();
        CheckNotification();
    }
    public void SetUp()
    {
        var dataSave = DataController.Instance.GetGameData().battlePassDataSave;
        levelTxt.text = (dataSave.level+1).ToString();
        if (dataSave.level < BattlePassHelper.MaxLevel())
        {
            fillImg.fillAmount = (float)dataSave.xp / BattlePassHelper.GetXPLevel(dataSave.level);
            levelTxt.text = (dataSave.level + 1).ToString();
        }
        else
        {
            fillImg.fillAmount = 1;
            levelTxt.text = BattlePassHelper.MaxLevel().ToString();
        }
    }
    public void CheckNotification()
    {
        var bpds = DataController.Instance.GetGameData().battlePassDataSave;
        if (bpds.rewardFreeCanCollect.Count > 0)
        {
            goNotification.SetActive(true);
            return;
        }
        if (bpds.rewardVipCanCollect.Count > 0&& DataController.Instance.IsBuyIAPPack("weup.ww2.duty.frontline.zone.battlepasspremium"))
        {
            goNotification.SetActive(true);
            return;
        }
        for (int i = 0; i < bpds.ListMission.Count; i++)
        {
            var data = BattlePassController.Instance.GetBattlePassMissionsData(bpds.ListMission[i].GetKeyToInt());
            if (bpds.ListMission[i].GetValueToInt() >= data.amount)
            {
                goNotification.SetActive(true);
                return;
            }
        }
        goNotification.SetActive(false);
    }
    public void OnClickBtn()
    {
        MainMenuUIManager.Instance.OnShowBattlePassPanel();
    }
   
}
