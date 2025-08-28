using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using DG.Tweening;

public class WeaponArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform weaponRotateTrans;
    public Transform[] targetTrans;
    public GameObject currentWeapon,goLoading;
    public float rotationSpeed = 100f;
    public float returnSpeed = 5f;
    public Quaternion idleRotation;
    private Vector2 previousInputPosition;
    private bool isTouching = false,canRotation;
    private float rotationY = 0f;

    public void LoadWeapon(WeaponType weaponType, string id)
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }
        canRotation = false;
        weaponRotateTrans.rotation = Quaternion.identity;
        Transform target= targetTrans[(int)weaponType];
        //foreach (Transform child in target)
        //{
        //    if (child.gameObject.name == id)
        //    {
        //        if (currentWeapon != null)
        //        {
        //            currentWeapon.SetActive(false);
        //        }

        //        currentWeapon = child.gameObject;
        //        currentWeapon.SetActive(true);
        //        return;
        //    }
        //}
        Transform existingWeapon = target.Find(id);
        if (existingWeapon != null)
        {
            currentWeapon = existingWeapon.gameObject;
            currentWeapon.SetActive(true);
            canRotation = true;
            return;
        }
        LoadWeaponFromResource(target, id);
    }
    public async void LoadWeaponFromResource(Transform parent, string id)
    {
        parent.gameObject.SetActive(false);
        goLoading.SetActive(true);
        currentWeapon = Instantiate(Resources.Load<GameObject>($"Weapon/{id}"), parent);
        currentWeapon.name = id;
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        await Task.Delay(200);
        goLoading.SetActive(false);
        canRotation = true;
        parent.gameObject.SetActive(true);


    }
    public void OnPointerDown(PointerEventData eventData)
    {
#if UNITY_EDITOR
        previousInputPosition = Input.mousePosition;
#else
       if (Input.touchCount > 0)
            previousInputPosition = Input.GetTouch(0).position;
#endif

        isTouching = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;
    }
  
    void Update()
    {
        if (!canRotation) return;
        if (isTouching)
        {
            Vector2 currentInputPosition;

#if UNITY_EDITOR
            currentInputPosition = Input.mousePosition;
#else
            if (Input.touchCount == 0) return;
            currentInputPosition = Input.GetTouch(0).position;
#endif
            Vector2 delta = currentInputPosition - previousInputPosition;
            previousInputPosition = currentInputPosition;
            rotationY = -delta.x * rotationSpeed*Time.deltaTime;
            Debug.Log(rotationY);
            weaponRotateTrans.Rotate(Vector3.up, rotationY, Space.Self);
        }
        else
        {
            weaponRotateTrans.rotation = Quaternion.Lerp(weaponRotateTrans.rotation, idleRotation, Time.deltaTime * returnSpeed);
        }
    }
    public void PlayUpgradeFx()
    {
        weaponRotateTrans.DOScale(Vector3.one * 1.25f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            VibrationController.Instance.PlayLight();
            weaponRotateTrans.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
        });
    }
}
