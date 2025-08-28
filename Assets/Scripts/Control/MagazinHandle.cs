using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinHandle : MonoBehaviour
{
    public GameObject goMagazin;
    public void OnDump()
    {
        GameObject newGoMagazin = Instantiate(goMagazin);
        newGoMagazin.SetActive(false);
        goMagazin.transform.parent = null;
        goMagazin.AddComponent<Rigidbody>();
        goMagazin = newGoMagazin;
    }
    public void OnTake(Transform magazinTarget)
    {
        goMagazin.transform.parent = magazinTarget;
        goMagazin.transform.localPosition = Vector3.zero;
        goMagazin.transform.localRotation = Quaternion.identity;
        goMagazin.SetActive(true);

    }
    public void OnPut()
    {
        goMagazin.transform.parent = transform;
        goMagazin.transform.localPosition = Vector3.zero;
        goMagazin.transform.localRotation = Quaternion.identity;

    }
}
