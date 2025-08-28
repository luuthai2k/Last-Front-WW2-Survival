using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlaySFX : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;

    void Start()
    {
        if (AudioController.Instance.SFX)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
