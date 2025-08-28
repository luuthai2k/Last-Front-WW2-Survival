using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineGunCrossHair : SMGCrossHair
{
    private bool isStart;

    void OnEnable()
    {
        handStability = DataController.Instance.GetCurrentSoldierIngameData().soldierStat.handStability;
        isStart = true;
        crosshairLeftLineImg.transform.localPosition = Vector3.left * damping * startStability / handStability;
        crosshairRightLineImg.transform.localPosition = Vector3.right * damping * startStability / handStability;
        crosshairLeftLineImg.transform.DOLocalMove(Vector3.zero, 0.1f);
        crosshairRightLineImg.transform.DOLocalMove(Vector3.zero, 0.1f).OnComplete(() =>
        {
            isStart = false;
        });
    }

    public override void Update()
    {
        if (CameraManager.Instance.IsAimTarget(LayerConfig.Instance.shootMask))
        {
            crosshairLeftLineImg.color = Color.green;
            crosshairRightLineImg.color = Color.green;
            crosshairBotLineImg.color = Color.green;

        }
        else
        {
            crosshairLeftLineImg.color = Color.white;
            crosshairRightLineImg.color = Color.white;
            crosshairBotLineImg.color = Color.white;
        }
        if (!isStart)
        {
            float stability = damping * CameraManager.Instance.GetStability() / handStability;
            crosshairLeftLineImg.transform.localPosition = Vector3.left * stability + Vector3.left * offset;
            crosshairRightLineImg.transform.localPosition = Vector3.right * stability + Vector3.right * offset;
        }

    }
  
}
