using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoFitImage : MonoBehaviour
{
    private void OnEnable()
    {
        var img = GetComponent<Image>();
        var rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0.5f, 0);
    }
}
