using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolControl : MonoBehaviour
{
    public TMP_InputField inputFieldCash, inputFieldGold, inputFieldLevel;
    public GameObject goPanel;
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void OnClickOpenBtn() 
    {
        goPanel.SetActive(true);
    }
    public void OnClickOKBtn()
    {
        if(int.TryParse(inputFieldCash.text,out int cash))
        {
            DataController.Instance.Cash = cash;
        }
        if (int.TryParse(inputFieldGold.text, out int gold))
        {
            DataController.Instance.Gold = gold;
        }
        if (int.TryParse(inputFieldLevel.text, out int level))
        {
            DataController.Instance.Level = level;
        }
        goPanel.SetActive(false);

    }
}
