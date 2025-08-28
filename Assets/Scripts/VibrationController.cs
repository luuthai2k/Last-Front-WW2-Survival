using CandyCoded.HapticFeedback;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationController : Singleton<VibrationController>
{
    [SerializeField] private bool canVibrate;
    public bool VIBRATION
    {
        set
        {
            PlayerPrefs.SetInt("vibration", value ? 1 : 0);
            canVibrate = value;
        }
        get { return PlayerPrefs.GetInt("vibration", 1) == 1; }
    }

    private void Start()
    {
        LoadVibrationSettings();
    }
    private void LoadVibrationSettings()
    {
        canVibrate = PlayerPrefs.GetInt("vibration", 1) == 1;
    }
    public void PlayMedium()
    {
        if (!canVibrate) return;
        HapticFeedback.MediumFeedback();
       
    }
    public void PlayLight()
    {
        if (!canVibrate) return;
        HapticFeedback.LightFeedback();

    }
    public void PlayHeavy()
    {
        if (!canVibrate) return;
        HapticFeedback.HeavyFeedback();

    }
    public void ToggleVibration(bool canVibration)
    {
        canVibrate = canVibration;
        PlayerPrefs.SetInt("vibration", canVibrate ? 1 : 0);
    }
   
}
