using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScaleCanvas : MonoBehaviour
{
    public CanvasScaler canvasScaler; 

    private void Start()
    {
        if (canvasScaler == null)
        {
            canvasScaler = GetComponent<CanvasScaler>();
            if (canvasScaler == null)
            {
                Debug.LogError("CanvasScaler component not found!");
                return;
            }
        }
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        Vector2 referenceResolution = new Vector2(1080, 1920); 
        canvasScaler.referenceResolution = referenceResolution; 
        float currentAspect = ((float)Screen.width) / ((float)Screen.height);
        float defaultAspect = referenceResolution.x / referenceResolution.y;
        if (currentAspect > defaultAspect)
        {
            canvasScaler.matchWidthOrHeight = 1f; 
          
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0f; // Khớp chiều rộng (Width)
           
        }
    }

}
