using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMachineGunControl : CameraBaseControl
{
    public override void Update()
    {

        switch (PlayerController.Instance.currentState)
        {
            case PlayerState.Ready:
            case PlayerState.Win:
            case PlayerState.Dead:
                break;
            case PlayerState.Move:
                targetTrans.localRotation = Quaternion.RotateTowards(targetTrans.localRotation, Quaternion.identity, rotationLerp * Time.deltaTime);
                angleV = 0;
                angleH = 0;
                break;
            case PlayerState.Reload:
            case PlayerState.Cover:
            case PlayerState.Shoot:

#if UNITY_EDITOR

                if (Input.GetMouseButton(0))
                {
                    if (PlayerUIControl.Instance.isAim)
                    {
                        angleH += Input.GetAxis("Mouse X") * 20 * horizontalShootSpeed * Time.deltaTime;
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
                         if (PlayerUIControl.Instance.isAim)
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
                targetTrans.localRotation = Quaternion.Euler(targetRotation.eulerAngles.x + offset.x, targetRotation.eulerAngles.y + offset.y, 0);
                stability = Quaternion.Angle(targetTrans.localRotation, Quaternion.Euler(angleV, angleH, 0));
                break;
        }

    }
}
