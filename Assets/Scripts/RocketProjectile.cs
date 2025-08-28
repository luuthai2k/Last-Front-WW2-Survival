using DG.Tweening;
using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RocketProjectile : Projectile
{
    public float rotationSpeed=50;
    public void Init(Vector3 target)
    {
      
        this.target = target;
        trail.enabled = true;
      

    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        if (transform.position == target)
        {
            ResourceHelper.Instance.GetEffect(EffectType.RocketExplosion, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            SmartPool.Instance.Despawn(gameObject);
        }
    }

}
