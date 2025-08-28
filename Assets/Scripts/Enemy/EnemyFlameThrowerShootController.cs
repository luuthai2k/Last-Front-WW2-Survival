using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlameThrowerShootController : MonoBehaviour
{
    public FlameThrower flameThrower;
    public void Init(int damage,float fireRate)
    {
        flameThrower.damage = damage;
        flameThrower.fireRate = fireRate;
    }
   
    public void StartShoot()
    {
        flameThrower.StartShoot();
       
    }
    public void FinishShoot()
    {
        flameThrower.FinishShoot();
      
    }
   
  

}
