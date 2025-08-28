using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactiveTutorial : MonoBehaviour
{
    public GameObject goRoot;
    public float delayTime=0.1f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delayTime);
        goRoot.SetActive(false);
    }
}
