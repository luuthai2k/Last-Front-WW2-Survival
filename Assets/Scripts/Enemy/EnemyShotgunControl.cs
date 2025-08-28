using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgunControl : EnemyBaseWeaponControl
{
    [SerializeField] private int bulletsPerShot = 8;
    int numberHitToPlayer;
    public override void Shooting(Vector3 target)
    {
        numberHitToPlayer = 0;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector3 direction = GetRandomDirectionInCone( target - muzzleTrans.position);
            if (Physics.Raycast(muzzleTrans.position, direction, out RaycastHit shootHit, 200f, LayerConfig.Instance.enemyRayCastMask))
            {
                if (shootHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (PlayerController.Instance.playerHealth.canTakeDame)
                    {
                        DrawShootToTarget(shootHit.point);
                        numberHitToPlayer++;
                    }
                    else
                    {
                        target.x += target.x > 0 ? safe : -safe;
                        DrawShoot(target - muzzleTrans.position);
                    }
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
        }
        PlayMuzzleEffect();
        if (numberHitToPlayer > 0)
        {
            PlayerController.Instance.playerHealth.TakeDamageBullet(Mathf.Max(1, damage*numberHitToPlayer / bulletsPerShot));
        }

    }
    public Vector3 GetRandomDirectionInCone(Vector3 forward)
    {
        float angleRad = spread * Mathf.Deg2Rad;

        Vector3 axis = Vector3.Cross(forward, Random.onUnitSphere).normalized;
        float randomAngle = Random.Range(0f, angleRad);
        Quaternion rot = Quaternion.AngleAxis(randomAngle * Mathf.Rad2Deg, axis);
        Vector3 dir = rot * forward;

        float twistAngle = Random.Range(0f, 360f);
        dir = Quaternion.AngleAxis(twistAngle, forward) * dir;

        return dir.normalized;
    }
    public override void PlayMuzzleEffect()
    {
        ResourceHelper.Instance.GetEffect(EffectType.ShotGunMuzzleFlash, muzzleTrans.position, muzzleTrans.rotation);
        int i = UnityEngine.Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[i]);
        casingFx.Emit(1);
    }
}
