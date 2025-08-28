using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDameBullet
{
    void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction,int damage,int maxDamage,out int damageRemain);
}
