using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour
{
    public Rigidbody rb;
    public Collider[] colliders;
    public void Drop()
    {
        if (transform.parent != null)
        {
            rb.isKinematic = false;
            transform.parent = null;
            rb.AddForce(Vector3.up);
        }
        for(int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }

    }
}
