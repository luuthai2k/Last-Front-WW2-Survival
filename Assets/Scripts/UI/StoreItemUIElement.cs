using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreItemUIElement : MonoBehaviour
{
    public string Id;
    public int pirce;
    public KeyValue[] keyValues;
    public GameObject goRemain, goButton;
    public TextMeshProUGUI remainTimeTxt;
    public void OnEnable()
    {
        if (goRemain != null)
        {
            double now = GlobalTimer.Instance.GetUnixTimeStampNow();
            double packLifeTime = IAPPackHelper.GetCrateOfferCooldown() + DataController.Instance.GetTimeStamp(IAPPackHelper.GetTimeStampKey(Id), 0) - now;
            Debug.LogWarning(DataController.Instance.GetTimeStamp(IAPPackHelper.GetTimeStampKey(Id)));
            if (packLifeTime > 0)
            {
                Wait(packLifeTime);
            }
        }
    }
    public void OnClickAdsBtn()
    {
        Debug.LogWarning("OnClickAdsBtn");

        ManagerAds.ins.ShowRewarded((x) =>
        {
            GetReward();
            Wait(IAPPackHelper.GetItemOfferCooldown());
            FirebaseServiceController.Instance.LogEvent($"REWARD_GET_{Id}");
        });
    }
    public void Wait(double time)
    {
        double now = (double)GlobalTimer.Instance.GetUnixTimeStampNow();
        DataController.Instance.SetTimeStamp(IAPPackHelper.GetTimeStampKey(Id), now);
        goRemain.gameObject.SetActive(true);
        goButton.gameObject.SetActive(false);
        StartCoroutine(CountDown(time));
    }
    IEnumerator CountDown(double time)
    {
        var delay = new WaitForSeconds(1);
        while (time > 0)
        {
            remainTimeTxt.text = ToolHelper.FormatTime(time);
            yield return delay;
            time--;
        }
        goRemain.gameObject.SetActive(false);
        goButton.gameObject.SetActive(true);
    }
    public void OnClickBuyGoldBtn()
    {
        if (DataController.Instance.Gold >= pirce)
        {
            DataController.Instance.AddGold(-pirce);
            GetReward();
            FirebaseServiceController.Instance.LogEvent($"BUY_{Id}");
        }
        else
        {
            UIManager.Instance.SendNoti("You_not_enough_gold");
            MainMenuUIManager.Instance.storePanelController.Forcus("Gold");
        }
    }
    public void OnClickBuyCashBtn()
    {
        if (DataController.Instance.Cash >= pirce)
        {
            DataController.Instance.AddCash(-pirce);
            GetReward();
            FirebaseServiceController.Instance.LogEvent($"BUY_{Id}");
        }
        else
        {
            UIManager.Instance.SendNoti("You_not_enough_cash");
            MainMenuUIManager.Instance.storePanelController.Forcus("Cash");
        }
    }
    public void GetReward()
    {
        foreach(var item in keyValues)
        {
            DataController.Instance.GetReward(item);
        }
        UIManager.Instance.SpawmReward(keyValues);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
