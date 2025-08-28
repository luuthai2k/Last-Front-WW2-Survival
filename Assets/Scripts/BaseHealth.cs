using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{

	public bool isDead;
    public int health;

	public virtual void TakeDamageBullet(BodyPartControl bodyPart, Vector3 direction, int damage,bool isGetHit=true)
	{
	}
    public virtual void TakeDamageBullet( int damage)
    {
    }
    public virtual void TakeDamageFlameThrower( int damage)
    {
    }
    public virtual void TakeDamageExplosion(Vector3 direction, int damage, float force )
    {
    }
    public virtual void TakeDamagePoison(int damage)
    {
    }

}
