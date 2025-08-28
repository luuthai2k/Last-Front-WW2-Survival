using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage = 100;
    public float rad=20,lifeTime=5;
    public AudioClip audioClip;
    public float force = 50;
    private void OnEnable()
    {
        CheckDameExplosion();
        SmartPool.Instance.Despawn(gameObject, lifeTime);
        if (audioClip != null)
        {
            AudioController.Instance.PlaySfx(audioClip);
        }
    }
    private void CheckDameExplosion()
    {
        Collider[] hitResults = Physics.OverlapSphere(transform.position, rad, LayerConfig.Instance.explosionMask);
        for (int i = 0; i < hitResults.Length; i++)
        {
            if (hitResults[i].gameObject.TryGetComponent(out ITakeDameExplosion takeDameExplosion))
            {
                takeDameExplosion.TakeDamageExplosion(transform.position, damage, force);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rad);
    }
}
