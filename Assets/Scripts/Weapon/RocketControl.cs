using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControl : BaseWeaponControl
{
    [SerializeField] public GameObject goRocketProjectile;
    public override void Start()
    {
        
    }
    public override void ReloadAmmo()
    {
        amountAmmo = 1;
        goRocketProjectile.gameObject.SetActive(true);
    }
    public override bool Shooting(float handStability)
    {
        goRocketProjectile.gameObject.SetActive(false);
        Vector3 target = CameraManager.Instance.GetAimTargetPosition();
        DrawShoot(target);
        VibrationController.Instance.PlayLight();
        CameraManager.Instance.TriggerRecoilRocketImpulse();
        PlayerController.Instance.animatorController.Recoil((int)type);
        amountAmmo--;
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnAmmoChange, new object[] { type, amountAmmo }));
        return true;
    }
    public override void DrawShoot(Vector3 target)
    {
        ResourceHelper.Instance.GetEffect(EffectType.RocketMuzzleFlash, muzzleTrans.position, muzzleTrans.rotation);
        GameObject bulletShot = ResourceHelper.Instance.GetBullet(WeaponType.Rocket);
        bulletShot.transform.parent = null;
        bulletShot.transform.position = goRocketProjectile.transform.position;
        bulletShot.transform.rotation = goRocketProjectile.transform.rotation;
        bulletShot.GetComponent<RocketProjectile>().Init( target);
        int i = UnityEngine.Random.Range(0, audioClips.Length);
        AudioController.Instance.PlaySfx(audioClips[i], 0.5f);
    }
    public override float GetShootDelay()
    {
        return 2f;
    }
    public override float StartDelay()
    {
        return 0.35f;
    }

}
