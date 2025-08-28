using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour,ITakeDameExplosion, ITakeDamePoison
{
    public BodyPartControl spineControl;
    public EnemyHealth enemyHealth;
    public void TakeDamageExplosion(Vector3 location, int damage,float force)
    {
        Vector3 direction = spineControl.transform.position - location;
        enemyHealth.TakeDamageExplosion(direction, damage, force);
    }
    public void TakeDamagePoison(int damage)
    {
        enemyHealth.TakeDamagePoison(damage);
    }
}
