using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBodySensor : MonoBehaviour,ITakeDameBullet,ITakeDameExplosion
{
    public CarHealth carHealth;
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage,int maxDamage, out int damageRemain)
    {
        damageRemain = 0;
        carHealth.TakeDamageBullet((int)Mathf.Min(damage, maxDamage));
        ResourceHelper.Instance.GetEffect(EffectType.MetalImpact, location, Quaternion.LookRotation(normal));
    }
    public void TakeDamageExplosion(Vector3 location, int damage, float force)
    {
        carHealth.TakeDamageBullet((int)damage);
    }
}
