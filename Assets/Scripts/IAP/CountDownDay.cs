using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountDownDay : MonoBehaviour
{
    public TextMeshProUGUI txtTimeCountdown;
    [SerializeField] private GameObject countDownRoot;
    void OnEnable()
    {
        StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        var delay = new WaitForSecondsRealtime(1);
        DateTime now = DateTime.Now;
        DateTime nextDay = new DateTime(now.Year, now.Month, now.Day).AddDays(1);
        double timeFinish = Utils.ConvertToUnixTime(nextDay);
        while (true)
        {
            double currentTime = Utils.ConvertToUnixTime(DateTime.Now);
            double deltaTime = timeFinish - currentTime;

            if (deltaTime <= 0)
            {
                if (countDownRoot != null)
                {
                    countDownRoot.SetActive(false);
                }
                yield break;
            }
            txtTimeCountdown.text = ToolHelper.FormatTime(deltaTime);
            yield return delay;
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
  
}
