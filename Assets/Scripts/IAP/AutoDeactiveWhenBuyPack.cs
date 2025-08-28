using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactiveWhenBuyPack : MonoBehaviour, IMessageHandle
{
    public string packId;
    public int levelRequier;
    void OnEnable()
    {
        if (DataController.Instance.IsBuyIAPPack(packId)||DataController.Instance.Level<levelRequier)
        {
            gameObject.SetActive(false);
            return;
        }
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnCompletePurchasePack, this);
    }
   
    public void Handle(Message message)
    {
        Debug.Log("buyPack");
        Debug.Log(message.data[0].ToString());
        if (message.data[0].ToString() == packId)
        {
          
            gameObject.SetActive(false);
        }
    }
    void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnCompletePurchasePack, this);
    }


}
