using UnityEngine;

public class PlayerSwapHandle : MonoBehaviour
{
    public PlayerIK playerIK;
    public Transform LeftParentTrans, RightGunParentTrans;
    public BaseWeaponControl LeftWeaponControl, RightWeaponControl;
    public void Start()
    {
        
    }
    public void SpawmWeapon(WeaponInGameData data)
    {
        GameObject goWeapon = Instantiate(Resources.Load<GameObject>($"Weapon/{data.ID}"), RightGunParentTrans);
        goWeapon.transform.localPosition = Vector3.zero;
        goWeapon.transform.localRotation = Quaternion.identity;
        RightWeaponControl = goWeapon.GetComponent<BaseWeaponControl>();
    }
    public void OnHolsterStart()
    {
        playerIK.LeftHandIKWeight(0, 0f);
    }
    public void OnHolster()
    {
        var weapon = PlayerController.Instance.playerShootController.GetCurrentWeaponControl();
        if (LeftWeaponControl==null||weapon == LeftWeaponControl)
        {
            weapon.transform.parent = LeftParentTrans;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            LeftWeaponControl = weapon;
            //PlayerController.Instance.playerShootController.SetCurrentWeaponControl(RightWeaponControl);
            //playerIK.SetLK(RightWeaponControl.leftHandIKTarget);
        }
        else if(RightWeaponControl==null||weapon == RightWeaponControl)
        {
            weapon.transform.parent = RightGunParentTrans;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            RightWeaponControl = weapon;
            //PlayerController.Instance.playerShootController.SetCurrentWeaponControl(LeftWeaponControl);
            //playerIK.SetLK(LeftWeaponControl.leftHandIKTarget);
        }
    }
    public void SwapWeapon()
    {
        var weapon = PlayerController.Instance.playerShootController.GetCurrentWeaponControl();
        if(LeftWeaponControl != null&&weapon != LeftWeaponControl)
        {
            PlayerController.Instance.playerShootController.SetCurrentWeaponControl(LeftWeaponControl);
            playerIK.SetLK(LeftWeaponControl.leftHandIKTarget);
        }
        if (RightWeaponControl != null && weapon != RightWeaponControl)
        {
            PlayerController.Instance.playerShootController.SetCurrentWeaponControl(RightWeaponControl);
            playerIK.SetLK(RightWeaponControl.leftHandIKTarget);
        }
    }
    public void OnEquip()
    {
        PlayerController.Instance.playerShootController.ResetCurrentWeaponPoint();
    }
    public void OnEquipEnd()
    {
        playerIK.LeftHandIKWeight(1, 0.5f);
        PlayerController.Instance.OnEndSwap();
    }
}
