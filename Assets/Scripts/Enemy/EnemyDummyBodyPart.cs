using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummyBodyPart : MonoBehaviour, ITakeDameBullet
{
    public BoneType boneType;
    public EnemyDummyController enemyDummyController;
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage, int maxDamage, out int damageRemain)
    {
        damageRemain = 0;
        enemyDummyController.TakeDamageBullet(boneType, GetDame(damage, maxDamage));
        ResourceHelper.Instance.GetEffect(EffectType.MetalImpact,transform, location, Quaternion.LookRotation(normal));
    }
  
    public int GetDame(int damage, int maxDamage)
    {
        float damageMultiplier = 1f;
        switch (boneType)
        {
            case BoneType.Head:
                damageMultiplier = 2f;
                break;
            default:
                damageMultiplier = 1f;
                break;
        }
        float finalDamage = damage * damageMultiplier ;
        return (int)Mathf.Min(finalDamage, maxDamage); ;
    }
  
}
