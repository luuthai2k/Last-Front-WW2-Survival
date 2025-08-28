using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class BaseWeaponControl : MonoBehaviour
{
	protected WeaponInGameData data;
    public WeaponType type;
	public int indexIndata;
	public Transform muzzleTrans,ejectorTrans, leftHandIKTarget;
    public MagazinHandle magazinHandle;
    public float startTime=0.5f,spread = 0.1f;
    public int amountAmmo;
    public AudioClip[] audioClips;
    protected RaycastHit[] hitResults;
    public ParticleSystem casingFx;
 
    public virtual void Start()
    {
        data = DataController.Instance.GetCurrentWeaponInGameData(type);
        ReloadAmmo();
    }
    public virtual void ReloadAmmo()
    {
        amountAmmo = data.specification.magazine;
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnAmmoChange, new object[] { type, amountAmmo }));
    }
    public virtual bool Shooting(float handStability)
    {
        //if (CameraManager.Instance.IsAimTarget(LayerConfig.Instance.shootMask, out RaycastHit hit))
        //{
        //    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && hit.collider.GetComponent<BodyPartControl>().health.isDead) return false;
            Vector3 origin = muzzleTrans.position;
            Vector3 target = CameraManager.Instance.GetAimTargetPosition();
            Vector3 direction = GetRandomDirectionInCone(target - origin);
            Vector3 defaultOrigin = CameraManager.Instance.transform.position;
            Vector3 defaultDirection = GetRandomDirectionInCone(target - defaultOrigin);
            DrawShoot(direction);
            StartCoroutine(DelayCheckDame(CameraManager.Instance.GetAimHit(), defaultOrigin, defaultDirection, origin, direction));
            VibrationController.Instance.PlayLight();
            CameraManager.Instance.TriggerRecoilSMGImpulse();
            PlayerController.Instance.animatorController.Recoil((int)type);
            amountAmmo--;
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnAmmoChange, new object[] { type, amountAmmo }));
            return true;
        //}
        //return false;
    }
   public  Vector3 GetRandomDirectionInCone(Vector3 forward)
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

    public IEnumerator DelayCheckDame(RaycastHit defaultHit,Vector3 defaultOrigin,Vector3 defaultDirection, Vector3 origin, Vector3 direction)
    {
        yield return new WaitForSeconds(0.1f);
        int damage = data.specification.damage;
        if (Physics.Raycast(defaultOrigin, defaultDirection, out RaycastHit aimHit, 200, LayerConfig.Instance.rayCastMask))
        {
            if (defaultHit.collider == aimHit.collider)
            {
               
                if (defaultHit.collider.TryGetComponent(out ITakeDameBullet takeDameBullet))
                {
                    takeDameBullet.TakeDamageBullet(defaultHit.point, defaultHit.normal, direction, damage, 1000, out damage);
                }
                if (defaultHit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    if (defaultHit.collider.CompareTag("Metal"))
                    {
                        ResourceHelper.Instance.GetEffect(EffectType.MetalImpact, defaultHit.point, Quaternion.LookRotation(defaultHit.normal));
                    }
                    else if (defaultHit.collider.CompareTag("Wood"))
                    {
                        ResourceHelper.Instance.GetEffect(EffectType.WoodImpact, defaultHit.point, Quaternion.LookRotation(defaultHit.normal));
                    }
                    else if (defaultHit.collider.CompareTag("Stone"))
                    {
                        ResourceHelper.Instance.GetEffect(EffectType.StoneImpact, defaultHit.point, Quaternion.LookRotation(defaultHit.normal));
                    }
                    else
                    {
                        ResourceHelper.Instance.GetEffect(EffectType.SoilImpact, defaultHit.point, Quaternion.LookRotation(defaultHit.normal));
                    }

                }
                yield break;
            }
        }
        hitResults = new RaycastHit[5];
        int numberOfHits = Physics.SphereCastNonAlloc(origin,0.1f, direction.normalized, hitResults, 100, LayerConfig.Instance.rayCastMask);
        if (numberOfHits > 0)
        {
            RaycastHit[] validHits = new RaycastHit[numberOfHits];
            System.Array.Copy(hitResults, validHits, numberOfHits);
            System.Array.Sort(validHits, (a, b) => a.distance.CompareTo(b.distance));
           
            for (int i = 0; i < numberOfHits; i++)
            {
                if (validHits[i].collider.TryGetComponent(out ITakeDameBullet takeDameBullet))
                {
                    takeDameBullet.TakeDamageBullet(validHits[i].point, validHits[i].normal, direction, damage, 1000, out damage);
                    CrosshairControl.Instance.GetHitCrosshair();
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
                    else if (validHits[i].collider.CompareTag("Stone"))
                    {
                        ResourceHelper.Instance.GetEffect(EffectType.StoneImpact, validHits[i].point, Quaternion.LookRotation(validHits[i].normal));
                    }
                    else
                    {
                        ResourceHelper.Instance.GetEffect(EffectType.SoilImpact, validHits[i].point, Quaternion.LookRotation(validHits[i].normal));
                    }
                    damage = 0;

                }
                if (damage <= 0)
                {
                   yield break;
                }
            }
        }
    }
    public virtual void DrawShoot( Vector3 direction)
    {
        PlayMuzzleEffect();
        GameObject bulletShot = ResourceHelper.Instance.GetBullet(type);
        bulletShot.GetComponent<Projectile>().Init(muzzleTrans.position, direction);
        int i = UnityEngine.Random.Range(0, audioClips.Length);
		AudioController.Instance.PlaySfx(audioClips[i], 0.5f);
	}
	public virtual void PlayMuzzleEffect()
	{
        casingFx.Emit(1);
        ResourceHelper.Instance.GetEffect(EffectType.SMGMuzzleFlash, muzzleTrans.position, muzzleTrans.rotation);
    }
   public virtual float GetShootDelay()
    {
        return data.GetShootDelay();
    }
    public virtual float StartDelay()
    {
        return startTime;
    }
    public virtual void SetActiveScope(bool isActive)
    {
    }
    public virtual bool IsActiveScope()
    {
        return false;
    }
}
