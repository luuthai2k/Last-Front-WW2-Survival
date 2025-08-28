using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatePanelController : MonoBehaviour
{
    public GameObject[] goStars;
    public Button rateBtn;
    public CanvasGroup rateBtnCanvasGroup;
    public int currentRate;
    private void OnEnable()
    {
        FirebaseServiceController.Instance.LogEvent("POPUP_RATE_SHOW");
    }
    public void OnClickStarBtn(int index)
    {
        currentRate = index;
        rateBtn.interactable = true;
        rateBtnCanvasGroup.alpha = 1;
        for (int i = 0; i < goStars.Length; i++)
        {
            if (i <= index)
            {
                goStars[i].SetActive(true);
            }
            else
            {
                goStars[i].SetActive(false);
            }
        }

    }
    public void OnClickRateBtn()
    {
        if (currentRate == 4)
        {
            OpenStore();

        }
        ClosePopUp();

    }
    public void OnClickLaterBtn()
    {
        ClosePopUp();
    }
    public void OpenStore()
    {
        PlayerPrefs.SetInt("Rating", 5);
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id6748380911");
#endif
    }
    public void ClosePopUp()
    {
        PlayerPrefs.SetInt(GameConstain.AUTO_SHOW_RATE_POPUP, 1);
        gameObject.SetActive(false);
    }
}
