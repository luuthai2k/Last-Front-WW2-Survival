using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponContentControl : MonoBehaviour, IMessageHandle
{
    public GameObject goWeaponHeader;
    public string[] packs;
    void Start()
    {
        if (CheckBuyAllPack())
        {
            goWeaponHeader.SetActive(true);
            gameObject.SetActive(false);
        }
       
    }
    void OnEnable()
    {
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnCompletePurchasePack, this);
    }
    public bool CheckBuyAllPack()
    {
        for(int i = 0; i < packs.Length; i++)
        {
            if (!DataController.Instance.IsBuyIAPPack(packs[i])) return false;
        }
        return true;
    }
    public void Handle(Message message)
    {
        if (CheckBuyAllPack())
        {
            goWeaponHeader.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    public void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnCompletePurchasePack, this);
    }
   

}
