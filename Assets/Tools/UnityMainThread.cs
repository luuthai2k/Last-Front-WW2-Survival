using System;
using System.Collections.Generic;
using UnityEngine;

internal class UnityMainThread : MonoBehaviour
{
    internal static UnityMainThread Instance;
    Queue<Action> jobs = new Queue<Action>();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        while (jobs.Count > 0)
            jobs.Dequeue().Invoke();
    }

    internal void AddJob(Action newJob)
    {
        jobs.Enqueue(newJob);
    }
}
