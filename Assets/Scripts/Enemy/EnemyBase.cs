using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int id;
    public EnemyState currentState;
    public EnemyType currentType;
  
    public EnemyHealth enemyHealth;
    public float moveSpeed,rotationSpeed,coverTime;
  
  
    [HideInInspector]
    public float timeStandMax, startTime;

    [HideInInspector]
    public float timerToStand, timerToReload, timerToShoot;
    public bool isCanShoot,canChangePatrolPoint;
    public AudioClip[] sfxHitBodys;
    public AudioSource audioSource;

    public virtual void Init(int id, MoveType height, bool canChangePatrolPoint,int maxHP, WeaponType weaponType, int damage,float fireRate)
    {
        this.id = id;
        this.canChangePatrolPoint = canChangePatrolPoint;
        enemyHealth.InitHP(maxHP);
    }
  
    public virtual void StartWave(EnemyPoint targetPoint)
    {

    }    

    public virtual void Dead( Vector3 direction)
    {
       
    }
    public virtual void GetHit(float hitType)
    {
       
    }
}
public enum EnemyType
{
    Soldier1,
    Soldier2,
    Soldier3,
    Soldier4,
    BlackSoldier,
    Officer,
    Flamethrower,
    Tank,
}

public enum EnemyState
{
    None,
    Move,
    Cover,
    Reload,
    Shoot,
    Dead
}
