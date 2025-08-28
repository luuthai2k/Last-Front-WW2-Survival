using System.Collections;
using UnityEngine;
namespace InviGiant.Tools
{
    public class AutoDeactiveAfterDelay : MonoBehaviour
    {
        public float delay;
        void OnEnable()
        {
            StartCoroutine(AutoDeActive());
        }
        IEnumerator AutoDeActive()
        {
            yield return new WaitForSeconds(delay);
            SmartPool.Instance.Despawn(gameObject);
        }
    }
}
