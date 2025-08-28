using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxSource : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    public bool playOnAwake=true;
    void OnEnable()
    {
        if (playOnAwake)
        {
            audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }

   
}
