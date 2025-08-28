using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildStep : MonoBehaviour
{
    public float startHeight, height;
    public Renderer[] targetRenderers;
    public UnityEvent onFilled;
    public Transform snapTarget;

    public void UpdateProcess(float process)
    {
        for(int i=0;i< targetRenderers.Length; i++)
        {
            Material[] material = targetRenderers[i].materials;
            float fillRate = startHeight + (process * height);
            for (int j = 0; j < material.Length; j++)
            {
                material[j].SetFloat("_FillRate", fillRate);
            }
            if (process >= 1)
            {
                OnComplete();
            }
        }
       
      
    }
    public void OnComplete()
    {
        Debug.LogWarning(gameObject.name);
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            Material[] material = targetRenderers[i].materials;
            for (int j = 0; j < material.Length; j++)
            {
                material[j].SetFloat("_FillRate", startHeight + height*2);
            }
            onFilled?.Invoke();

        }
    }
}
