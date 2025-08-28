using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Car : MonoBehaviour
{
    public Rigidbody[] rigidbodies;
    public Renderer[] renderers;
    public Material destructionMaterial;
   
    public Transform explosionTrans;
    public UnityEvent onDestruction;
    public void Destruction()
    {
        onDestruction?.Invoke();
        ResourceHelper.Instance.GetEffect(EffectType.BigExplosion, explosionTrans.position, explosionTrans.rotation);
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            if (rigidbodies[i] != null)
            {
                rigidbodies[i].isKinematic = false;
                rigidbodies[i].AddExplosionForce(50, transform.position, 5);
            }
        }
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                renderers[i].material = destructionMaterial;
            }
        }
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDestroyVehicles));
    }
    
}
