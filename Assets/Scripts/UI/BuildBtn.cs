using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildBtn : MonoBehaviour, IMessageHandle
{
    public Image fillAmount;
    public TextMeshProUGUI metalTxt;
    public GameObject goNotification;
    void OnEnable()
    {
        if (DataController.Instance.Level < 7)
        {
            gameObject.SetActive(false);
            return;
        }
        SetUp();
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnMetalChange, this);
    }
    public void Handle(Message message)
    {
        SetUp();
    }
    public void SetUp()
    {
        var bpigd = DataController.Instance.GetBuildProcessInGameData();
        metalTxt.text = bpigd.cost.ToString();
        fillAmount.fillAmount = (float)bpigd.process / bpigd.cost;
        if(DataController.Instance.Metal>= (bpigd.cost- bpigd.process))
        {
            goNotification.gameObject.SetActive(true);
        }
        else
        {
            goNotification.SetActive(false);
        }
    }
    public void OnClickBtn()
    {
        MainMenuUIManager.Instance.OnShowBuildPanel();
    }
    void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnMetalChange, this);
    }
}
