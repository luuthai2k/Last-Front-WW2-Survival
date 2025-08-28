using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePopUp : MonoBehaviour
{
   public void OnClickCloseBtn()
    {
        gameObject.SetActive(false);
        GamePlayUIManager.Instance.OpenFailPopUp();
    }
    public void OnClickFreeAdsBtn()
    {
        Revive();
    }
    public void Revive()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
            PlayerController.Instance.Revive();
            gameObject.SetActive(false);
            FirebaseServiceController.Instance.LogEvent($"REWARD_REVIVE");
        });

       
    }
}
