using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactiveWhenRemoveAds : MonoBehaviour, IMessageHandle
{
    void OnEnable()
    {
        if (PlayerPrefs.GetInt("RemoveAds", 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnRemoveAds, this);
    }
    public void Handle(Message message)
    {
      
        gameObject.SetActive(false);
    }
    public void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnRemoveAds, this);
    }
   
}
