using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public void Play()
    {
        canvasGroup.DOFade(0.5f, 0.1f).SetEase(Ease.InSine).OnComplete(() =>
        {
            canvasGroup.DOFade(0, 0.1f).SetEase(Ease.InSine);
        });
    }
}
