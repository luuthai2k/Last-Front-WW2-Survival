using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

using UnityEngine;
//using UnityEngine.Localization;
//using UnityEngine.Localization.Settings;
//using UnityEngine.Localization.Tables;
//using UnityEngine.ResourceManagement.AsyncOperations;

public class NotificationsManager : MonoBehaviour
{
    [SerializeField] AndroidNotifications androidNotifications;
    [SerializeField] iOSNotifications iOSNotifications;

    private void OnValidate()
    {
        if (androidNotifications == null)
        {
            if (TryGetComponent(out AndroidNotifications notifications))
            {
                androidNotifications = notifications;
            }
            else
            {
                Debug.LogWarning("NotificationsManager component is missing from your scene.");
            }
        }
        if (iOSNotifications == null)
        {
            if (TryGetComponent(out iOSNotifications notifications))
            {
                iOSNotifications = notifications;
            }
            else
            {
                Debug.LogWarning("NotificationsManager component is missing from your scene.");
            }
        }
    }

    protected void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
#if UNITY_ANDROID
        androidNotifications.RequestAuthorization();
        androidNotifications.RegisterNotificationChannel();
#endif

#if UNITY_IOS
        StartCoroutine(iOSNotifications.RequestAuthorization());
#endif
    }

    string[] notiDataBuilder = new string[]
    {
        "WW2: Duty Calls! 💥 | Fight on the fierce frontline of WW2!",

        "The Frontline Awaits! 🗺️| Complete the mission, save the world!",

        "Duty WW2: Get Into Battle Now! 🔫| New weapons, new enemies, are you ready?",

        "Heroes of WW2, Come In! 🎖️| Big update! Play now to get rewards!",

        "Experience War! 🔥| Shoot down the enemy, win!",

        "WW2: Get Ready to Fight! 🛡️| Join the epic battle now!",

        "New Missions Are Here! 📜| Are you ready for the frontline?",

        "FrontLine Zone: Action! 🚀| WW2 Duty: Play to feel!",

         "Don't Miss WW2 Duty! 🚨| New missions await you!",

          "Join the Great WW2 War! 🌍| A relentless battle! 💪",

    };
    int length = 8;

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
#if UNITY_ANDROID
            if (notiDataBuilder[0] == null) return;
            AndroidNotificationCenter.CancelAllNotifications();
            DateTime now = DateTime.Now;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int offSet = (2 * (int)DateTime.Now.DayOfWeek) % length;
                    int id = (2 * i + j + offSet) % length;
                    string[] data = notiDataBuilder[id].Split('|');
                    string title = data[0];
                    string content = data[1];
                    int hour = (j == 0 ? 11 : 20);
                    int day = i;
                    if (now.Hour >= hour && i==0)
                    {
                        day += 7;
                    }
                    DateTime fireTime = new DateTime(now.Year, now.Month, now.Day, hour, 0, 0).AddDays(day);
                    androidNotifications.SendNotification(title, content, fireTime);
                }
            }
#endif
#if UNITY_IOS
            if (notiDataBuilder[0] == null) return;
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            DateTime now = DateTime.Now;
            for (int i = 0; i < 31; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int offSet = (2 * DateTime.Now.Day) % length;
                    int id = (2 * i + j + offSet) % length;
                    string[] data = notiDataBuilder[id].ToString().Split('|');
                    string title = data[0];
                    string content = data[1];
                    int hour = (j == 0 ? 11 : 20);
                    int day = i;
                    if (now.Hour >= hour && i == 0)
                    {
                        day += DateTime.DaysInMonth(now.Year, now.Month);
                    }
                    DateTime fireTime = new DateTime(now.Year, now.Month, now.Day, hour, 0, 0).AddDays(day);
                    iOSNotifications.SendNotifications(title, content, fireTime);
                }
            }
#endif
        }
    }
    /*void OnEnable()
    {
        // During initialization we start a request for the string and subscribe to any locale change events so that we can update the strings in the future.
        StartCoroutine(LoadStrings());
        LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
    }

    void OnSelectedLocaleChanged(Locale obj)
    {
        StartCoroutine(LoadStrings());
    }

    IEnumerator LoadStrings()
    {
        // A string table may not be immediately available such as during initialization of the localization system or when a table has not been loaded yet.
        var loadingOperation = LocalizationSettings.StringDatabase.GetTableAsync("GameString");
        yield return loadingOperation;

        if (loadingOperation.Status == AsyncOperationStatus.Succeeded)
        {
            var stringTable = loadingOperation.Result;
            for (int i = 0; i < length; i++)
            {
                notiDataBuilder[i] = new StringBuilder();
                for (int j = 1; j <= 2; j++)
                {
                    if (j == 1)
                    {
                        if (i == 5)
                        {
                            notiDataBuilder[i].Append("\U0001F976 ");
                        }
                        else if (i == 6)
                        {
                            notiDataBuilder[i].Append("\U0001F917 ");
                        }
                        else if (i == 7)
                        {
                            notiDataBuilder[i].Append("\U0001F9D0 ");
                        }
                    }
                    notiDataBuilder[i].Append(GetLocalizedString(stringTable, "NotiData_" + i + "_" + j));
                    if (j == 1)
                    {
                        if (i == 0)
                        {
                            notiDataBuilder[i].Append("\U0001F4AA\U0001F4AA");
                        }
                        else if (i == 1)
                        {
                            notiDataBuilder[i].Append("\U0001F60D\U0001F60D");
                        }
                        else if (i == 2)
                        {
                            notiDataBuilder[i].Append("\U0001F929\U0001F929");
                        }
                        else if (i == 3)
                        {
                            notiDataBuilder[i].Append("\U0001F976\U0001F976");
                        }
                        else if (i == 4)
                        {
                            notiDataBuilder[i].Append("\U0001F61C\U0001F976\U0001F976");
                        }
                        else if (i == 5)
                        {
                            notiDataBuilder[i].Append(" \U0001F383\U0001F607");
                        }
                        else if (i == 6)
                        {
                            notiDataBuilder[i].Append("\U0001F607");
                        }
                        else if (i == 7)
                        {
                            notiDataBuilder[i].Append("\U0001F976");
                        }
                        notiDataBuilder[i].Append("|");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Could not load String Table\n" + loadingOperation.OperationException.ToString());
        }
    }

    string GetLocalizedString(StringTable table, string entryName)
    {
        // Get the table entry. The entry contains the localized string and Metadata
        var entry = table.GetEntry(entryName);
        return entry.GetLocalizedString(); // We can pass in optional arguments for Smart Format or String.Format here
    }*/
}
