using UnityEngine;

public class SoldierView : MonoBehaviour
{
    public Animator animator;
    public Transform weaponTrans;
    public Transform leftHandIKTarget;
    public GameObject goWeapon;

    private void OnEnable ()
    {
        LoadWeaponFromResource();
    }
    public void LoadWeaponFromResource()
    {
        string weaponID = DataController.Instance.GetCurrentWeaponInGameData(WeaponType.SMG).ID;
        if (goWeapon != null && goWeapon.name == weaponID) return;
        if (goWeapon != null)
        {
            goWeapon.SetActive(false);
        }
        Transform existingWeapon = weaponTrans.Find(weaponID);
        if (existingWeapon != null)
        {
            goWeapon = existingWeapon.gameObject;
            goWeapon.SetActive(true);
            leftHandIKTarget = goWeapon.GetComponent<BaseWeaponControl>().leftHandIKTarget;
            return;
        }
        GameObject goNewWeapon = Instantiate(Resources.Load<GameObject>($"Weapon/{weaponID}"), weaponTrans);
        goWeapon = goNewWeapon;
        goWeapon.transform.localPosition = Vector3.zero;
        goWeapon.transform.localRotation = Quaternion.identity;
        goWeapon.name = weaponID;
        leftHandIKTarget = goWeapon.GetComponent<BaseWeaponControl>().leftHandIKTarget;


    }

    private void OnAnimatorIK(int layerIndex)
    {

        if (leftHandIKTarget != null)
        {
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        }
        
    }

}
