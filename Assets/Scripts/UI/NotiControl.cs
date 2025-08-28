using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotiControl : MonoBehaviour
{
    public TextMeshProUGUI notiTxt;
    public CanvasGroup canvasGroup;

    public void SendNoti(string key)
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.35f);
        LoadText(key);
        StopAllCoroutines();
        StartCoroutine(DelayActive());
    }
    public void SendNoti(string key, object arg0)
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.35f);
        LoadText(key, arg0);
        StopAllCoroutines();
        StartCoroutine(DelayActive());
    }
    IEnumerator DelayActive()
    {
        yield return new WaitForSeconds(1.5f);
        canvasGroup.DOFade(0, 0.35f);
    }
    async void LoadText(string key)
    {
        notiTxt.text = await LocalizationManager.Instance.GetLocalizedText(key);
    }
    async void LoadText(string key,object arg0)
    {
       string text = await LocalizationManager.Instance.GetLocalizedText(key);
        notiTxt.text = string.Format(text, arg0);
    }
}
