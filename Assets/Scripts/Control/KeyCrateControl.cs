using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCrateControl : MonoBehaviour
{
    public int id;
    public void Start()
    {
        if (PlayerPrefs.GetInt($"Key_Crate_{DataController.Instance.Level}_{id}", 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }
    public void TakeKey()
    {
        PlayerPrefs.SetInt($"Key_Crate_{DataController.Instance.Level}_{id}", 1);
        DataController.Instance.AddKey(1);
        UIManager.Instance.SpawmKeyEffect();
    }
}
