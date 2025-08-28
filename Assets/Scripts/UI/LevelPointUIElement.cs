using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPointUIElement : MonoBehaviour
{
    public int level;
    public GameObject goBtn, goLock,goSelect;
    public void Start()
    {
        if (DataController.Instance.Level < level)
        {
            goBtn.SetActive(false);
            goLock.SetActive(true);
        }
        else
        {

            goBtn.SetActive(true);
            goLock.SetActive(false);
        }
        if (DataController.Instance.Level == level)
        {
            goSelect.SetActive(true);
        }

    }
    public void OnClickBtn()
    {

    }
    public void Select(bool isSelect)
    {
        goSelect.SetActive(isSelect);
    }
}
