using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerConfig : Singleton<LayerConfig>
{
    public LayerMask rayCastMask, shootMask, explosionMask,poisonMask, enemyMask,enemyRayCastMask;
}
