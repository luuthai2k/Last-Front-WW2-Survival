using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SMGCrossHair : Crosshair
{
    public float handStability,damping=150, startStability=20,offset;
    private bool isStart;

    void OnEnable()
    {
        isStart = true;
        handStability = DataController.Instance.GetCurrentSoldierIngameData().soldierStat.handStability;
        crosshairTopLineImg.transform.localPosition = Vector3.up * damping * startStability / handStability; 
        crosshairBotLineImg.transform.localPosition = Vector3.down * damping * startStability / handStability; 
        crosshairLeftLineImg.transform.localPosition = Vector3.left * damping * startStability / handStability; 
        crosshairRightLineImg.transform.localPosition = Vector3.right * damping * startStability / handStability; 
        crosshairTopLineImg.transform.DOLocalMove(Vector3.zero, 0.5f);
        crosshairBotLineImg.transform.DOLocalMove(Vector3.zero, 0.5f);
        crosshairLeftLineImg.transform.DOLocalMove(Vector3.zero, 0.5f);
        crosshairRightLineImg.transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() =>
        {
            isStart = false;
        });
    }

    public override void Update()
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
        if (!isStart)
        {
            float stability = damping * CameraManager.Instance.GetStability() / handStability;
            crosshairTopLineImg.transform.localPosition = Vector3.up * stability  + Vector3.up * offset;
            crosshairBotLineImg.transform.localPosition = Vector3.down * stability  + Vector3.down * offset;
            crosshairLeftLineImg.transform.localPosition = Vector3.left * stability  + Vector3.left * offset;
            crosshairRightLineImg.transform.localPosition = Vector3.right * stability  + Vector3.right * offset;
        }



    }
    public override void Shoot()
    {
        if (offset != 0) return;
        DOTween.To(() => 0f, x => offset = x, 10, 0.05f).SetEase(Ease.InSine).OnComplete(() =>
        {
            DOTween.To(() => 0f, x => offset = x, 0, 0.05f).SetEase(Ease.OutSine);
        });
    }
}
