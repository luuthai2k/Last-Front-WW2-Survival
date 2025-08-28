using DG.Tweening;
using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    public int damage = 100;
    public float startDelay = 2, rad = 20, lifeTime = 60,duration=1,currentLifeTime;
    float currentRad;
    public ParticleSystem particle;
    private void OnEnable()
    {
        if (particle != null)
        {
            var main = particle.main;
            main.duration = lifeTime-5;
            main.startDelay = startDelay;
            particle.Play();
        }
        currentLifeTime = lifeTime;
        StartCoroutine(DelayCheckDamePoison());
    }
    IEnumerator DelayCheckDamePoison()
    {
        yield return new WaitForSeconds(startDelay);
        DOTween.To(() => 0, x => currentRad = x, rad, 5).SetEase(Ease.OutSine);
        while (currentLifeTime>0)
        {
            CheckDameExplosion();
            currentLifeTime--;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(5f);
        SmartPool.Instance.Despawn(gameObject);
    }
    private void CheckDameExplosion()
    {
        Collider[] hitResults = Physics.OverlapSphere(transform.position, currentRad, LayerConfig.Instance.poisonMask);
        for (int i = 0; i < hitResults.Length; i++)
        {
            if (hitResults[i].gameObject.TryGetComponent(out ITakeDamePoison takeDamePoison))
            {
                takeDamePoison.TakeDamagePoison(damage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentRad);
    }
}
