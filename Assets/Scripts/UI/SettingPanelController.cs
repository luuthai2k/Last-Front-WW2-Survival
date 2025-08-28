using Samples.Purchasing.Core.IAPManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingPanelController : MonoBehaviour
{
    public GameObject goMusicOn, goMusicOff, goSoundOn, goSoundOff, goVibrationOn, goVibrationOff;
    public TextMeshProUGUI versionTxt;
    public GameObject goLanguagePanel;
    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay") return;
        ManagerAds.ins.ShowMrec();
    }
  
    public void Start()
    {
        goMusicOn.SetActive(AudioController.Instance.Music);
        goMusicOff.SetActive(!AudioController.Instance.Music);
        goSoundOn.SetActive(AudioController.Instance.SFX);
        goSoundOff.SetActive(!AudioController.Instance.SFX);
        goVibrationOn.SetActive(VibrationController.Instance.VIBRATION);
        goVibrationOff.SetActive(!VibrationController.Instance.VIBRATION);
        versionTxt.text =string.Format("Ver {0}",Application.version);
       
    }
    public void ToggleMusic()
    {
        AudioController.Instance.Music = !AudioController.Instance.Music;
        goMusicOn.SetActive(AudioController.Instance.Music);
        goMusicOff.SetActive(!AudioController.Instance.Music);
    }
    public void ToggleSound()
    {
        AudioController.Instance.SFX = !AudioController.Instance.SFX;
        goSoundOn.SetActive(AudioController.Instance.SFX);
        goSoundOff.SetActive(!AudioController.Instance.SFX);
    }
    public void ToggleVibration()
    {
        VibrationController.Instance.VIBRATION = !VibrationController.Instance.VIBRATION;
        goVibrationOn.SetActive(VibrationController.Instance.VIBRATION);
        goVibrationOff.SetActive(!VibrationController.Instance.VIBRATION);
    }
    public void OnClickCloseBtn()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnClickLanguageBtn()
    {
        goLanguagePanel.SetActive(true);
        goLanguagePanel.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
        {
            gameObject.SetActive(true);
        });
        gameObject.SetActive(false);
    }
    public void OnClickMainMenuBtn()
    {
        ManagerAds.ins.ShowInterside();
        SceneController.Instance.LoadScene(GameConstain.MainMenu, "Main_menu_loading");
        OnClickCloseBtn();
    }
    public void OnClickRestoreBtn()
    {
        IAPManager.Instance.RestorePurchases();
    }
    private void OnDisable()
    {
        ManagerAds.ins.HideMrec();
    }
}
