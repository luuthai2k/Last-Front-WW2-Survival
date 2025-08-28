using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirdropBtn : MonoBehaviour
{
    public Image[] keyIcons;
    public void Start()
    {
        SetUp();
    }
    public void SetUp()
    {
        for (int i = 0; i < keyIcons.Length; i++)
        {
            if (i <= DataController.Instance.Key - 1)
            {
                keyIcons[i].color = Color.white;
            }
            else
            {
                keyIcons[i].color = Color.black;
            }
        }
    }
    public void OnClickBtn()
    {
        if (DataController.Instance.Key >= 3)
        {
            UIManager.Instance.SpawnAirdropPanel();
        }
        else
        {
            UIManager.Instance.SendNoti("You_need_more_keys");
        }
    }
}
