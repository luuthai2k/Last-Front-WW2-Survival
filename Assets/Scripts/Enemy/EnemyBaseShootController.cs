using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseShootController : MonoBehaviour
{
    public EnemyBaseWeaponControl weaponControl;
    public float startDelay,fireRate;
    public Transform weaponContainerTrans;
    public void Init(WeaponType weaponType,int damage, float fireRate)
    {
        weaponControl = ResourceHelper.Instance.GetEnemyWeapon(weaponType,weaponContainerTrans).GetComponent<EnemyBaseWeaponControl>();
        weaponControl.transform.localPosition = Vector3.zero;
        weaponControl.transform.localRotation = Quaternion.identity;
        weaponControl.damage = damage;
        this.fireRate = fireRate;
    }
    public void Shoot()
    {
        Vector3 playerFixedPos = PlayerController.Instance.GetHeadTargetTrans().position;
        weaponControl.Shooting(playerFixedPos);
    }
    public void DropWeapon()
    {
        weaponControl.GetComponent<DropObject>().Drop();
    }
}

