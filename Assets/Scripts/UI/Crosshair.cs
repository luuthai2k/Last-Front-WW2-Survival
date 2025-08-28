using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image crosshairTopLineImg, crosshairBotLineImg, crosshairLeftLineImg, crosshairRightLineImg;
    Coroutine coroutineOnShoot;
    public virtual void Update()
    {
        if (CameraManager.Instance.IsAimTarget(LayerConfig.Instance.shootMask))
        {
            crosshairTopLineImg.color = Color.green;
            crosshairBotLineImg.color = Color.green;
            crosshairLeftLineImg.color = Color.green;
            crosshairRightLineImg.color = Color.green;

        }
        else
        {
            crosshairTopLineImg.color = Color.white;
            crosshairBotLineImg.color = Color.white;
            crosshairLeftLineImg.color = Color.white;
            crosshairRightLineImg.color = Color.white;
        }

    }
    public virtual void Shoot ()
    {
       
       

    }
}
