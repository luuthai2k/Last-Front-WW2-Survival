using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShineEffect : MonoBehaviour
{
    public RectTransform rootRectTrans,shineRectTrans;
    public Image shineImg;
    public float duration, timeDelay;
    public Gradient gradient;
    float targetWidth;

    void OnEnable()
    {
        targetWidth = rootRectTrans.rect.width + rootRectTrans.rect.height+Mathf.Sqrt(2f* shineRectTrans.rect.width * shineRectTrans.rect.width);
        PlayEffect();
    }
    public void PlayEffect()
    {
        shineRectTrans.anchoredPosition = Vector3.zero;
        shineRectTrans.DOAnchorPosX(targetWidth, duration).SetEase(Ease.Linear);
        DOVirtual.Float(0f, 1f, duration, value =>
        {
            if (shineImg != null && gradient != null)
                shineImg.color = gradient.Evaluate(value);
        })
         .SetEase(Ease.Linear);
        StartCoroutine(DelayPlayEffect());
    }

  IEnumerator DelayPlayEffect()
    {
        yield return new WaitForSeconds(timeDelay);
        PlayEffect();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
