using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SniperAimCamera : MonoBehaviour
{
    public Transform parentTrans;
    private Tween moveTween;
    public void OnActive()
    {
        if(moveTween != null)
        {
            moveTween.Kill();
        }
        transform.DOKill();
        transform.parent = null;
        transform.position = CameraManager.Instance.transform.position;
        transform.rotation = CameraManager.Instance.transform.rotation;
        CameraManager.Instance.SetActiveLensRenderCamera(true);
        BlendCamera(0.55f);
    }

    void BlendCamera(float duration)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        moveTween = DOVirtual.Float(0, 1, duration, (x) =>
        {
            transform.position = Vector3.Lerp(startPos, parentTrans.position, x);
            Vector3 targetDir = CameraManager.Instance.GetAimTargetPosition() - transform.position;
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, x);
        }).SetEase(Ease.InOutQuad)
        .OnComplete(() =>
        {
            transform.parent = parentTrans;
            transform.DOLocalRotate(Vector3.zero, 0.05f);
            transform.DOLocalMove(Vector3.zero, 0.05f);

        });
    }

    public void OnDeActive()
    {
        if (moveTween != null)
        {
            moveTween.Kill();
        }
        transform.DOKill();
        transform.parent = null;
        transform.DOMove(CameraManager.Instance.transform.position,0.5f).SetEase(Ease.OutSine);
        transform.DORotate(CameraManager.Instance.transform.rotation.eulerAngles, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            CameraManager.Instance.SetActiveLensRenderCamera(false);
            transform.parent = parentTrans;
            gameObject.SetActive(false);
        });
    }
    public void Recoil()
    {
        transform.DOPunchRotation(new Vector3(-2f, 0,0), 0.45f, 7).SetEase(Ease.OutQuart);
    }
}
