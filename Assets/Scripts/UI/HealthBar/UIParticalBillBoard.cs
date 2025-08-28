using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticalBillBoard : MonoBehaviour
{
    [SerializeField] Transform target;

    void Start()
    {
        Debug.LogError("UIParticalBillBoard");
        if (target == null)
        {
            target = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        Vector3 directionToTarget = -target.forward;
        directionToTarget.x = 0;
        directionToTarget.z = 0;
        transform.LookAt(directionToTarget);
    }
}
