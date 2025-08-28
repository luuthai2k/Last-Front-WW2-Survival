using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Rigidbody rb;
    //public Collider collider;

    public void ReleaseGrenade(float timeToTarget)
    {
        //collider.enabled = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;
        rb.transform.SetParent(null);
        Vector3 start = transform.position;
        Vector3 target = CameraManager.Instance.GetAimTargetPosition();
        Vector3 gravity = Physics.gravity;
        float t = timeToTarget;

        Vector3 velocity = (target - start - 0.5f * gravity * t * t) / t;

        rb.AddForce(velocity * rb.mass, ForceMode.Impulse);
        StartCoroutine(DelayActiveCollider(timeToTarget));
    }
    IEnumerator DelayActiveCollider(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        ResourceHelper.Instance.GetEffect(EffectType.BigExplosion, transform.position, transform.rotation);
       

    }
}
