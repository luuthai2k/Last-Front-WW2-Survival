using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeeMessageType
{
    OnGameStart = 0,
    OnGamePlay = 1,
    OnGamePause = 2,
    OnGameEnd = 3,
    OnWinGame = 4,
    OnLoseGame = 5,
    OnDataChange = 6,
    OnFirstTime = 7,
    OnWatchAds = 8,
    OnSceneLoaded = 9,
    OnCashChange = 10,
    OnGoldChange = 11,
    OnMetalChange = 12,
    OnKeyChange = 13,
    OnLoadGamePLayScene = 14,
    OnRemoveAds=15,
    OnCompletePurchasePack = 16,
    TutorialDialog=17,
    OnOpenCrate = 18,
    OnShoot = 19,
    OnShootHelmets = 20,
    OnDestroyVehicles = 21,
    OnKillEnermy = 22,
    OnHeadShoot = 23,
    OnAmmoChange = 24,
}

public class Message
{
    public TeeMessageType type;

    public object[] data;

    public Message(TeeMessageType type)
    {
        this.type = type;
    }

    public Message(TeeMessageType type, object[] data)
    {
        this.type = type;
        this.data = data;
    }
}

public interface IMessageHandle
{
    void Handle(Message message);
}

public class MessageManager : MonoBehaviour, ISerializationCallbackReceiver
{
    private static MessageManager instance = null;

    [HideInInspector]
    public List<TeeMessageType> _keys = new List<TeeMessageType>();

    [HideInInspector]
    public List<List<IMessageHandle>>
        _values = new List<List<IMessageHandle>>();

    private Dictionary<TeeMessageType, List<IMessageHandle>>
        subcribers = new Dictionary<TeeMessageType, List<IMessageHandle>>();

    public static MessageManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void AddSubcriber(TeeMessageType type, IMessageHandle handle)
    {
        if (!subcribers.ContainsKey(type))
            subcribers[type] = new List<IMessageHandle>();
        if (!subcribers[type].Contains(handle)) subcribers[type].Add(handle);
    }

    public void RemoveSubcriber(TeeMessageType type, IMessageHandle handle)
    {
        if (subcribers.ContainsKey(type))
            if (subcribers[type].Contains(handle))
                subcribers[type].Remove(handle);
    }

    public void SendMessage(Message message)
    {
        if (subcribers.ContainsKey(message.type))
            for (int i = subcribers[message.type].Count - 1; i > -1; i--)
            {
                if (subcribers[message.type][i] != null)
                    subcribers[message.type][i].Handle(message);
                else
                    subcribers[message.type].RemoveAt(i);
            }
    }

    public void SendMessageWithDelay(Message message, float delay)
    {
        StartCoroutine(_DelaySendMessage(message, delay));
    }

    private IEnumerator _DelaySendMessage(Message message, float delay)
    {
        yield return new WaitForSeconds(delay);
        SendMessage(message);
    }

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach (var element in subcribers)
        {
            _keys.Add(element.Key);
            _values.Add(element.Value);
        }
    }

    public void OnAfterDeserialize()
    {
    }
}
