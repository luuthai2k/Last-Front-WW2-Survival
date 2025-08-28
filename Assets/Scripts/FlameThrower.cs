using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public ParticleSystem fx;
    public int damage = 10;
    public float fireRate = 1;
    public AudioSource audioSource;
    private bool canTakeDame = true;
    public void StartShoot()
    {
        fx.Play();
        if (AudioController.Instance.SFX)
        {
            audioSource.Play();
            audioSource.DOFade(1,1f);
        }
    }
    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (!canTakeDame||PlayerController.Instance.currentState != PlayerState.Shoot) return;
        if (other.CompareTag("Player"))
        {
            canTakeDame = false;
            PlayerController.Instance.playerHealth.TakeDamageFlameThrower(damage);
            StartCoroutine(DelayCanTakeDame());
        }
    }
    IEnumerator DelayCanTakeDame()
    {
        yield return new WaitForSeconds(1f / fireRate);
        canTakeDame = true;
    }
    public void FinishShoot()
    {
        fx.Stop();
        audioSource.DOFade(0, 1f).OnComplete(() =>
        {
            audioSource.Stop();
        });
    }

}
