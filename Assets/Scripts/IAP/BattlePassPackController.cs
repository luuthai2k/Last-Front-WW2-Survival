using Samples.Purchasing.Core.IAPManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattlePassPackController : PackController
{
    public TextMeshProUGUI timeCountDownTxt;
    public double packLifeTime;
    public void Start()
    {
        SetUp();
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

    public override void CompletePurchase(string id)
    {
        Debug.Log(id);
        if (packId != id) return;
        var packData = IAPPackHelper.GetPack(packId);
        UIManager.Instance.GetPackReward(packData, () =>
        {
            onCompletePurchase?.Invoke();
        });
        DataController.Instance.AddListIAP(packId);
        DataController.Instance.SaveData();
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnCompletePurchasePack, new object[] { packId }));
        MainMenuUIManager.Instance.OnShowBattlePassPanel();
        IAPManager.Instance.onPurchaseComplete -= CompletePurchase;
        gameObject.SetActive(false);
       

    }
}

