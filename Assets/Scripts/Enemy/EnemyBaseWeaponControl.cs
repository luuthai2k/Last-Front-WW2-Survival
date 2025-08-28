using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseWeaponControl : MonoBehaviour
{
    public WeaponType weaponType;
    public Transform muzzleTrans;
    public AudioClip[] audioClips;
    public float spread = 1f, safe = 1;
    public int damage = 10;
    public AudioSource audioSource;
    public ParticleSystem casingFx;
    public virtual void Shooting(Vector3 target)
    {
        target +=Vector3.right* Random.Range(-spread, spread)+Vector3.up* Random.Range(0, spread)+Vector3.forward* Random.Range(-spread, spread);
        Vector3 direction = target-muzzleTrans.position;
        if (Physics.Raycast(muzzleTrans.position, direction, out RaycastHit shootHit, 200f, LayerConfig.Instance.enemyRayCastMask))
        {
            if (shootHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (PlayerController.Instance.playerHealth.canTakeDame)
                {
                    PlayerController.Instance.playerHealth.TakeDamageBullet(damage);
                    DrawShootToTarget(shootHit.point);
                    PlayMuzzleEffect();
                    return;
                }
                target.x += target.x > 0 ? safe : -safe;
                DrawShoot(target - muzzleTrans.position);
            }
            else
            {
                target = shootHit.point;
                if (shootHit.collider.CompareTag("Metal"))
                {
                    ResourceHelper.Instance.GetEffect(EffectType.MetalImpact, shootHit.transform, shootHit.point, Quaternion.LookRotation(shootHit.normal));
                }
                else if (shootHit.collider.CompareTag("Wood"))
                {
                    ResourceHelper.Instance.GetEffect(EffectType.WoodImpact, shootHit.transform, shootHit.point, Quaternion.LookRotation(shootHit.normal));
                }
                else if (shootHit.collider.CompareTag("Stone"))
                {
                    ResourceHelper.Instance.GetEffect(EffectType.StoneImpact, shootHit.transform, shootHit.point, Quaternion.LookRotation(shootHit.normal));
                }
                else
                {
                    ResourceHelper.Instance.GetEffect(EffectType.SoilImpact, shootHit.transform, shootHit.point, Quaternion.LookRotation(shootHit.normal));
                }
                DrawShootToTarget(target);

            }
        }
        else
        {
            DrawShoot(direction);
        }
        PlayMuzzleEffect();
    }
    public Vector3 GetRandomDirectionInCone(Vector3 target,Vector3 start)
    {
        target.x+= Random.Range(-spread, spread);
        target.y += Random.Range(0, spread);
        target.z += Random.Range(-spread, spread);
        Vector3 dir = target - start;
        return dir.normalized;
    }
    public void DrawShootToTarget(Vector3 target)
    {
        GameObject bullet = ResourceHelper.Instance.GetBullet((int)weaponType);
        bullet.GetComponent<Projectile>().Init(muzzleTrans.position, target, true);
     
    }
    public void DrawShoot(Vector3 direction)
    {
      
        GameObject bullet = ResourceHelper.Instance.GetBullet((int)weaponType);
        bullet.GetComponent<Projectile>().Init(muzzleTrans.position, direction);
    }
   public virtual void PlayMuzzleEffect()
    {
        ResourceHelper.Instance.GetEffect(EffectType.SMGMuzzleFlash, muzzleTrans.position, muzzleTrans.rotation);
        int i = UnityEngine.Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[i]);
        casingFx.Emit(1);
    }
}
