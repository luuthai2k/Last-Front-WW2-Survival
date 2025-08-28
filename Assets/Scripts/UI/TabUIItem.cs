using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabUIItem : MonoBehaviour
{
    public GameObject goSelect;
    public bool isSelect;
   
    public void OnClickBtn()
    {
        if (isSelect) return;
        isSelect = true;
        goSelect.SetActive(true);
    }
    public void DeSelect()
    {
        isSelect = false;
        goSelect.SetActive(false);
    }
}
