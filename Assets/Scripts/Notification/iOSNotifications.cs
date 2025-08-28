using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using System;

public class iOSNotifications : MonoBehaviour
{
#if UNITY_IOS
    public IEnumerator RequestAuthorization()
    {
        using var request = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true);
        while (!request.IsFinished)
        {
            yield return null;
        }
    }
    public void SendNotifications(string title, string body, DateTime fireTime, string subtitle = "")
    {
        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            Second = fireTime.Second,
            Minute = fireTime.Minute,
            Hour = fireTime.Hour,
            Day = fireTime.Day,
            Repeats = true,
        };

        var notification = new iOSNotification()
        {
            Identifier = "lives_full",
            Title = title,
            Body = body,
            Subtitle = subtitle,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Badge),
            CategoryIdentifier = "default_category",
            ThreadIdentifier = "thread1",
            Trigger = calendarTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }
#endif
}
