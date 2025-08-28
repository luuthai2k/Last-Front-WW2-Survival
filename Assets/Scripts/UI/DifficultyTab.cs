using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyTab : MonoBehaviour
{
    public GameObject goSelect, goLock,goBtn;
    public void Select(bool isSelect)
    {
        goSelect.SetActive(isSelect);
    }
    public void Lock(bool isLock)
    {
        goLock.SetActive(isLock);
        goBtn.SetActive(!isLock);
    }
}
