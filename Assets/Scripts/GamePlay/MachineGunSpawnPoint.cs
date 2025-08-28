using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunSpawnPoint : MonoBehaviour
{
    private MachinegunControl machinegunControl;
    private void Start()
    {
        SpawnMachineGun();
    }
    public void SpawnMachineGun()
    {
       
        string id = DataController.Instance.GetCurrentWeaponInGameData(WeaponType.Machinegun).ID;
        Debug.LogError(id + "jhsdvbfjlikeb");
        GameObject goWeapon = Instantiate(Resources.Load<GameObject>($"Weapon/{id}"), transform);
        goWeapon.transform.position = transform.position;
        goWeapon.transform.rotation = transform.rotation;
        machinegunControl = goWeapon.GetComponent<MachinegunControl>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController.Instance.ChangeToStateMachinegun(machinegunControl);
        }
    }
}
