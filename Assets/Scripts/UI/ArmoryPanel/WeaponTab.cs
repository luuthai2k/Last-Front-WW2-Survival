using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTab : MonoBehaviour
{
    public GameObject goSelect, goNoti;
    public void SetActive(bool isActive)
    {
        goSelect.SetActive(isActive);
    }
    public void CheckNotification(int index)
    {
        Debug.LogError("CheckNotification");
        goNoti.SetActive(MainMenuUIManager.Instance.CanUpdateWeapon(index));
    }
}
