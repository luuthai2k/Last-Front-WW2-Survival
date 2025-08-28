using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
public class TimeDelayTutorialState : State
{
    [SerializeField] private float delay;
    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(delay);
        Next();
    }

}
