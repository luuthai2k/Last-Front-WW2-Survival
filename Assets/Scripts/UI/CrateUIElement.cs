using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrateUIElement : MonoBehaviour
{
    public int pirce;
    public string Id;
    public GameObject goRemain,goButton;
    public TextMeshProUGUI remainTimeTxt;
    public void OnEnable()
    {
        if (goRemain != null)
        {
            double now =GlobalTimer.Instance.GetUnixTimeStampNow();
            double packLifeTime = IAPPackHelper.GetCrateOfferCooldown() +DataController.Instance.GetTimeStamp(IAPPackHelper.GetTimeStampKey(Id),0) - now;
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
            GetCrate();
            Wait(IAPPackHelper.GetCrateOfferCooldown());
            FirebaseServiceController.Instance.LogEvent($"REWARD_CHEST_FREE_SHOP");
        });


    }
    public void Wait(double time)
    {
        StopAllCoroutines();
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
            GetCrate();
            FirebaseServiceController.Instance.LogEvent($"BUY_{Id}_GOLD");
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
            GetCrate();
            FirebaseServiceController.Instance.LogEvent($"BUY_{Id}_CASH");
        }
        else
        {
            UIManager.Instance.SendNoti("You_not_enough_cash");
            MainMenuUIManager.Instance.storePanelController.Forcus("Cash");
        }
    }
    public void GetCrate()
    {
        if (MainMenuUIManager.Instance != null)
        {
            MainMenuUIManager.Instance.gameObject.SetActive(false);
        }
        if (GamePlayUIManager.Instance != null)
        {
            GamePlayUIManager.Instance.gameObject.SetActive(false);
        }
        GameObject go = UIManager.Instance.SpawnLootBox(Id); ;
        go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
        {
            if (MainMenuUIManager.Instance != null)
            {
                MainMenuUIManager.Instance.gameObject.SetActive(true);
            }
            if (GamePlayUIManager.Instance != null)
            {
                GamePlayUIManager.Instance.gameObject.SetActive(true);
            }
        });
    }
}
