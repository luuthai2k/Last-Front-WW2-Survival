using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBarrel : MonoBehaviour,ITakeDameBullet
{
    private bool isDestruction = false;
    public GameObject originBarrel,destructionBarrel;
    public Rigidbody[] rigidbodies;

    public float health = 50f;
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage,int maxDamage, out int damageRemain)
    {
        damageRemain = 0;
        if (isDestruction) return;
        health -= Mathf.Min(damage, maxDamage);
        if (health <= 0&&!isDestruction)
        {
            isDestruction = true;
            GetComponent<Collider>().enabled = false;
            ResourceHelper.Instance.GetEffect(EffectType.BigExplosion, location, Quaternion.identity);
            destructionBarrel.gameObject.SetActive(true);
            originBarrel.gameObject.SetActive(false);
            foreach (var rb in rigidbodies)
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.AddExplosionForce(50, location, 5);
                }
            }
            VibrationController.Instance.PlayMedium();
            CameraManager.Instance.TriggerBigExplosionImpulse();
        }
    }
}
