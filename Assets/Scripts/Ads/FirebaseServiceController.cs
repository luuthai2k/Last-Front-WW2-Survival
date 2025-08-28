using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FirebaseServiceController : SingletonPersistent<FirebaseServiceController>
{

    public bool isFirebaseInited = false;
  
    public int GetItemOfferCooldown()
    {
      
        int value = 0;
        if (value == 0)
        {
            value = IAPPackHelper.GetItemOfferCooldown();
        }
        return value;
    }
    public void LogEvent(string eventKey, string parameterName, string parameterValue)
    {
        if (!isFirebaseInited) return;
        FirebaseAnalytics.LogEvent(eventKey, parameterName, parameterValue);
    }
    public void LogEvent(string eventKey)
    {
        if (!isFirebaseInited) return;
        FirebaseAnalytics.LogEvent(eventKey);
    }

}
