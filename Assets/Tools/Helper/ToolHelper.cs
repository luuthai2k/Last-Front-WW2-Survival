
using System.IO;
using System.Net;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using Newtonsoft.Json;
public static class ToolHelper
{
    public static string[] TextFormat = { " ", " K", " M", " B", " T", " a"," b"," c"," d"," e"," f"," g"," h"," i"," j"," k"," l", " m",
    " n"," o"," p"," q"," r"," s", " t"," u"," v"," w","x"," y"," z"," aa"," ab"," ac"," ad"," ae"," af"," ag"," ah"," ai"," aj"," ak"," al"," am"," an"," ao"," ap"," aq"," ar"," as"," at"," au"," av"," aw"," ax"," ay"," az"};
    public static string FormatDouble(double number)
    {
        string txt = "";
        if (number < 1000)
        {
            number = (int)number;
            if (number < 1000)
                txt = number.ToString();
        }
        else
        {
            if (number <= 0)
                return "0";
            int a = (int)System.Math.Log10(number);
            int b = a / 3;
            if (b == 0)
                return number.ToString("F2");
            if (b == 1)
                txt = (number / 1000).ToString("F2") + " K";
            else if (b < TextFormat.Length)
                txt = (number / System.Math.Pow(10, b * 3)).ToString("F2") + TextFormat[b];
            else
             if (number < 1000)
                txt = number.ToString();
            string[] tmp = txt.Split(' ');
            string[] tmp2 = tmp[0].Split('.');
            if (tmp2.Length > 1)
            {
                if (tmp2[1] == "00")
                    return tmp2[0] + " " + tmp[1];
            }
        }
        return txt;
    }
    public static string FormatDouble2(double number)
    {
        if (number == 0)
            return "0";
        int a = (int)System.Math.Log10(number);
        int b = a / 3;
        if (b == 0)
            return ((int)number).ToString("F0");
        return FormatDouble(number);
    }
    public static string FormatFloat(double number)
    {
        return string.Format("{0:0.##}", number);
    }
    public static string FormatLong(long number)
    {
        if (number < 1000)
        {
            return number.ToString("F0");
        }
        else if (number < 1000000)
        {
            return ((double)number / 1000).ToString("F2") + " K";
        }
        else if (number < 1000000000)
        {
            return ((double)number / 1000000).ToString("F2") + " M";
        }
        else if (number < 1000000000000)
        {
            return ((double)number / 1000000000).ToString("F2") + " B";
        }
        else if (number < 1000000000000000)
        {
            return ((double)number / 1000000000000).ToString("F2") + " T";
        }
        else
        {
            return number.ToString("0.00e0");
        }
    }
    public static string FormatLong2(long number)
    {
        return string.Format("{0:N0}", number).Replace(',', '.');
    }
    public static string FormatInt2(int number)
    {
        return string.Format("{0:N0}", number).Replace(',', '.');
    }
    public static long ParseFormattedStringToLong(string formattedNumber)
    {
        string clean = formattedNumber.Replace(".", "");
        return long.Parse(clean);
    }
    public static int ParseFormattedStringToInt(string formattedNumber)
    {
        string clean = formattedNumber.Replace(".", "");
        return int.Parse(clean);
    }
    public static string FormatMultiplier(double number)
    {
        if (number < 1000)
        {
            return number.ToString("F2");
        }
        else if (number < 1000000)
        {
            return (number / 1000).ToString("F2") + " K";
        }
        else if (number < 1000000000)
        {
            return (number / 1000000).ToString("F2") + " M";
        }
        else if (number < 1000000000000)
        {
            return (number / 1000000000).ToString("F2") + " B";
        }
        else if (number < 1000000000000000)
        {
            return (number / 1000000000000).ToString("F2") + " T";
        }
        else
        {
            return number.ToString("0.00e0");
        }
    }
    public static string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

    public static void SetDefaultTransform(this Transform transform)
    {
        transform.gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }
    public static void DelaySetActive(this GameObject gameObject, bool active, float timer)
    {
        DG.Tweening.DOVirtual.DelayedCall(timer, () =>
        {
            gameObject.SetActive(active);
        });

    }
    public static void SetRectTransform(this RectTransform transform, RectTransform target)
    {
        transform.anchoredPosition = target.anchoredPosition;
        transform.anchorMax = target.anchorMax;
        transform.anchorMin = target.anchorMin;
        transform.localPosition = target.localPosition;
        transform.localScale = target.localScale;
        transform.offsetMax = target.offsetMax;
        transform.offsetMin = target.offsetMin;
        transform.pivot = target.pivot;
    }
    public static string FormatTime(double seconds)
    {
        int days = (int)(seconds / 86400);
        int hours = (int)((seconds % 86400) / 3600);
        int minutes = (int)((seconds % 3600) / 60);
        int secs = (int)(seconds % 60);

        if (days > 0)
            return $"{days}d {hours}h";
        else if (hours > 0)
            return $"{hours}h {minutes}m";
        else
            return $"{minutes}m {secs}s";
    }
    public static string GetTextTime(double sec, bool ignoreHour00 = false)
    {
        try
        {
            var TimeSpan1 = TimeSpan.FromSeconds(sec);
            string timeOffline = "";
            if (TimeSpan1.Hours > 0)
            {
                if (TimeSpan1.Hours < 10)
                {
                    timeOffline = "0" + TimeSpan1.Hours.ToString() + ":";
                }
                else
                {
                    timeOffline = TimeSpan1.Hours.ToString() + ":";
                }
            }
            else
            {
                if (!ignoreHour00)
                    timeOffline = "00:";
            }
            if (TimeSpan1.Minutes > 0)
            {
                if (TimeSpan1.Minutes < 10)
                {
                    timeOffline += "0" + TimeSpan1.Minutes.ToString();
                }
                else
                {
                    timeOffline += TimeSpan1.Minutes.ToString();
                }
            }
            else
            {
                timeOffline += "00";
            }
            if (TimeSpan1.Seconds > 0)
            {
                if (TimeSpan1.Seconds < 10)
                {
                    timeOffline += ":0" + TimeSpan1.Seconds.ToString();
                }
                else
                {
                    timeOffline += ":" + TimeSpan1.Seconds.ToString();
                }
            }
            else
            {
                timeOffline += ":00";
            }
            return timeOffline;
        }
        catch (System.Exception ex)
        {
            return "";
        }
    }
    public static string GetTextTimeLarge(double sec, bool ignoreHour00 = false)
    {
        // var TimeSpan1 = TimeSpan.FromSeconds(sec);
        int Hours = GetHours((float)sec);
        int Minutes = GetMinutes((float)sec);
        int Seconds = GetSeconds((float)sec);
        string timeOffline = "";
        if (Hours > 0)
        {
            if (Hours < 10)
            {
                timeOffline = "0" + Hours.ToString() + ":";
            }
            else
            {
                timeOffline = Hours.ToString() + ":";
            }
        }
        else
        {
            if (!ignoreHour00)
                timeOffline = "00:";
        }
        if (Minutes > 0)
        {
            if (Minutes < 10)
            {
                timeOffline += "0" + Minutes.ToString();
            }
            else
            {
                timeOffline += Minutes.ToString();
            }
        }
        else
        {
            timeOffline += "00";
        }
        if (Seconds > 0)
        {
            if (Seconds < 10)
            {
                timeOffline += ":0" + Seconds.ToString();
            }
            else
            {
                timeOffline += ":" + Seconds.ToString();
            }
        }
        else
        {
            timeOffline += ":00";
        }
        return timeOffline;

    }
    public static int GetSecondsLeft(int hours, int minutes, int seconds)
    {
        //Create Desired time
        DateTime target = new DateTime(2020, 1, 1, hours, minutes, seconds);

        //Get the current time
        DateTime now = System.DateTime.Now;

        //Convert both to seconds
        int targetSec = target.Hour * 60 * 60 + target.Minute * 60 + target.Second;
        int nowSec = now.Hour * 60 * 60 + now.Minute * 60 + now.Second;
        if (target.Hour < now.Hour)
        {
            targetSec += 24 * 60 * 60;
        }
        //Get the difference in seconds
        int diff = targetSec - nowSec;

        return diff;
    }
    public static List<int> GetIndexOfSmallestNumber(List<int> inputs)
    {
        List<int> result = new List<int>();
        int SmallestNumber = inputs[0];
        if (inputs.Count > 1)
        {
            for (int i = 1; i < inputs.Count; i++)
            {
                if (inputs[i] < SmallestNumber)
                    SmallestNumber = inputs[i];
            }
        }
        for (int i = 0; i < inputs.Count; i++)
        {
            if (inputs[i] == SmallestNumber)
                result.Add(i);
        }
        return result;
    }
    public static bool IsPassNextDay(DateTime dateTime)
    {
        if (dateTime.Year < DateTime.Now.Year)
        {
            return true;
        }
        else if (dateTime.Month < DateTime.Now.Month)
        {
            return true;
        }
        else if (dateTime.Day < DateTime.Now.Day)
        {
            return true;
        }
        return false;
    }
    public static bool IsPassNextDay(double dateTimeTimeStamp)
    {
        var dateTime = ConvertFromUnixTime(dateTimeTimeStamp);
        if (dateTime.Year < DateTime.Now.Year)
        {
            return true;
        }
        else if (dateTime.Month < DateTime.Now.Month)
        {
            return true;
        }
        else if (dateTime.Day < DateTime.Now.Day)
        {
            return true;
        }
        return false;
    }
    public static bool IsPassNextMonth(DateTime dateTime)
    {
        if (dateTime.Year < DateTime.Now.Year)
        {
            return true;
        }
        else if (dateTime.Month < DateTime.Now.Month)
        {
            return true;
        }
        return false;
    }
    public static bool IsPassNextMonth(double dateTimeTimeStamp)
    {
        var dateTime = ConvertFromUnixTime(dateTimeTimeStamp);
        if (dateTime.Year < DateTime.Now.Year)
        {
            return true;
        }
        else if (dateTime.Month < DateTime.Now.Month)
        {
            return true;
        }
        return false;
    }
    public static double ConvertToUnixTime(DateTime time)
    {
        DateTime epoch = new System.DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return (time - epoch).TotalSeconds;
    }
    public static double ConvertToUnixTimeNow()
    {
        DateTime epoch = new System.DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return (DateTime.Now - epoch).TotalSeconds;
    }

    public static DateTime ConvertFromUnixTime(double timeStamp)
    {
        DateTime epoch = new System.DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        try
        {
            DateTime time = epoch.AddSeconds(timeStamp);
            return time;
        }
        catch (System.Exception e)
        {
            return epoch;
        }
    }
    public static DateTime UnixTimeStampSeverToDateTime(long unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
        return dtDateTime;
    }
    public static int GetHours(float totalSeconds)
    {
        return (int)(totalSeconds / 3600f);
    }
    public static int GetMinutes(float totalSeconds)
    {
        return (int)((totalSeconds / 60) % 60);
    }
    public static int GetSeconds(float totalSeconds)
    {
        return (int)(totalSeconds % 60);
    }
    public static T GetRandomInList<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T Clone<T>(this T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable.", "source");
        }

        // Don't serialize a null object, simply return the default for that object
        if (System.Object.ReferenceEquals(source, null))
        {
            return default(T);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        using (stream)
        {
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }
    //public static T CloneJson<T>(this T source)
    //{
    //    var serialized = JsonConvert.SerializeObject(source);
    //    return JsonConvert.DeserializeObject<T>(serialized);
    //}
}
