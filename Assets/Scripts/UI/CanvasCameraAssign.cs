using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CanvasCameraAssign : MonoBehaviour
{
    public bool shouldOverrideSorting = false;
    public string sortingLayerName = "Default";
    public int sortingOrder = 1, planeDistance = 5;
    public Camera cameraForcus;
    private void OnEnable()
    {
        if (cameraForcus == null)
        {
            cameraForcus = Camera.main;
        }
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = cameraForcus;
        canvas.planeDistance = planeDistance;
        if (shouldOverrideSorting)
        {
            canvas.sortingLayerName = sortingLayerName;
            canvas.sortingOrder = sortingOrder;
        }
    }
}
