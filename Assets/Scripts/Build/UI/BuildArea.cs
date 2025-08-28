using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using DG.Tweening;

public class BuildArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform buildTrans;
    public GameObject goCurrentBuild,goLoading;
    public float rotationSpeed = 100f;
    public float returnSpeed = 5f;
    public Quaternion idleRotation;
    private Vector2 previousInputPosition;
    private bool isTouching = false, canRotation;
    private float rotationY = 0f;
    public float rotationRange = 15f;
    float angleOffset;
    public async Task<GameObject> LoadBuild(int index)
    {
        if (goCurrentBuild != null)
        {
            goCurrentBuild.gameObject.SetActive(false);
        }

        canRotation = false;
        buildTrans.rotation = Quaternion.identity;

        //Transform existingWeapon = buildTrans.Find($"Build_{index}");
        //if (existingWeapon != null)
        //{
        //    goCurrentBuild = existingWeapon.gameObject;
        //    goCurrentBuild.gameObject.SetActive(true);
        //    canRotation = true;
        //    return goCurrentBuild;
        //}

        return await LoadBuildFromAdressable(index);
    }
    public async Task<GameObject> LoadBuildFromResource(int index)
    {
        buildTrans.gameObject.SetActive(false);
        goLoading.SetActive(true);
        goCurrentBuild = Instantiate(Resources.Load<GameObject>($"Build/Build_{index}"), buildTrans);
        goCurrentBuild.name = $"Build_{index}";
        goCurrentBuild.transform.localPosition = Vector3.zero;
        goCurrentBuild.transform.localRotation = Quaternion.identity;
        await Task.Delay(100);
        canRotation = true;
        buildTrans.gameObject.SetActive(true);
        goLoading.SetActive(false);
        return goCurrentBuild;
    }
    public async Task<GameObject> LoadBuildFromAdressable(int index)
    {
        buildTrans.gameObject.SetActive(false);
        goLoading.SetActive(true);

        var handle = Addressables.LoadAssetAsync<GameObject>($"Assets/Build/Build_{index}.prefab");
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
        {
            Debug.LogError($"Load fail: {index}");
            return null;
        }
        goCurrentBuild = Instantiate(handle.Result, buildTrans);
        goCurrentBuild.name = $"Build_{index}";
        goCurrentBuild.transform.localPosition = Vector3.zero;
        goCurrentBuild.transform.localRotation = Quaternion.identity;
        await Task.Delay(200);
        canRotation = true;
        buildTrans.gameObject.SetActive(true);
        goLoading.SetActive(false);
        return goCurrentBuild;
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
        angleOffset = 0;
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
            rotationY = -delta.x * rotationSpeed * Time.deltaTime;
            Debug.Log(rotationY);
            buildTrans.Rotate(Vector3.up, rotationY, Space.Self);
          
        }
        else
        {
            angleOffset = Mathf.PingPong(Time.time*2 , rotationRange * 2) - rotationRange;
            buildTrans.rotation = Quaternion.Lerp(buildTrans.rotation,Quaternion.Euler(idleRotation.x, idleRotation.y+angleOffset, idleRotation.z), Time.deltaTime * returnSpeed);
        }
    }
}
