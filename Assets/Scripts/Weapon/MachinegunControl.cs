using UnityEngine;
using UnityEngine.UI;
using InviGiant.Tools;
using System.Threading.Tasks;

public class MachinegunControl : BaseWeaponControl
{
    [SerializeField] public Transform gunAssemblyTrans;
    public override bool Shooting(float handStability)
    {
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

    }
    public override void PlayMuzzleEffect()
    {
        casingFx.Emit(1);
        ResourceHelper.Instance.GetEffect(EffectType.MachinegunMuzzleFlash, muzzleTrans.position, muzzleTrans.rotation);

    }
}
