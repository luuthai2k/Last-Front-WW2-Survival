using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainGunControl : MonoBehaviour
{
    public Transform barrelTrans;
    public float startDelay, fireRate;
    public Transform muzzleTrans;
    public AudioClip[] audioClips;
    public float spread = 1f, safe = 1;
    public int damage = 10;
    public AudioSource audioSource;
    public void Init(int damage, float fireRate)
    {
        this.damage = damage;
        this.fireRate = fireRate;
    }
    public virtual void Shooting(Vector3 target)
    {
        Vector3 spreadOffset = Vector3.zero;
        spreadOffset.x = UnityEngine.Random.Range(-spread, spread);
        spreadOffset.y = UnityEngine.Random.Range(-spread, spread);
        target += spreadOffset;
        Vector3 direction = target - muzzleTrans.position;
        if (Physics.Raycast(muzzleTrans.position, direction, out RaycastHit shootHit, 200f, LayerConfig.Instance.enemyRayCastMask))
        {
            if (shootHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (PlayerController.Instance.currentState == PlayerState.Shoot)
                {
                    PlayerController.Instance.playerHealth.TakeDamageTank(damage);
                    DrawShootToTarget(target);
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
                    ResourceHelper.Instance.GetEffect(EffectType.MetalImpact, shootHit.point, Quaternion.LookRotation(shootHit.normal));
                }
                else if (shootHit.collider.CompareTag("Wood"))
                {
                    ResourceHelper.Instance.GetEffect(EffectType.WoodImpact, shootHit.point, Quaternion.LookRotation(shootHit.normal));
                }
                else if (shootHit.collider.CompareTag("Stone"))
                {
                    ResourceHelper.Instance.GetEffect(EffectType.StoneImpact, shootHit.point, Quaternion.LookRotation(shootHit.normal));
                }
                else
                {
                    ResourceHelper.Instance.GetEffect(EffectType.SoilImpact, shootHit.point, Quaternion.LookRotation(shootHit.normal));
                }
                DrawShootToTarget(target);

            }

        }
        else
        {
            DrawShoot(direction);
        }

    }

    void DrawShootToTarget(Vector3 target)
    {
        PlayMuzzleEffect();
        GameObject bullet = ResourceHelper.Instance.GetBullet(WeaponType.Machinegun);
        bullet.GetComponent<Projectile>().Init(muzzleTrans.position, target, true);
        int i = UnityEngine.Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[i]);
    }
    void DrawShoot(Vector3 direction)
    {
        PlayMuzzleEffect();
        GameObject bullet = ResourceHelper.Instance.GetBullet(WeaponType.Machinegun);
        bullet.GetComponent<Projectile>().Init(muzzleTrans.position, direction);
        int i = UnityEngine.Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[i]);
    }
    void PlayMuzzleEffect()
    {
        ResourceHelper.Instance.GetEffect(EffectType.MachinegunMuzzleFlash, muzzleTrans.position, muzzleTrans.rotation);
    }

    public void LookAtTarget()
    {

        Vector3 directionToTarget = PlayerController.Instance.GetHeadTargetTrans().position - barrelTrans.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        float targetXAngle = targetRotation.eulerAngles.x;
        if (targetXAngle > 180f)
        {
            targetXAngle -= 360f;
        }
        barrelTrans.transform.DOLocalRotateQuaternion(Quaternion.Euler(targetXAngle, 0f, 0f),1f);
    }

}
