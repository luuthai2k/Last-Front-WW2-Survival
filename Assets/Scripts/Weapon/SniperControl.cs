using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Cinemachine;
using DG.Tweening;
using System.Drawing.Drawing2D;

public class SniperControl : BaseWeaponControl
{
    public SniperAimCamera sniperAimCamera;
    Coroutine coroutine;
    public override bool Shooting(float handStability)
    {
        Vector3 origin = CameraManager.Instance.transform.position;
        Vector3 target = CameraManager.Instance.GetAimTargetPosition();
        Vector3 direction = target - origin;
        if (GameManager.Instance.levelControl.IsLastEnermy())
        {
            Debug.Log("IsLastEnermy");
            if (Physics.Raycast(origin, CameraManager.Instance.transform.forward, out RaycastHit hit, 50, LayerConfig.Instance.rayCastMask))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && !hit.collider.GetComponent<BodyPartControl>().health.isDead)
                {
                    BodyPartControl bodyPartControl = hit.collider.GetComponent<BodyPartControl>();
                    int damage = bodyPartControl.GetDame(data.specification.damage, 1000);
                    if (bodyPartControl.health.health <= damage)
                    {
                        bodyPartControl.health.GetComponent<CharactorAnimatorController>().animator.speed = 0;
                        VibrationController.Instance.PlayLight();
                        CameraManager.Instance.SetBlend(CinemachineBlendDefinition.Style.EaseIn, 0.1f);
                        Time.timeScale = 0.1f;
                        Time.fixedDeltaTime = 0.02f / 0.1f;
                        DrawShootTrackingBullet(hit.point, hit.normal, bodyPartControl, damage);
                        sniperAimCamera.gameObject.SetActive(false);
                        CameraManager.Instance.mainCamera.enabled = true;
                        amountAmmo--;
                        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnAmmoChange, new object[] { type, amountAmmo }));
                        return true;
                    }
                    DrawShoot(direction);
                    CameraManager.Instance.TriggerRecoilSniperImpulse();
                    VibrationController.Instance.PlayLight();
                    StartCoroutine(DelayCheckDame(origin, CameraManager.Instance.transform.forward, false, damage));
                    amountAmmo--;
                    MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnAmmoChange, new object[] { type, amountAmmo }));
                    return true;
                }
            }
            DrawShoot(target - origin);
            CameraManager.Instance.TriggerRecoilSniperImpulse();
            VibrationController.Instance.PlayLight();
            StartCoroutine(DelayCheckDame(origin, CameraManager.Instance.transform.forward, true));
            amountAmmo--;
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnAmmoChange, new object[] { type, amountAmmo }));
            return true;

        }
        DrawShoot(target - origin);
        StartCoroutine(DelayCheckDame(origin, CameraManager.Instance.transform.forward, false));
        VibrationController.Instance.PlayLight();
        CameraManager.Instance.TriggerRecoilSniperImpulse();
        amountAmmo--;
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnAmmoChange, new object[] { type, amountAmmo }));
        return true;

    }
   
    IEnumerator DelayCheckDame(Vector3 origin, Vector3 direction,bool isIgnoreEnemy,int maxDamage= 1000)
    {
        yield return new WaitForSeconds(0.1f);
        hitResults = new RaycastHit[5];
        int numberOfHits = Physics.RaycastNonAlloc(origin, direction, hitResults, 100, LayerConfig.Instance.rayCastMask);
        if (numberOfHits > 0)
        {
            RaycastHit[] validHits = new RaycastHit[numberOfHits];
            Array.Copy(hitResults, validHits, numberOfHits);
            Array.Sort(validHits, (a, b) => a.distance.CompareTo(b.distance));
            int damage = data.specification.damage;
            for (int i = 0; i < numberOfHits; i++)
            {
                Debug.Log("  - Tên đối tượng: " + validHits[i].collider.gameObject.name + ", Khoảng cách: " + validHits[i].distance);
                if (validHits[i].collider.TryGetComponent(out ITakeDameBullet takeDameBullet))
                {
                    if (!isIgnoreEnemy)
                    {
                        takeDameBullet.TakeDamageBullet(validHits[i].point, validHits[i].normal, direction, damage, 1000, out damage);
                    }
                    else
                    {
                        if (validHits[i].collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                        {
                            takeDameBullet.TakeDamageBullet(validHits[i].point, validHits[i].normal, direction, damage, 1000, out damage);
                        }
                    }
                }
                if (validHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    if (validHits[i].collider.CompareTag("Metal"))
                    {
                        ResourceHelper.Instance.GetEffect(EffectType.MetalImpact, validHits[i].point, Quaternion.LookRotation(validHits[i].normal));
                    }
                    else if (validHits[i].collider.CompareTag("Wood"))
                    {
                        ResourceHelper.Instance.GetEffect(EffectType.WoodImpact, validHits[i].point, Quaternion.LookRotation(validHits[i].normal));
                    }
                    else
                    {
                        ResourceHelper.Instance.GetEffect(EffectType.StoneImpact, validHits[i].point, Quaternion.LookRotation(validHits[i].normal));
                    }
                    damage = 0;

                }
                if (damage <= 0)
                {
                    break;
                }
            }
        }
    }
    public override void DrawShoot(Vector3 direction)
    {
        int i = UnityEngine.Random.Range(0, audioClips.Length);
        AudioController.Instance.PlaySfx(audioClips[i], 0.5f);
    }
    public void DrawShootTrackingBullet(Vector3 target, Vector3 normal, BodyPartControl bodyPartControl,int damage)
    {
        PlayMuzzleEffect();
        Quaternion rot = Quaternion.LookRotation(target - muzzleTrans.position);
        GameObject bulletShot = ResourceHelper.Instance.GetBullet(WeaponType.Sniper, muzzleTrans.position, rot);
        bulletShot.GetComponent<SniperProjectile>().Init(muzzleTrans.position, target, normal, bodyPartControl, damage);
        int i = UnityEngine.Random.Range(0, audioClips.Length);
        AudioController.Instance.PlaySfx(audioClips[i], 0.5f);
    }
  
    public override void SetActiveScope(bool isActive)
    {
        if (isActive)
        {
            transform.DOKill();
            sniperAimCamera.gameObject.SetActive(true);
            sniperAimCamera.OnActive();
            coroutine=StartCoroutine(LookAtTarget());
        }
        else
        {
            if (!sniperAimCamera.gameObject.activeSelf) return;
            sniperAimCamera.OnDeActive();
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            transform.DOKill();
            transform.DOLocalRotate(Vector3.zero, 0.5f);
        }
       
    }
    public override bool IsActiveScope()
    {

        return sniperAimCamera.gameObject.activeSelf;


    }
    IEnumerator  LookAtTarget()
    {
        yield return new WaitForSeconds(0.55f);
        Vector3 direction = CameraManager.Instance.GetAimTargetPosition() - muzzleTrans.position;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.DORotate( targetRot.eulerAngles, 0.1f);
    }
    public override void PlayMuzzleEffect()
    {
        casingFx.Emit(1);
        ResourceHelper.Instance.GetEffect(EffectType.SniperMuzzleFlash, muzzleTrans.position, muzzleTrans.rotation);
    }
}
