using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditMap : MonoBehaviour
{
# if UNITY_EDITOR
    public GameObject prefab;
    public List<Vector3> pos = new List<Vector3>();
    public List<Quaternion> rot = new List<Quaternion>();
    public List<GameObject> oldChilds = new List<GameObject>();
    public List<GameObject> newChilds = new List<GameObject>();

    [ContextMenu("Get Old Child")]
    void GetOldChild()
    {
        pos.Clear();
        rot.Clear();
        oldChilds.Clear();

        foreach (Transform child in transform)
        {
            pos.Add(child.position);
            rot.Add(child.rotation);
            oldChilds.Add(child.gameObject);
        }
    }

    [ContextMenu("Spawn Prefab")]
    void SpawnPrefab()
    {
        int count = pos.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
            go.transform.position = pos[i];
            go.transform.rotation = rot[i];
            newChilds.Add(go);
        }
    }

    [ContextMenu("Destroy Old Child")]
    void DesTroyOldChild()
    {
        foreach (var child in oldChilds)
        {
            DestroyImmediate(child);
        }
        oldChilds.Clear();
    }
#endif
}
