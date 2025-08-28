using DG.Tweening;
using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePassPanelController : MonoBehaviour
{
    public BattlePassMissionUIManager battlePassMissionUIManager;
    public BattlePassRewardManager battlePassRewardManager;
    public GameObject battlePassIAPPack;
    public TextMeshProUGUI timeCountDownTxt;
    public double packLifeTime;
    public GameObject[] tabSelects;
    public GameObject[] goNotis;
    public TextMeshProUGUI cashTxt, goldTxt, metalTxt;
    public void UpdateText()
    {
        goldTxt.text = ToolHelper.FormatLong2(DataController.Instance.Gold);
        cashTxt.text = ToolHelper.FormatLong2(DataController.Instance.Cash);
        metalTxt.text = ToolHelper.FormatLong2(DataController.Instance.Metal);
    }
    public void Start()
    {
        SetUp();
      
    }
    private void OnEnable()
    {
        UpdateText();
        CheckNotification();
    }
    public void CheckNotification()
    {
        var bpds = DataController.Instance.GetGameData().battlePassDataSave;
        CheckRewardNotification(bpds);
        CheckMissionNotification(bpds);
    }
    public void CheckRewardNotification(BattlePassDataSave bpds)
    {
        goNotis[0].SetActive(false);
      
        if (bpds.rewardFreeCanCollect.Count > 0)
        {
            goNotis[0].SetActive(true);
            return;
        }
        if (bpds.rewardVipCanCollect.Count > 0 && DataController.Instance.IsBuyIAPPack("weup.ww2.duty.frontline.zone.battlepasspremium"))
        {
            goNotis[0].SetActive(true);
            return;
        }
    }
    public void CheckMissionNotification(BattlePassDataSave bpds)
    {
        goNotis[1].SetActive(false);
        for (int i = 0; i < bpds.ListMission.Count; i++)
        {
            var data = BattlePassController.Instance.GetBattlePassMissionsData(bpds.ListMission[i].GetKeyToInt());
            if (bpds.ListMission[i].GetValueToInt() >= data.amount)
            {
                goNotis[1].SetActive(true);
                return;
            }
        }
    }
    void SetUp()
    {
        double now = (double)GlobalTimer.Instance.GetUnixTimeStampNow();
        double originPackLifeTime = 2419200;
        packLifeTime = originPackLifeTime + (double)DataController.Instance.GetTimeStamp(IAPPackHelper.GetTimeStampKey("weup.ww2.duty.frontline.zone.battlepasspremium")) - now;
        if (packLifeTime <= 0)
        {
            packLifeTime = originPackLifeTime;
            BattlePassController.Instance.ResetData();
        }
        timeCountDownTxt.text = ToolHelper.GetTextTime(packLifeTime);
        StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        var delay = new WaitForSeconds(1);
        while (packLifeTime > 0)
        {
            timeCountDownTxt.text = ToolHelper.FormatTime(packLifeTime);
            yield return delay;
            packLifeTime--;
        }
        gameObject.SetActive(false);
        BattlePassController.Instance.ResetData();
    }

    public void OnClickActivateBtn()
    {
       _= UIManager.Instance.GetOfferPanel("weup.ww2.duty.frontline.zone.battlepasspremium");
        gameObject.SetActive(false);
    }
    public void OnSelectRewardsTab()
    {
        battlePassMissionUIManager.gameObject.SetActive(false);
        battlePassRewardManager.gameObject.SetActive(true);
        tabSelects[0].SetActive(true);
        tabSelects[1].SetActive(false);
    }
    public void OnSelectMissionTab()
    {
        battlePassRewardManager.gameObject.SetActive(false);
        battlePassMissionUIManager.gameObject.SetActive(true);
        tabSelects[1].SetActive(true);
        tabSelects[0].SetActive(false);

    }
    public void OnClickCloseBtn()
    {
        gameObject.SetActive(false);
    }
}
