using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyCrate : MonoBehaviour,ITakeDameBullet
{
    public KeyCrateControl keyCrateControl;
    public GameObject goBreakFx,goCrate;
    public Rigidbody[] cratePartRbs;
    public Collider collider;
    public Rigidbody rb;
    public UnityEvent onBreak;
    bool isBreak;
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage, int maxDamage, out int damageRemain)
    {
        damageRemain = 0;
        Break();
    }
    public void Break()
    {
        if (isBreak) return;
        isBreak = true;
        keyCrateControl.TakeKey();
        goBreakFx.SetActive(true);
        goCrate.SetActive(false);
        collider.enabled = false;
        for (int i = 0; i < cratePartRbs.Length; i++)
        {
            cratePartRbs[i].gameObject.SetActive(true);
            cratePartRbs[i].isKinematic=false;
            cratePartRbs[i].AddExplosionForce(5, transform.position, 5);
            cratePartRbs[i].transform.parent = null;
        }
        onBreak?.Invoke();
    }
    public void Drop()
    {
        rb.isKinematic = false;
    }
    public void OnCollisionEnter(Collision collision)
    {
        Break();
    }
}
