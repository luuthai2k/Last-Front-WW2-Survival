using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButton : MonoBehaviour
{
    public GameObject goContent;
    public void OnClickBtn()
    {
      
        if (!goContent.activeSelf)
        {
            goContent.gameObject.SetActive(true);
            goContent.transform.localScale = Vector3.zero;
            goContent.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        }
        else
        {
            goContent.gameObject.SetActive(false);
        }
       
    }
}
