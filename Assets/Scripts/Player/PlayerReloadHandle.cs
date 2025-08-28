using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerReloadHandle : MonoBehaviour
{
    public PlayerIK playerIK;
    public Transform magazinTarget;
    public MagazinHandle magazinHandle;
    public Transform reloadLeftHandLkTarget;
    public void ReloadStart()
    {
        Debug.Log("ReloadEnd");
        magazinHandle = PlayerController.Instance.playerShootController.currentWeaponControl.magazinHandle;
        playerIK.LeftHandIKWeight(0, 0.1f);
    }
    public void ReloadDump()
    {
       
            magazinHandle?.OnDump();
       
    }
    public void ReloadTake()
    {
        magazinHandle?.OnTake(magazinTarget);

    }
    public void ReloadPut()
    {
        magazinHandle?.OnPut();
    }
    public void ReloadEnd()
    {
        playerIK.LeftHandIKWeight(1, 0.35f);
        PlayerController.Instance.playerShootController.ReloadAmmo();
    }
}
