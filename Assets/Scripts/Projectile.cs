using InviGiant.Tools;
using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[System.NonSerialized]
	public int bulletId;
	public TrailRenderer trail;
	public float speed=500;
    public Vector3 target;
    public bool isTarget;

    public virtual void Init(Vector3 initPos, Vector3 direction)
    {
        transform.position = initPos;
        transform.forward = direction;
        trail.enabled = true;
        isTarget = false;
        StartCoroutine(DelayReturn());

    }
    public virtual void Init(Vector3 initPos, Vector3 direction,float speed)
    {
        transform.position = initPos;
        transform.forward = direction;
        trail.enabled = true;
        isTarget = false;
        this.speed = speed;
        StartCoroutine(DelayReturn());

    }
    public virtual void Init(Vector3 initPos, Vector3 target,bool isTarget)
    {
        transform.position = initPos;
        this.target = target;
        this.isTarget = isTarget;
        trail.enabled = true;
        StartCoroutine(DelayReturn());

    }
    void Update()
    {
        if (isTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position,target, speed * Time.deltaTime);
          
        }
        else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
      
    }
   IEnumerator  DelayReturn()
    {
        yield return new WaitForSeconds(0.5f);
        trail.enabled = false;
        yield return new WaitForSeconds(0.1f);
        SmartPool.Instance.Despawn(gameObject);
    }
   
}
