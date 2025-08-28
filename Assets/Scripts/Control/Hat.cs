using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hat : MonoBehaviour, ITakeDameBullet
{
    public Rigidbody rb;
    //public int def=100;
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage,int maxDamage, out int damageRemain)
    {
        if (PlayerController.Instance.currentweaponType == WeaponType.Sniper)
        {
            damageRemain = damage;
        }
        else
        {
            damageRemain = 0;
        }
        rb.AddForceAtPosition(direction.normalized * 100, location);
        if (transform.parent != null)
        {
            GamePlayUIManager.Instance.gamePlayMenu.ShowBadge(BadgeType.HatOff);
            transform.parent = null;
            rb.isKinematic = false;
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnShootHelmets));
            Debug.LogError("OnShootHelmets");
        }
        ResourceHelper.Instance.GetEffect(EffectType.MetalImpact,transform, location, Quaternion.LookRotation(normal));
    }
    public void Drop()
    {
        if (transform.parent != null)
        {
            rb.isKinematic = false;
            transform.parent = null;
            rb.AddForce(Vector3.up);
        }
      
    }
}
