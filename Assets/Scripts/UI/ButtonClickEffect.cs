using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickEffect : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip btnClickAudio;
    public bool isPunchScale=true;
    public bool isVibration=true;
    Vector3 baseScale;
    private void Start()
    {
        baseScale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPunchScale)
        {
            transform.localScale = new Vector3(baseScale.x - 0.1f, baseScale.y - 0.1f, baseScale.z - 0.1f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPunchScale)
        {
            transform.localScale = baseScale;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (btnClickAudio != null)
        {
            AudioController.Instance.PlaySfx(btnClickAudio);
        }
        if (isVibration)
        {
            VibrationController.Instance.PlayLight();
        }
           
    }
}
