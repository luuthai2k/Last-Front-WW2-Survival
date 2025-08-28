using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBaseControl : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    protected Cinemachine3rdPersonFollow thirdPersonFollow;
    [SerializeField] protected float rotationLerp = 5;
    [SerializeField] protected Transform targetTrans;
    [SerializeField] protected float verticalSpeed = 6f;
    [SerializeField] protected float horizontalSpeed = 6f;
    [SerializeField] protected float verticalShootSpeed = 6f;
    [SerializeField] protected float horizontalShootSpeed = 6f;
    [SerializeField] protected float maxVerticalAngle = 45f;
    [SerializeField] protected float minVerticalAngle = -45f;
    [SerializeField] protected float maxHorizontalAngle = 110f;
    [SerializeField] protected float minHorizontalAngle = -110f;
    protected float angleH = 0;
    protected float angleV = 0;
    public float stability = 0;
    protected Quaternion targetRotation;
    public Vector2 offset;
    Tween verticalFOVTween;
    public virtual void Update()
    {
      
        switch (PlayerController.Instance.currentState)
        {
            case PlayerState.Ready:
            case PlayerState.Win:
            case PlayerState.Dead:
                break;
            case PlayerState.Move:
                targetTrans.localRotation = Quaternion.RotateTowards(targetTrans.localRotation,Quaternion.identity, rotationLerp * Time.deltaTime);
                angleV = 0;
                angleH = 0;
                break;
            case PlayerState.Reload:
            case PlayerState.Cover:
            case PlayerState.Shoot:
            case PlayerState.Grenade:

#if UNITY_EDITOR

                if (Input.GetMouseButton(0))
                {
                    if (PlayerController.Instance.currentState == PlayerState.Shoot)
                    {
                        angleH += Input.GetAxis("Mouse X") *20* horizontalShootSpeed * Time.deltaTime;
                        angleV -= Input.GetAxis("Mouse Y") * 20 * verticalShootSpeed * Time.deltaTime;
                    }
                    else
                    {
                        angleH += Input.GetAxis("Mouse X") * 20 * horizontalSpeed * Time.deltaTime;
                        angleV -= Input.GetAxis("Mouse Y") * 20 * verticalSpeed * Time.deltaTime;
                    }
                   
                }

#endif
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved)
                    {
                        if (PlayerController.Instance.currentState == PlayerState.Shoot)
                        {
                            angleH += touch.deltaPosition.x * horizontalShootSpeed * Time.deltaTime;
                            angleV -= touch.deltaPosition.y * verticalShootSpeed * Time.deltaTime;

                        }
                        else
                        {
                            angleH += touch.deltaPosition.x * horizontalSpeed * Time.deltaTime;
                            angleV -= touch.deltaPosition.y * verticalSpeed * Time.deltaTime;
                        }
                    }

                }
#endif
                angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
                angleH = Mathf.Clamp(angleH, minHorizontalAngle, maxHorizontalAngle);
                targetRotation = Quaternion.Lerp(targetRotation, Quaternion.Euler(angleV, angleH, 0), rotationLerp * Time.deltaTime);
                targetTrans.localRotation = Quaternion.Euler( targetRotation.eulerAngles.x+offset.x,targetRotation.eulerAngles.y+offset.y,0);
                stability = Quaternion.Angle(targetTrans.localRotation, Quaternion.Euler(angleV, angleH, 0));
                break;
        }

    }
    public void SetTarget( Transform target)
    {
        targetTrans = target;
        virtualCamera.Follow = target;
    }
    public void SetLookAt(Transform lookAt)
    {
        virtualCamera.LookAt = lookAt;

    }
    public void SetCamSideUseTween(float value, float time)
    {
        if (thirdPersonFollow == null)
        {
            thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }
        if(value!= thirdPersonFollow.CameraSide)
        {
            DOTween.To(() => thirdPersonFollow.CameraSide, x => thirdPersonFollow.CameraSide = x, value, time).SetEase(Ease.OutSine);
        }
    }
    public void SetCamSide(float value)
    {
        if (thirdPersonFollow == null)
        {
            thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }
        thirdPersonFollow.CameraSide = value;
    }
    public float GetCamSide()
    {
        if (thirdPersonFollow == null)
        {
            thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }
        return thirdPersonFollow.CameraSide ;
    }
    public void SetCamDistanceUseTween(float value, float time)
    {
        if (thirdPersonFollow == null)
        {
            thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }
        DOTween.To(() => thirdPersonFollow.CameraDistance, x => thirdPersonFollow.CameraDistance = x, value, time).SetEase(Ease.OutSine);
    }
    public void SetVerticalArmLengthUseTween(float value, float time)
    {
        if (thirdPersonFollow == null)
        {
            thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }
        DOTween.To(() => thirdPersonFollow.VerticalArmLength, x => thirdPersonFollow.VerticalArmLength = x, value, time).SetEase(Ease.OutSine);
    }
    public void SetRotationToCenter(float time)
    {
        targetTrans.DOLocalRotate(Vector3.zero, time);
    }
    public Quaternion GetTargetRotation()
    {
        return Quaternion.Euler(angleV, angleH, 0);
    }
    public void ForceCameraPositionToTarget()
    {
        virtualCamera.ForceCameraPosition(targetTrans.position, targetTrans.rotation);
    }
    public void SetVerticalFOVUseTween(float value, float time,Action onComplete=null)
    {
        if (verticalFOVTween != null)
            verticalFOVTween.Kill();
        verticalFOVTween=DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, value, time).SetEase(Ease.InSine).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
    public void SetDamping(Vector3 target, float wait)
    {
        StartCoroutine(WaitSetDamping());
        IEnumerator WaitSetDamping()
        {
            yield return new WaitForSeconds(wait);

            if (thirdPersonFollow == null)
            {
                thirdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            }
            thirdPersonFollow.Damping = target;
        }
    }
    //public
}
