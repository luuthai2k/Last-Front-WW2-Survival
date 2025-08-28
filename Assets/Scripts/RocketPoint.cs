using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPoint : MonoBehaviour
{
    public RocketControl rocketControl;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController.Instance.ChangeToStateRocket(rocketControl);
        }
    }
}
