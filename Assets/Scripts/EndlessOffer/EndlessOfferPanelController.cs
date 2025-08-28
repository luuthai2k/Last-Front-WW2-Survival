using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndlessOfferPanelController : MonoBehaviour
{
    [SerializeField] private EndlessOfferPack[] packs;
    [SerializeField] private GameObject[] arrows;
    private Vector3[] rewardPacksPos;
    public RectTransform contentRectTrans;
    int process;
    [SerializeField] private TextMeshProUGUI coundownText;
    public double packLifeTime = 0;
    private void Start()
    {
        rewardPacksPos = new Vector3[packs.Length];
        for (int i = 0; i < packs.Length; i++)
        {
            rewardPacksPos[i] = packs[i].transform.localPosition;
        }
        SetUp();
    }
    private void OnEnable()
    {
        UpdateCountDownTime();
    }
    public void UpdateCountDownTime()
    {
        double now = (double)GlobalTimer.Instance.GetUnixTimeStampNow();
        double originPackLifeTime = 172800;
        packLifeTime = originPackLifeTime + (double)DataController.Instance.GetTimeStamp(IAPPackHelper.GetTimeStampKey("weup.ww2.duty.frontline.zone.endlessoffer")) - now;
        if (packLifeTime <= 0)
        {
            packLifeTime = 172800;
            PlayerPrefs.SetInt(GameConstain.ENDLESS_OFFER_PROCESS, 0);
        }
        coundownText.text = ToolHelper.GetTextTime(packLifeTime);
        StartCoroutine(CountDown());
    }
    public void SetUp()
    {
       
        process = PlayerPrefs.GetInt(GameConstain.ENDLESS_OFFER_PROCESS, 0);
        Vector2 sizeDelta = contentRectTrans.sizeDelta;
        sizeDelta.y = (packs.Length - process+1) / 2 * 468 + 300;
        contentRectTrans.sizeDelta = sizeDelta;
        if (process == 0) 
        {
            packs[0].Unlock();
            return;
        } 
        for (int i = packs.Length - 1; i >= 0; i--)
        {
            if (i < process)
            {
                packs[i].gameObject.SetActive(false);
                arrows[arrows.Length-1 - i].gameObject.SetActive(false);
            }
            else
            {
                if (i == process)
                {
                    packs[i].Unlock();
                }
                packs[i].transform.localPosition=rewardPacksPos[i - process];
            }

        }
      
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
        gameObject.SetActive(false);
    }
    public void UpdateProcess()
    {
        process++;
        if (process >= 20)
        {
            gameObject.SetActive(false);
        }
        Vector2 sizeDelta = contentRectTrans.sizeDelta;
        sizeDelta.y = (packs.Length - process+1) / 2 * 468 + 300;
        contentRectTrans.sizeDelta = sizeDelta;
        PlayerPrefs.SetInt(GameConstain.ENDLESS_OFFER_PROCESS, process);
        for (int i = packs.Length - 1; i >= 0; i--)
        {
            if (i < process)
            {
                packs[i].gameObject.SetActive(false);
                arrows[arrows.Length-1 - i].gameObject.SetActive(false);
            }
            else
            {
                if (i == process)
                {
                    packs[i].Unlock();
                }
                packs[i].transform.DOLocalMove(rewardPacksPos[i - process], 0.5f);
            }

        }


    }
    public void OnClose()
    {
        gameObject.SetActive(false);
    }
    public void OnClickCloseBtn()
    {
        OnClose();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
