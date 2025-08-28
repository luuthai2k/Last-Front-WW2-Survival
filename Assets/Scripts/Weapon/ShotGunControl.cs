using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunControl : BaseWeaponControl
{
    [SerializeField] private int bulletsPerShot = 8;
    public override bool Shooting(float handStability)
    {
        if (CameraManager.Instance.IsAimTarget(LayerConfig.Instance.shootMask, out RaycastHit hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && hit.collider.GetComponent<BodyPartControl>().health.isDead) return false;
            Vector3 origin = muzzleTrans.position;
            Vector3 target = CameraManager.Instance.GetAimTargetPosition();
            for (int i = 0; i < bulletsPerShot; i++)
            {

                Vector3 direction = GetRandomDirectionInCone(target - origin);
                DrawShoot(direction);
                StartCoroutine(DelayCheckDame(origin, direction));
                VibrationController.Instance.PlayLight();
                CameraManager.Instance.TriggerRecoilSMGImpulse();
                PlayerController.Instance.animatorController.Recoil(1);
            }
            PlayMuzzleEffect();
            int index = UnityEngine.Random.Range(0, audioClips.Length);
            AudioController.Instance.PlaySfx(audioClips[index], 0.5f);
            amountAmmo--;
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnAmmoChange, new object[] { type, amountAmmo }));
            return true;
        }
        return false;
       
      
    }
    IEnumerator DelayCheckDame(Vector3 origin, Vector3 direction)
    {
        yield return new WaitForSeconds(0.1f);
        hitResults = new RaycastHit[5];
        int numberOfHits = Physics.RaycastNonAlloc(origin, direction, hitResults, 100, LayerConfig.Instance.rayCastMask);
        if (numberOfHits > 0)
        {
            RaycastHit[] validHits = new RaycastHit[numberOfHits];
            Array.Copy(hitResults, validHits, numberOfHits);
            Array.Sort(validHits, (a, b) => a.distance.CompareTo(b.distance));
            int damage = data.specification.damage/ bulletsPerShot;
            for (int i = 0; i < numberOfHits; i++)
            {
                Debug.Log("  - Tên đối tượng: " + validHits[i].collider.gameObject.name + ", Khoảng cách: " + validHits[i].distance);
                if (validHits[i].collider.TryGetComponent(out ITakeDameBullet takeDameBullet))
                {

                    takeDameBullet.TakeDamageBullet(validHits[i].point, validHits[i].normal, direction, damage, 1000, out damage);

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
      
        GameObject bulletShot = ResourceHelper.Instance.GetBullet(type);
        bulletShot.GetComponent<Projectile>().Init(muzzleTrans.position, direction);
        
    }
    public override void PlayMuzzleEffect()
    {
        casingFx.Emit(1);
        ResourceHelper.Instance.GetEffect(EffectType.ShotGunMuzzleFlash, muzzleTrans.position, muzzleTrans.rotation);
    }
}
