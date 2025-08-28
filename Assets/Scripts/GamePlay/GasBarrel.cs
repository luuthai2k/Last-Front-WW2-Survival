using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GasBarrel : MonoBehaviour, ITakeDameBullet
{
    private bool isDestruction = false;
    public float health = 30f;
    [SerializeField] private int numberHit ;
    public GameObject goExplosive, goFlameBurn;
    Coroutine coroutine;
    public UnityEvent onDestruction;
    public List<GameObject> pressurisedSteams = new List<GameObject>();
    public void TakeDamageBullet(Vector3 location, Vector3 normal, Vector3 direction, int damage, int maxDamage, out int damageRemain)
    {
        damageRemain = 0;
        if (isDestruction) return;
        numberHit++;
        if (coroutine == null)
        {
            StartCoroutine(DelayDestruction());
        }
        if (pressurisedSteams.Count < 3)
        {
            var go = ResourceHelper.Instance.GetEffect(EffectType.PressurisedSteam,transform, location, Quaternion.LookRotation(normal));
            pressurisedSteams.Add(go);
        }
      
    }
    public void Destruction()
    {
        isDestruction = true;
        GetComponent<Collider>().enabled = false;
        goExplosive.gameObject.SetActive(true);
        goFlameBurn.SetActive(false);
        foreach(var go in pressurisedSteams)
        {
            go.SetActive(false);
        }
        VibrationController.Instance.PlayMedium();
        CameraManager.Instance.TriggerSmallExplosionImpulse();
        onDestruction?.Invoke();
    }
    IEnumerator DelayDestruction()
    {
        while (health >= 0)
        {
            health -= numberHit * 0.1f;
            if (health <= 15)
            {
                goFlameBurn.SetActive(true);
            }
            yield return new WaitForSeconds(0.1f);
           
        }
        if (!isDestruction)
        {
            Destruction();
        }
    }
}
