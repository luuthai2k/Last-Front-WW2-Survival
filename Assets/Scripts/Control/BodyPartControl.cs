using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class BodyPartControl : MonoBehaviour, ITakeDameBullet
{
   
    public BoneType boneType;
    public BaseHealth health;
    public Rigidbody rb;
    public Collider collider;
    public bool deactiveWhenDead;
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage,int maxDamage, out int damageRemain)
    {
        damageRemain = 0;
        health.TakeDamageBullet(this, direction,GetDame(damage,maxDamage));
        ResourceHelper.Instance.GetEffect(EffectType.Blood, location, Quaternion.LookRotation(normal));
    }
    public int GetDame(int damage,int maxDamage)
    {
        float damageMultiplier = 1f;
        //float randomMultiplier = Random.Range(0.8f, 1f);
        switch (boneType)
        {
            case BoneType.Head:
                damageMultiplier = 2f;
                break;
            //case BoneType.Spine:
            //case BoneType.Pelvis:
            //case BoneType.Neck:
            //    damageMultiplier = 1.5f;
            //    break;
            default:
                damageMultiplier = 1f;
                break;
        }
        float finalDamage = damage * damageMultiplier /** randomMultiplier*/;
        return (int)Mathf.Min(finalDamage, maxDamage); ;
    }
    public async void OnDead(Vector3 direction)
    {
        if (deactiveWhenDead)
        {
            collider.enabled = false;
        }
        await Task.Yield();
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        if (boneType == BoneType.Spine || boneType == BoneType.Head)
        {
            rb.AddForce(direction, ForceMode.VelocityChange);
        }
    }

}
public enum BoneType
{
    Pelvis,
    LeftHips,
    LeftKnee,
    RightHips,
    RightKnee,
    LeftArm,
    LeftElbow,
    RightArm,
    RightElbow,
    Spine,
    Neck,
    Head
}
