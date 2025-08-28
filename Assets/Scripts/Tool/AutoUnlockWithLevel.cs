using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUnlockWithLevel : MonoBehaviour
{
    public int levelRequire;
    void Start()
    {
        if (DataController.Instance.Level < levelRequire)
        {
            gameObject.SetActive(false);
        }
    }
}
