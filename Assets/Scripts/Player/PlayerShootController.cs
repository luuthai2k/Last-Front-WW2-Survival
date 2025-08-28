using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class PlayerShootController : MonoBehaviour
{
    public BaseWeaponControl currentWeaponControl;
    public BaseWeaponControl mainWeaponControl;
    public Transform weaponContainerTrans;
    public LayerMask rayCastMask, shootMask;
   [SerializeField] private float _shootDelay;
    public float handStability = 50f;
    private void Update()
    {
        if (_shootDelay > 0)
        {
            _shootDelay -= Time.deltaTime;
        }
    }
    public void SpawmMainWeapon(string id)
    {
        GameObject goWeapon = Instantiate(Resources.Load<GameObject>($"Weapon/{id}"), weaponContainerTrans);
        goWeapon.transform.localPosition = Vector3.zero;
        goWeapon.transform.localRotation = Quaternion.identity;
        mainWeaponControl = goWeapon.GetComponent<BaseWeaponControl>();
        currentWeaponControl = mainWeaponControl;
       
    }
    public void ChangeMainWeapon(string id)
    {

        Debug.LogError(id);
        GameObject goWeapon = Instantiate(Resources.Load<GameObject>($"Weapon/{id}"), weaponContainerTrans);
        goWeapon.transform.localPosition = Vector3.zero;
        goWeapon.transform.localRotation = Quaternion.identity;
        mainWeaponControl.gameObject.SetActive(false);
        mainWeaponControl = goWeapon.GetComponent<BaseWeaponControl>();
        if (!currentWeaponControl.gameObject.activeSelf)
        {
            currentWeaponControl = mainWeaponControl;
        }
    }
  
    public void SetWeaponControl(BaseWeaponControl weaponControl,bool activeWeapon=false)
    {
        if (currentWeaponControl != null)
        {
            currentWeaponControl.gameObject.SetActive(activeWeapon);
        }
        Debug.Log(currentWeaponControl.gameObject.name);
        currentWeaponControl = weaponControl;
    }
    public void ReloadAmmo()
    {
        currentWeaponControl.ReloadAmmo();
    }

    public void ShootDelay(WeaponType weaponType)
    {
        if (_shootDelay <= 0)
        {
            if (CheckReLoad()) return;
            if (currentWeaponControl.Shooting(CameraManager.Instance.GetStability() / handStability))
            {
                CrosshairControl.Instance.OnShoot();
                _shootDelay = currentWeaponControl.GetShootDelay();
                CheckReLoad();
                if (weaponType != WeaponType.Rocket)
                {
                    MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnShoot));
                }
            }
            //currentWeaponControl.Shooting(CameraManager.Instance.GetStability() / handStability);
            
        }
    }
   public bool CheckReLoad()
    {
        if (currentWeaponControl.amountAmmo <= 0)
        {
            PlayerUIControl.Instance.OnClickReloadBtn();
            return true;
        }
        return false;
    }
  public bool IsCanShoot()
    {
        return currentWeaponControl.amountAmmo > 0;
    }
    public float GetStartDelayWeaponCanShoot()
    {
        return currentWeaponControl.StartDelay();
    }
    public void ChangeParentMainWeapon(Transform parent)
    {
        mainWeaponControl.transform.parent = parent;
    }
    public void ChangeParentMainWeapon(Transform parent,Vector3 localPosition, Vector3 localRotation,float duration) 
    {
        mainWeaponControl.transform.parent = parent;
        mainWeaponControl.transform.DOLocalMove(localPosition, duration);
        mainWeaponControl.transform.DOLocalRotate(localRotation, duration);
    }
    public BaseWeaponControl GetCurrentWeaponControl()
    {
        return currentWeaponControl;
    }
    public void SetCurrentWeaponControl(BaseWeaponControl weaponControl)
    {
        currentWeaponControl=weaponControl;
    }
    public void ResetCurrentWeaponPoint()
    {
        currentWeaponControl.transform.parent = weaponContainerTrans;
        currentWeaponControl.transform.localPosition = Vector3.zero;
        currentWeaponControl.transform.localRotation = Quaternion.identity;
    }

}
