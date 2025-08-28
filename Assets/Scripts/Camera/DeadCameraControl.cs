using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class DeadCameraControl : CameraBaseControl
{
    public PostProcessVolume postProcessVolume;
    public void OnEnable()
    {
        DOTween.To(() => 0f, x => postProcessVolume.weight = x, 1, 2.5f).SetEase(Ease.InCubic);
    }
}
