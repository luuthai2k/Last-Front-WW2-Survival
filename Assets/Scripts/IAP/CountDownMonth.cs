using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownMonth : MonoBehaviour
{
    public TextMeshProUGUI txtTimeCountdown;
    [SerializeField] private GameObject countDownRoot;
    void Start()
    {
        StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        var delay = new WaitForSecondsRealtime(1);
        DateTime now = DateTime.Now;
        DateTime nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
        double timeFinish = Utils.ConvertToUnixTime(nextMonth);
        while (true)
        {
            double currentTime = Utils.ConvertToUnixTime(DateTime.Now);
            double deltaTime = timeFinish - currentTime;

            if (deltaTime <= 0)
            {
                countDownRoot.SetActive(false);
                yield break; 
            }
            txtTimeCountdown.text = ToolHelper.FormatTime(deltaTime);
            yield return delay;
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
