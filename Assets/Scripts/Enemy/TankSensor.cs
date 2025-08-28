using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSensor : MonoBehaviour, ITakeDameExplosion
{
    public TankHealth tankHealth;
     public void TakeDamageExplosion(Vector3 location, int damage, float force)
    {
        Vector3 direction = transform.position - location;
        tankHealth.TakeDamageExplosion(direction, damage, force);
    }
  
}
