using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreenEffect : MonoBehaviour
{
    public float durationIn = 0.5f;
    public float durationOut = 0.5f;
    public Image image;
    public void Play()
    {
        image.DOKill();
        image.DOFade(1, durationIn).SetEase(Ease.OutSine).OnComplete(() =>
        {
            image.DOFade(0, durationOut).SetEase(Ease.OutSine);
        });

    }
}
