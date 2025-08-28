using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattlePassDatas
{
    public BattlePassData[] events;
    public BattlePassData GetEventById(string eventId)
    {
        foreach (var ev in events)
            if (ev.ID == eventId)
                return ev;
        return null;
    }
}
[System.Serializable]
public class BattlePassData
{
    public string ID;
    public string Name;
  
}
[System.Serializable]
public class GameEvents
{
    public GameEvent[] events;
    public GameEvent GetEventById(string eventId)
    {
        foreach (var ev in events)
            if (ev.EventID == eventId)
                return ev;
        return null;
    }
    public float GetPackLifeTime(string eventId)
    {
        foreach (var ev in events)
            if (ev.EventID == eventId)
                return ev.packLifeTime;
        return 0;
    }
    public float GetPackWaitTime(string eventId)
    {
        foreach (var ev in events)
            if (ev.EventID == eventId)
                return ev.waitLifeTime;
        return 0;
    }
}
[System.Serializable]
public class GameEvent
{
    public string EventID;
    public string EventName;
    public int packLifeTime = 86400;
    public int waitLifeTime = 86400;
}
public class EventHelper : MonoBehaviour
{
    public static GameEvents events = null;
    public const string IAP_VALUE = "user_iap_value";
    public static GameEvent GetEvent(string eventId)
    {
        if (events == null)
        {
            string packsData = Resources.Load<TextAsset>("Event_Config").text;
            events = JsonUtility.FromJson<GameEvents>(packsData);
        }
        return events.GetEventById(eventId);
    }
    public static float GetEventLifeTime(string eventId)
    {
        if (events == null)
        {
            string packsData = Resources.Load<TextAsset>("Event_Config").text;
            events = JsonUtility.FromJson<GameEvents>(packsData);
        }
        return events.GetPackLifeTime(eventId);
    }
    public static float GetEventWaitTime(string eventId)
    {
        if (events == null)
        {
            string packsData = Resources.Load<TextAsset>("Event_Config").text;
            events = JsonUtility.FromJson<GameEvents>(packsData);
        }
        return events.GetPackWaitTime(eventId);
    }
}
