using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBarrel : MonoBehaviour, ITakeDameBullet
{
    public float health = 50f;
    public SkinnedMeshRenderer renderer;
    [SerializeField]private int maxHit = 3;
    private bool isDestruction = false;
    private float currentWeight = 100f;
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage,int maxDamage, out int damageRemain)
    {
        damageRemain = 0;
        if (maxHit > 0&& currentWeight>=20)
        {
            maxHit--;
            ResourceHelper.Instance.GetEffect(EffectType.PoisonBarretImpact, location, Quaternion.LookRotation(normal));
        }
        health -= Mathf.Min(damage,maxDamage);
        if (health <= 0&&!isDestruction)
        {
            isDestruction = true;
            ResourceHelper.Instance.GetEffect(EffectType.PoisonCloud, transform.position, Quaternion.identity);
            DOTween.To(() => currentWeight, x =>
            {
                currentWeight = x;
                renderer.SetBlendShapeWeight(0, currentWeight);
            }, 0f, 10f);
        }
       

    }

}