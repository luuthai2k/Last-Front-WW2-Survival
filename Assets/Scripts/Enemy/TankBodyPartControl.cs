using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBodyPartControl : MonoBehaviour, ITakeDameBullet
{
    public TankHealth tankHealth;
    public Rigidbody rb;
    public Collider collider;
    public bool deactiveWhenDead;
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage, int maxDamage, out int damageRemain)
    {
        damageRemain = 0;
        tankHealth.TakeDamageBullet(damage);
        ResourceHelper.Instance.GetEffect(EffectType.MetalImpact, location, Quaternion.LookRotation(normal));
    }
}
