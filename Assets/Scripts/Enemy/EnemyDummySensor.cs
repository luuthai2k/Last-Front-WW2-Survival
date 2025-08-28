using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummySensor : MonoBehaviour, ITakeDameExplosion, ITakeDamePoison
{
    public EnemyDummyController enemyDummyController;
    public void TakeDamageExplosion(Vector3 location, int damage, float force)
    {
        enemyDummyController.TakeDamage(damage);
    }
    public void TakeDamagePoison(int damage)
    {
        enemyDummyController.TakeDamage(damage);
    }
}
