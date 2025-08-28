using UnityEngine;
using UnityEngine.EventSystems;


public class SoldierArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform soldierRotateTrans;
    public GameObject currentSoldier;
    public float rotationSpeed = 100f;
    public float returnSpeed = 5f;
    public Quaternion idleRotation;
    private Vector2 previousInputPosition;
    private bool isTouching = false;
    private float rotationY = 0f;

    public void LoadSoldier(string id)
    {
        if (currentSoldier!=null&&currentSoldier.name == id) return;
        Transform existingSoldier = soldierRotateTrans.Find(id);
        if (existingSoldier != null)
        {
            if (currentSoldier != null)
            {
                currentSoldier.SetActive(false);
            }

            currentSoldier = existingSoldier.gameObject;
            currentSoldier.SetActive(true);
            return;
        }
        LoadSoldierFromResources(id);
    }
    public void LoadSoldierFromResources(string id)
    {
        if (currentSoldier != null)
        {
            currentSoldier.SetActive(false);
        }
        currentSoldier = Instantiate(Resources.Load<GameObject>($"Soldier/View/{id}"), soldierRotateTrans);
        currentSoldier.name = id;
        currentSoldier.transform.localPosition = Vector3.zero;
        currentSoldier.transform.localRotation = Quaternion.identity;

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
            soldierRotateTrans.Rotate(Vector3.up, rotationY, Space.Self);
        }
        else
        {
            soldierRotateTrans.rotation = Quaternion.Lerp(soldierRotateTrans.rotation, idleRotation, Time.deltaTime * returnSpeed);
        }
    }
}
