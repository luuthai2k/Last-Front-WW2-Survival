using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDameExplosion 
{
    void TakeDamageExplosion(Vector3 location, int damage, float force);
}
