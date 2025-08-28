using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguegePanelController : MonoBehaviour
{
    public GameObject[] goSelects;
    public void Start()
    {
        ToggleLanguage(LocalizationManager.Instance.localizationId);
    }
    public void OnClickSelectLanguageBtn(int id)
    {
        if (LocalizationManager.Instance.localizationId == id) return;
        ToggleLanguage(id);
    }
    public void ToggleLanguage(int id)
    {
        LocalizationManager.Instance.SetLanguege(id);
        for(int i = 0; i < goSelects.Length; i++)
        {
            if (i == id)
            {
                goSelects[i].SetActive(true);
            }
            else
            {
                goSelects[i].SetActive(false);
            }
        }
    }
    public void OnClickCloseBtn()
    {
        gameObject.SetActive(false);
    }
}
