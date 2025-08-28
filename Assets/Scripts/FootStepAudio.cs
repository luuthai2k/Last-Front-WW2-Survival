using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepAudio : MonoBehaviour
{
    public AudioClip[] audioClips;
  
    public virtual void TriggerFootStep()
    {
        int i = Random.Range(0, audioClips.Length);
        AudioController.Instance.PlaySfx(audioClips[i],0.5f);
    }
}
