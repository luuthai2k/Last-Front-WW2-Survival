using UnityEngine;
using System.Collections.Generic;
using System;
public class SafeArea : MonoBehaviour
{
    //[SerializeField] private bool shouldMoveBottom = true;
    private Rect safeArea;
    private Vector2 anchorMin, anchorMax;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        safeArea = Screen.safeArea;
        ConvertToScreen();
        Rect ratio169 = new Rect(0, 0, 1080, 1920);
        float currentAspect = ((float)Screen.width) / ((float)Screen.height);
        float defaultAspect = ratio169.width / ratio169.height;
        if (anchorMin.x == 0 && anchorMin.y == 0 && anchorMax.x == 1 && anchorMax.y == 1 && currentAspect < defaultAspect)
        {
            float heightConvert169, heightDelta;
            float delta = ratio169.width / (float)Screen.width;
            float delta1 = (float)Screen.width / ratio169.width;
            heightConvert169 = ((float)Screen.height * delta);
            heightDelta = (heightConvert169 - ratio169.height) * delta1;
            safeArea = new Rect(0, heightDelta / 4, (float)Screen.width, (float)Screen.height - heightDelta / 2);
            ConvertToScreen();
        }
    }
    private void ConvertToScreen()
    {
        anchorMin = safeArea.position;
        anchorMax = anchorMin + safeArea.size;
        anchorMin.x /= Screen.width;
        //if (shouldMoveBottom)
        anchorMin.y /= Screen.height;
        //else
        //    anchorMin.y = 0;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
