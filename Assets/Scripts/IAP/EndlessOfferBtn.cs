using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndlessOfferBtn : OfferBtn, IMessageHandle
{
    //[SerializeField] private TextMeshProUGUI coundownText;
    public double packLifeTime = 0;
    void Start()
    {
        UpdateCountDownTime();
        if (PlayerPrefs.GetInt(GameConstain.ENDLESS_OFFER_PROCESS, 0) >= 20 || DataController.Instance.Level < 5)
        {
            gameObject.SetActive(false);
            return;
        }
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnCompletePurchasePack, this);
    }
    public void UpdateCountDownTime()
    {
        double now = (double)GlobalTimer.Instance.GetUnixTimeStampNow();
        double originPackLifeTime = 172800;
        packLifeTime = originPackLifeTime + (double)DataController.Instance.GetTimeStamp(IAPPackHelper.GetTimeStampKey("weup.ww2.duty.frontline.zone.endlessoffer")) - now;
        if (packLifeTime <= 0)
        {
            packLifeTime = 172800;
            PlayerPrefs.SetInt(GameConstain.ENDLESS_OFFER_PROCESS, 0);
        }
        //coundownText.text = ToolHelper.GetTextTime(packLifeTime);
        //StartCoroutine(CountDown());
    }
    //private IEnumerator CountDown()
    //{
    //    var delay = new WaitForSeconds(1);
    //    while (packLifeTime > 0)
    //    {
    //        coundownText.text = ToolHelper.FormatTime(packLifeTime);
    //        yield return delay;
    //        packLifeTime--;
    //    }
    //    gameObject.SetActive(false);
    //}
    public void Handle(Message message)
    {
        if (message.data[0].ToString() == "weup.ww2.duty.frontline.zone.endlessoffer2")
        {

            gameObject.SetActive(false);
        }
    }
    void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnCompletePurchasePack, this);
        StopAllCoroutines();
    }
}
