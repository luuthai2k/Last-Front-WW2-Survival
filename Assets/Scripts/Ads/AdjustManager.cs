using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.adjust.sdk;

public class AdjustManager : MonoBehaviour
{
    //Start is called before the first frame update
    public string appToken = "kc6zdfwx9zpc";
    void Start()
    {
        AdjustEnvironment environment = AdjustEnvironment.Production;
        AdjustConfig adjustConfig = new AdjustConfig(appToken, AdjustEnvironment.Production);
        adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
        adjustConfig.setLogDelegate(msg => Debug.Log(msg));
        adjustConfig.setEventSuccessDelegate(EventSuccessCallback);
        adjustConfig.setEventFailureDelegate(EventFailureCallback);
        adjustConfig.setSessionSuccessDelegate(SessionSuccessCallback);
        adjustConfig.setSessionFailureDelegate(SessionFailureCallback);
        adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);
        adjustConfig.setAttributionChangedDelegate(AttributionChangedCallback);
        Adjust.start(adjustConfig);
    }
    public void AttributionChangedCallback(AdjustAttribution attributionData)
    {
        Debug.Log("Attribution changed!");

        if (attributionData.trackerName != null)
        {
            Debug.Log("Tracker name: " + attributionData.trackerName);
        }
        if (attributionData.trackerToken != null)
        {
            Debug.Log("Tracker token: " + attributionData.trackerToken);
        }
        if (attributionData.network != null)
        {
            Debug.Log("Network: " + attributionData.network);
        }
        if (attributionData.campaign != null)
        {
            Debug.Log("Campaign: " + attributionData.campaign);
        }
        if (attributionData.adgroup != null)
        {
            Debug.Log("Adgroup: " + attributionData.adgroup);
        }
        if (attributionData.creative != null)
        {
            Debug.Log("Creative: " + attributionData.creative);
        }
        if (attributionData.clickLabel != null)
        {
            Debug.Log("Click label: " + attributionData.clickLabel);
        }
        if (attributionData.adid != null)
        {
            Debug.Log("ADID: " + attributionData.adid);
        }
    }

    public void EventSuccessCallback(AdjustEventSuccess eventSuccessData)
    {
        Debug.Log("Event tracked successfully!");

        if (eventSuccessData.Message != null)
        {
            Debug.Log("Message: " + eventSuccessData.Message);
        }
        if (eventSuccessData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + eventSuccessData.Timestamp);
        }
        if (eventSuccessData.Adid != null)
        {
            Debug.Log("Adid: " + eventSuccessData.Adid);
        }
        if (eventSuccessData.EventToken != null)
        {
            Debug.Log("EventToken: " + eventSuccessData.EventToken);
        }
        if (eventSuccessData.CallbackId != null)
        {
            Debug.Log("CallbackId: " + eventSuccessData.CallbackId);
        }
        if (eventSuccessData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + eventSuccessData.GetJsonResponse());
        }
    }

    public void EventFailureCallback(AdjustEventFailure eventFailureData)
    {
        Debug.Log("Event tracking failed!");

        if (eventFailureData.Message != null)
        {
            Debug.Log("Message: " + eventFailureData.Message);
        }
        if (eventFailureData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + eventFailureData.Timestamp);
        }
        if (eventFailureData.Adid != null)
        {
            Debug.Log("Adid: " + eventFailureData.Adid);
        }
        if (eventFailureData.EventToken != null)
        {
            Debug.Log("EventToken: " + eventFailureData.EventToken);
        }
        if (eventFailureData.CallbackId != null)
        {
            Debug.Log("CallbackId: " + eventFailureData.CallbackId);
        }
        if (eventFailureData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + eventFailureData.GetJsonResponse());
        }

        Debug.Log("WillRetry: " + eventFailureData.WillRetry.ToString());
    }

    public void SessionSuccessCallback(AdjustSessionSuccess sessionSuccessData)
    {
        Debug.Log("Session tracked successfully!");

        if (sessionSuccessData.Message != null)
        {
            Debug.Log("Message: " + sessionSuccessData.Message);
        }
        if (sessionSuccessData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + sessionSuccessData.Timestamp);
        }
        if (sessionSuccessData.Adid != null)
        {
            Debug.Log("Adid: " + sessionSuccessData.Adid);
        }
        if (sessionSuccessData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + sessionSuccessData.GetJsonResponse());
        }
    }

    public void SessionFailureCallback(AdjustSessionFailure sessionFailureData)
    {
        Debug.Log("Session tracking failed!");

        if (sessionFailureData.Message != null)
        {
            Debug.Log("Message: " + sessionFailureData.Message);
        }
        if (sessionFailureData.Timestamp != null)
        {
            Debug.Log("Timestamp: " + sessionFailureData.Timestamp);
        }
        if (sessionFailureData.Adid != null)
        {
            Debug.Log("Adid: " + sessionFailureData.Adid);
        }
        if (sessionFailureData.JsonResponse != null)
        {
            Debug.Log("JsonResponse: " + sessionFailureData.GetJsonResponse());
        }

        Debug.Log("WillRetry: " + sessionFailureData.WillRetry.ToString());
    }

    private void DeferredDeeplinkCallback(string deeplinkURL)
    {
        Debug.Log("Deferred deeplink reported!");

        if (deeplinkURL != null)
        {
            Debug.Log("Deeplink URL: " + deeplinkURL);
        }
        else
        {
            Debug.Log("Deeplink URL is null!");
        }
    }

}
