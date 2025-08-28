using DG.Tweening;
using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{
    public MeshRenderer decalRenderer;
    public float lifeTime=2;
    private void OnEnable()
    {
        if (decalRenderer) decalRenderer.material.DOFade(1, 0.1f);
        StartCoroutine(DelayDeSpawn());
    }
    IEnumerator DelayDeSpawn()
    {
        yield return new WaitForSeconds(lifeTime-0.35f);
        if (decalRenderer) decalRenderer.material.DOFade(0, 0.35f);
        yield return new WaitForSeconds(0.35f);
        SmartPool.Instance.Despawn(gameObject);
    }
}
