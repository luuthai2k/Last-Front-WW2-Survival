using DG.Tweening;
using Samples.Purchasing.Core.IAPManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndlessOfferPack : MonoBehaviour
{
    public GameObject goLock,goBorder,goShine;
    public EndlessOfferType type;
    public Pack pack;
    public UnityEvent onComplete;
    public UnityEvent onClick;
    public Button button;
    public CanvasGroup canvasGroup;
   

    public void Unlock()
    {
        goLock.gameObject.SetActive(false);
        goBorder.gameObject.SetActive(true);
        goShine.gameObject.SetActive(true);
        button.interactable = true;
    }
    public void OnClickPurchaseBtn()
    {
        onClick?.Invoke();
        IAPManager.Instance.BuyPack(pack.PackID, CompletePurchase, OnPurchaseFailed);

    }
    public void CompletePurchase(string id)
    {
        if (pack.PackID != id) return;
        UIManager.Instance.GetPackReward(pack, () =>
        {
            button.interactable = false;
            canvasGroup.alpha = 0.5f;
            onComplete?.Invoke();
        });
        DataController.Instance.AddListIAP(pack.PackID);
        DataController.Instance.SaveData();
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnCompletePurchasePack, new object[] { pack.PackID }));
        IAPManager.Instance.onPurchaseComplete -= CompletePurchase;

    }
    public void OnPurchaseFailed(string id)
    {
        if (pack.PackID != id) return;
        IAPManager.Instance.onPurchaseComplete -= CompletePurchase;
    }
    public void OnClickAdsBtn()
    {
        onClick?.Invoke();
        ManagerAds.ins.ShowRewarded((x) =>
        {
            UIManager.Instance.GetPackReward(pack, () =>
            {
                button.interactable = false;
                canvasGroup.alpha = 0.5f;
                onComplete?.Invoke();
            });

        });

    }
    public void OnClickFreeBtn()
    {
        onClick?.Invoke();
        UIManager.Instance.GetPackReward(pack, () =>
        {
            button.interactable = false;
            canvasGroup.alpha = 0.5f;
            onComplete?.Invoke();
        });

    }


}
public enum EndlessOfferType
{
    Free,
    Ads,
    Purchase
}

