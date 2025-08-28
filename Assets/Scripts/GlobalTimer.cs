using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using InviGiant.Tools;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class GlobalTimer : MonoBehaviour
{
    private static GlobalTimer _instance = null;

    public static GlobalTimer Instance
    {
        get { return _instance; }
    }
    [SerializeField]
    private double UnixTimeStampNow, UnixTimeStampMidnight;

    [SerializeField]
    private double UnixServerTimeStampNow,UnixTimePlay;

    public List<ITimer> iTimers = new List<ITimer>();

    public List<ITimer> iImmortalTimers = new List<ITimer>();

    public DateTime ServerTime;
    Coroutine coroutine;

    public int SubCount;
    public DateTime LocalUTC
    {
        get { return DateTime.UtcNow; }
    }
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        UnixTimeStampNow = ToolHelper.ConvertToUnixTimeNow();
        SetUnixTimeMidnight();
        coroutine=StartCoroutine(UpDateTimer());
  
    }
    void SetUnixTimeMidnight()
    {
        DateTime today = DateTime.Today;
        DateTime mid = today.AddDays(1).AddSeconds(-1);
        UnixTimeStampMidnight = ToolHelper.ConvertToUnixTime(mid);
        // Debug.Log("SeconLeft 2 Midnight " + (UnixTimeStampMidnight - UnixTimeStampNow).ToString());
    }
    public double GetUnixTimeStampNow(bool serverTime = false)
    {
        if (serverTime) return UnixServerTimeStampNow;
        return UnixTimeStampNow;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        UnixTimeStampNow = ToolHelper.ConvertToUnixTimeNow();
        if (hasFocus)
        {
            UnixTimePlay = 0;
        }

    }
    void OnApplicationPause(bool pauseStatus)
    {
        UnixTimeStampNow = ToolHelper.ConvertToUnixTimeNow();
    }

    IEnumerator UpDateTimer()
    {
        var wfsr = new WaitForSecondsRealtime(1f);
        while (true)
        {
            if (UnityEngine.Time.timeScale == 0)
            {

            }
            else
            {
                UnixTimeStampNow++;
                UnixServerTimeStampNow++;
                UnixTimePlay++;
                SubCount = iTimers.Count;
                for (int i = iTimers.Count - 1; i >= 0; i--)
                {
                    if (!iTimers[i].IsNull())
                    {
                        iTimers[i].OnTick();
                    }
                    else
                    {
                        iTimers.RemoveAt(i);
                    }
                }
                for (int i = iImmortalTimers.Count - 1; i >= 0; i--)
                {
                    if (!iImmortalTimers[i].IsNull())
                    {
                        iImmortalTimers[i].OnTick();
                    }
                    else
                    {
                        iImmortalTimers.RemoveAt(i);
                    }
                }
            }
            if (UnixTimeStampMidnight - UnixTimeStampNow <= 0)
            {
                SetUnixTimeMidnight();

                DataController.Instance.ResetNewDay();
            }
            yield return wfsr;
        }
    }

    public void SubcribeITimer(ITimer it)
    {
        if (iTimers == null) iTimers = new List<ITimer>();
        if (!iTimers.Contains(it)) iTimers.Add(it);
    }

    public void SubcribeImmortalTimer(ITimer it)
    {
        if (iImmortalTimers == null) iImmortalTimers = new List<ITimer>();
        if (!iImmortalTimers.Contains(it)) iImmortalTimers.Add(it);
    }

    public void UnSubcribeImmortalTimer(ITimer it)
    {
        if (iImmortalTimers.Contains(it)) iImmortalTimers.Remove(it);
    }

    public void UnSubcribeITimer(ITimer it)
    {
        if (iTimers.Contains(it))
        {
            iTimers.Remove(it);
        }
    }

    public void OnAddTime(int Value)
    {
        for (int i = iTimers.Count - 1; i >= 0; i--)
        {
            if (!iTimers[i].IsNull())
            {
                iTimers[i].OnAddTime(Value);
            }
            else
            {
                iTimers.RemoveAt(i);
            }
        }
    }

    public void ClearTimers()
    {
        iTimers = new List<ITimer>();
    }

  
}
