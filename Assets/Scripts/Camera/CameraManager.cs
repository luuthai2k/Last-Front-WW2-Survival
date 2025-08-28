using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;
using DG.Tweening;

public class CameraManager : Singleton<CameraManager>
{
    public CinemachineImpulseSource impulseSource;
    public CameraBaseControl[] virtualCameras;
    public CameraBaseControl currentVirtualCamnera;
    public Camera mainCamera,lensRenderTextureCamera;
    public Transform aimTargetTrans;
    private RaycastHit aimHit;
    public int impluseRank;
    bool isActive=true;
    public void Update()
    {
        if (!isActive) return;
        if (Physics.Raycast(transform.position, transform.forward, out aimHit, 50, LayerConfig.Instance.rayCastMask))
        {
            aimTargetTrans.position = aimHit.point;
        }
        else
        {
            aimHit = new RaycastHit();
            aimTargetTrans.position = transform.position+transform.forward*50;
        }
    }
    public void ChangeCamera(CameraType type)
    {
       
        int index = (int)type;
        for(int i=0;i< virtualCameras.Length; i++)
        {
            if (i == index)
            {
                virtualCameras[i].gameObject.SetActive(true);
                currentVirtualCamnera = virtualCameras[i];
            }
            else
            {
                virtualCameras[i].gameObject.SetActive(false);
            }
        }

    }
    public void ChangeCamera(int index)
    {
        if (currentVirtualCamnera != null) 
        {
            currentVirtualCamnera.gameObject.SetActive(false);
        }
        virtualCameras[index].gameObject.SetActive(true);

    }
    public void ChangeCamera(CameraType type, Transform target)
    {
        int index = (int)type;
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (i == index)
            {
             
                virtualCameras[i].SetTarget(target);
                virtualCameras[i].gameObject.SetActive(true);
                currentVirtualCamnera = virtualCameras[i];
            }
            else
            {
                virtualCameras[i].gameObject.SetActive(false);
            }
        }

    }
   
    public bool IsAimTarget(LayerMask mask, out RaycastHit hit)
    {
        hit = aimHit;
        if (aimHit.collider == null) return false;
        if (((1 << aimHit.collider.gameObject.layer) & mask) != 0) return true;
        return false;

    }
    public bool IsAimTarget(LayerMask mask)
    {
        if (aimHit.collider == null) return false;
        if (((1 << aimHit.collider.gameObject.layer) & mask) != 0) return true;
        return false;

    }
    public bool IsAimTarget(out RaycastHit hit)
    {
        hit = aimHit;
        if (aimHit.collider == null) return false;
        return true;

    }
    public float GetStability()
    {
        return currentVirtualCamnera.stability;
    }
    public Vector3 GetAimTargetPosition()
    {
        return aimTargetTrans.position;
    }
    public RaycastHit GetAimHit()
    {
        return aimHit;
    }
    public void TriggerBigExplosionImpulse()
    {
        if (impluseRank > 3) return;
        impluseRank = 3;
        var perlin = currentVirtualCamnera.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_FrequencyGain = 2f;
        DOTween.To(() => perlin.m_AmplitudeGain, x => perlin.m_AmplitudeGain = x, 1f, 0.1f).SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                DOTween.To(() => perlin.m_AmplitudeGain, x => perlin.m_AmplitudeGain = x, 0, 0.75f).SetEase(Ease.InCubic)
                    .OnComplete(() => { impluseRank = 1; });
            });
        impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = 0.5f;
        impulseSource.GenerateImpulse(new Vector3(0.25f, 0.1f, 0f));
    }
    public void TriggerSmallExplosionImpulse()
    {
        if (impluseRank > 2) return;
        impluseRank = 2;
        var perlin = currentVirtualCamnera.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_FrequencyGain = 1f;
        DOTween.To(() => perlin.m_AmplitudeGain, x => perlin.m_AmplitudeGain = x, 0.5f, 0.1f).SetEase(Ease.OutSine)
           .OnComplete(() =>
           {
               DOTween.To(() => perlin.m_AmplitudeGain, x => perlin.m_AmplitudeGain = x, 0, 0.75f).SetEase(Ease.InCubic)
                   .OnComplete(() => { impluseRank = 1; });
           });
        impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = 0.5f;
        impulseSource.GenerateImpulse(new Vector3(0.2f, 0.1f, 0f));
    }
    public void TriggerRecoilSMGImpulse()
    {
        if (impluseRank > 1) return;
        impluseRank = 1;
        impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil;
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = 0.1f;
        impulseSource.GenerateImpulse(new Vector3(0, 0.05f, -0.01f));
    }
    public void TriggerRecoilSniperImpulse()
    {
        if (impluseRank > 1) return;
        impluseRank = 1;
        impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil;
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = 0.5f;
        impulseSource.GenerateImpulse(new Vector3(0, 0, -0.1f));
        DOTween.To(() => currentVirtualCamnera.offset, x => currentVirtualCamnera.offset = x, new Vector2(-1.5f, 0f), 0.035f).SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                DOTween.To(() => currentVirtualCamnera.offset, x => currentVirtualCamnera.offset = x, Vector2.zero, 0.5f).SetEase(Ease.InSine);
            });

    }
    public void TriggerRecoilMachinegunImpulse()
    {
        if (impluseRank > 1) return;
        impluseRank = 1;
        impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil;
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = 0.1f;
        impulseSource.GenerateImpulse(new Vector3(0.01f, 0.1f, -0.05f));

    }
    public void TriggerRecoilRocketImpulse()
    {
        if (impluseRank > 1) return;
        impluseRank = 1;
        impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Recoil;
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = 0.1f;
        impulseSource.GenerateImpulse(new Vector3(0f, 0f, -0.75f));

    }
    public void TriggerDeadImpulse()
    {
        if (impluseRank > 1) return;
        impluseRank = 1;
        impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = 1f;
        impulseSource.GenerateImpulse(new Vector3(0f, 1.5f, 0f));

    }
    public void SetActiveLensRenderCamera(bool isActive)
    {
        lensRenderTextureCamera.enabled = isActive;
        mainCamera.enabled = !isActive;
    }
    public Quaternion GetTargetRotation()
    {
        if (currentVirtualCamnera == null)
        {
            currentVirtualCamnera = virtualCameras[0];
        }
        return currentVirtualCamnera.GetTargetRotation();
    }
    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
        currentVirtualCamnera.gameObject.SetActive(isActive);
    }

    public void SetBlend(CinemachineBlendDefinition.Style style,float time)
    {
        CinemachineBrain cinemachineBrain = GetComponent<CinemachineBrain>();
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(style, time);
    }
    public void MoveState()
    {
        currentVirtualCamnera.SetCamSideUseTween(0.5f, 0.5f);
        currentVirtualCamnera.SetVerticalFOVUseTween(60, 0.5f);
        currentVirtualCamnera.SetRotationToCenter(0.5f);
    }
    public void CoverState(CoverType coverType, CoverDirection coverDirection)
    {
        float side = coverDirection == CoverDirection.Left ? -1 : 1;
      
        if (coverType == CoverType.LowCover)
        {
            if (coverDirection == CoverDirection.Right)
            {
                currentVirtualCamnera.SetCamSideUseTween(0.7f, 0.5f);
            }
            else
            {
                currentVirtualCamnera.SetCamSideUseTween(0.45f, 0.5f);
            }
            currentVirtualCamnera.SetVerticalArmLengthUseTween(0.4f, 0.35f);
            currentVirtualCamnera.SetCamDistanceUseTween(2.5f, 0.35f);
        }
       else if (coverType == CoverType.HightCover)
        {
            if (coverDirection == CoverDirection.Right)
            {
                currentVirtualCamnera.SetCamSideUseTween(0.8f, 0.5f);
                currentVirtualCamnera.SetVerticalArmLengthUseTween(0.2f, 0.5f);
            }
            else
            {
                currentVirtualCamnera.SetCamSideUseTween(0.2f, 0.5f);
                currentVirtualCamnera.SetVerticalArmLengthUseTween(0.25f, 0.5f);
            }
            currentVirtualCamnera.SetCamDistanceUseTween(2.5f, 0.35f);
        }
       
        currentVirtualCamnera.SetVerticalFOVUseTween(60, 0.35f);
       

    }
    public void AimState(CoverType coverType, CoverDirection coverDirection)
    {
        if (coverType == CoverType.LowCover)
        {
            currentVirtualCamnera.SetCamDistanceUseTween(1.5f, 0.5f);
            currentVirtualCamnera.SetVerticalFOVUseTween(35, 0.5f);
        }
        if (coverType == CoverType.HightCover)
        {
           
            currentVirtualCamnera.SetCamDistanceUseTween(2f, 0.5f);
            currentVirtualCamnera.SetVerticalFOVUseTween(35, 0.5f);
        }
       

    }
   
}
public enum CameraType
{
    Base=0,
    Sniper = 1,
    Machinegun =2,
    Rocket=3,
    Support=4,
    Dead=5,
    KillEnemy=6
}
