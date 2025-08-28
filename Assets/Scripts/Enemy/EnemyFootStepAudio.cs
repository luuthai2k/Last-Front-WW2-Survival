using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootStepAudio : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public  void TriggerFootStep()
    {
        int i = Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[i]);
    }
}
