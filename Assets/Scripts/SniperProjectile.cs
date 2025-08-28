using DG.Tweening;
using System;
using UnityEngine;
using System.Collections;

public class SniperProjectile : Projectile
{
	public BodyPartControl bodyPartControl;
    public Transform bulletTrans,camTarget;
    public AnimationCurve animationCurve;
    public float rotationSpeed;
    Vector3 normal;
    int damage;
    private void OnEnable()
    {
        CameraManager.Instance.SetActive(false);
        PlayerUIControl.Instance.SetDisplay(false);
        StartCoroutine(DelayMove());

    }
    IEnumerator  DelayMove()
    {
        yield return new WaitForSeconds(0.01f);
        AudioController.Instance.PlaySfx(GameConstain.BULLET_TIME);
        float time = 0;
        Vector3 startPoint = transform.position;
        while (time < 0.25f)
        {
            time += Time.deltaTime;
            float normalizedTime = time / 0.25f;
            float curveValue = animationCurve.Evaluate(normalizedTime);
            transform.position = Vector3.Lerp(startPoint, target, curveValue);
            yield return null;
        }
        //bodyPartControl.enemyHealth.enemyController.animatorController.animator.speed = 0;
        ResourceHelper.Instance.GetEffect(EffectType.Blood, target, Quaternion.LookRotation(normal));
        bodyPartControl.health.TakeDamageBullet(bodyPartControl, transform.forward, damage);
        gameObject.SetActive(false);
    }
    public void Init(Vector3 initPos, Vector3 target,Vector3 normal,  BodyPartControl bodyPartControl,int damage)
    {
        transform.position = initPos;
        transform.LookAt(target);
        this.target = target;
        this.isTarget = true;
        this.normal = normal;
        this.bodyPartControl = bodyPartControl;
        this.damage = damage;
        trail.enabled = true;
      
      
    }
    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        bulletTrans.Rotate(0, 0, rotationSpeed);

    }
    private void OnDisable()
    {
        CameraManager.Instance.SetActive(true);
    }
}
