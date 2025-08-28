using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SenCam : MonoBehaviour
{
    public Slider slider;
    public Slider sliderShoot;
    public TextMeshProUGUI senTxt,senshootTxt;
   
    private void Update()
    {
        senTxt.text = slider.value.ToString();
        senshootTxt.text = sliderShoot.value.ToString();
        if (CameraManager.Instance.currentVirtualCamnera==null)return;
        //CameraManager.Instance.currentVirtualCamnera.horizontalSpeed = slider.value;
        //CameraManager.Instance.currentVirtualCamnera.verticalSpeed = slider.value;
        //CameraManager.Instance.currentVirtualCamnera.horizontalShootSpeed = sliderShoot.value;
        //CameraManager.Instance.currentVirtualCamnera.verticalShootSpeed = sliderShoot.value;
    }

}
