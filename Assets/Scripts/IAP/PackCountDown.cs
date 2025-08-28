using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PackCountDown : MonoBehaviour
{
    [Tooltip("This pack id will automatic sync with the IAP Button pack ")]
    [SerializeField] private string packId ;
    [Tooltip("This pack count down will automatic reset when reach 0")]
    [SerializeField] private bool shouldResetPackCountdown = false;
    [SerializeField] private TextMeshProUGUI coundownText;
    [SerializeField] private GameObject countDownRoot;
    public double packLifeTime = 0;
    private void OnEnable()
    {
        SetUp();
    }
    void SetUp()
    {
        double now = (double)GlobalTimer.Instance.GetUnixTimeStampNow();
        double originPackLifeTime = IAPPackHelper.GetPack(packId).packLifeTime;
        packLifeTime = originPackLifeTime + (double)DataController.Instance.GetTimeStamp(IAPPackHelper.GetTimeStampKey(packId)) - now;
        if (packLifeTime <= 0&& shouldResetPackCountdown)
        {
            packLifeTime = originPackLifeTime;
        }
        coundownText.text = ToolHelper.GetTextTime(packLifeTime);
        StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        var delay = new WaitForSeconds(1);
        while (packLifeTime > 0)
        {
            coundownText.text = ToolHelper.FormatTime(packLifeTime);
            yield return delay;
            packLifeTime--;
        }
        countDownRoot.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

