using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : Singleton<MainMenuCameraController>
{
    public Camera mainCamera;
    public Transform targetTrans;
    public void SetActiveCamera(bool isActive)
    {

        mainCamera.enabled = isActive;
    }
    public void SetTargetCam(Vector3 point,float duration)
    {
        targetTrans.DOMove(point, duration);
    }
    public void SetTargetCam(Vector3 point)
    {
        targetTrans.position = point;
    }
}
