using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayParticleSystem : MonoBehaviour,IMessageHandle
{
    public ParticleSystem particleSystem;
    public bool onPlayOnAwake = true;
    public float startTimeDelay=2,timeDelayMin,timeDelayMax;
    public bool isLoop,isRandomPos;
    public Vector3 randomRange;
    private Vector3 defPos;
    public AudioClip[] audioClip;
    public AudioSource audioSource;
    public void Start()
    {
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnSceneLoaded, this);
    }
    public void Handle(Message message)
    {
        if (onPlayOnAwake)
        {
            defPos = particleSystem.transform.position;
            StartCoroutine(StartDelay());
        }
    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startTimeDelay);
        Play();
    }
    public void Play()
    {
        if (isRandomPos)
        {
            Vector3 randomPos = new Vector3(Random.Range(-randomRange.x, randomRange.x),Random.Range(-randomRange.y, randomRange.y),Random.Range(-randomRange.z, randomRange.z));
            particleSystem.transform.position = defPos + randomPos;
        }
        particleSystem.Play();
        if (audioClip.Length> 0)
        {
            if (audioSource != null)
            {
                if (AudioController.Instance.SFX)
                {
                    audioSource.PlayOneShot(audioClip[Random.Range(0, audioClip.Length)]);
                }
            }
            else
            {
                AudioController.Instance.PlaySfx(audioClip[Random.Range(0, audioClip.Length)]);

            }
        }
        StartCoroutine(DelayPLay());
    }
    IEnumerator DelayPLay()
    {
        yield return new WaitForSeconds(Random.Range(timeDelayMin, timeDelayMax));
        if (isLoop)
        {
            Play();
        }
      
    }
    public void Stop()
    {
        this.isLoop = false;
        particleSystem.Stop();
    }
    void OnDestroy()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnSceneLoaded, this);
    }
}

