using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengingMissionPanelController : MonoBehaviour
{
    public TextMeshProUGUI dameRequireTxt;
    public Image weaponImg;
    public void SetUp(Sprite weaponIcon, int dameRequire)
    {
        weaponImg.sprite = weaponIcon;
        dameRequireTxt.text = dameRequire.ToString();
    }
    public void OnClickUpgradeBtn()
    {
        MainMenuUIManager.Instance.OnSelectArmoryTab();
        gameObject.SetActive(false);
    }
    public void OnClickStartMissionBtn()
    {
        MainMenuUIManager.Instance.Play();
        gameObject.SetActive(false);
    }
}
