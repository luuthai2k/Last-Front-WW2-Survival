using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : Point
{
    public bool isTargeted = false;
    public EnemyPoint[] nextPoint;
    public Transform killCamPoint;
    private void Awake()
    {
        isTargeted = false;
    }
   public EnemyPoint GetNextPoint()
    {
        if (nextPoint.Length == 0) return null;
        return nextPoint[Random.Range(0, nextPoint.Length)];
    }
}
