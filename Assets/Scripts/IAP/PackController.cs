using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Samples.Purchasing.Core.IAPManager;

public class PackController : MonoBehaviour
{
    [Tooltip("This pack id will automatic sync with the IAP Button pack ")]
    public string packId;
    [Tooltip("Can User buy this pack several time a day.")]
    public UnityEvent onCompletePurchase;
    public virtual void OnClickPurchase()
    {
        IAPManager.Instance.BuyPack(packId, CompletePurchase, OnPurchaseFailed);
       
    }
   public virtual void CompletePurchase(string id)
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
        IAPManager.Instance.onPurchaseComplete -= CompletePurchase;

    }
    public void LogEventPurchase()
    {
        FirebaseServiceController.Instance.LogEvent($"PURCHASE_{packId}_MAIN");
      
    }
    public void LogEventClickPurchaseBtn()
    {
        FirebaseServiceController.Instance.LogEvent($"  ENTER_IAP_{packId}_UPGRADE");
    }
    public void OnPurchaseFailed(string id)
    {
        if (packId != id) return;
        IAPManager.Instance.onPurchaseComplete -= CompletePurchase;
    }

    public void OnClickClose()
    {

        gameObject.SetActive(false);

    }
    IEnumerator DelayActive(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        gameObject.SetActive(false);
    }
   
}
