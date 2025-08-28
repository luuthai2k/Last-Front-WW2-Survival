using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class GamePlayMenu : MonoBehaviour
{
    public TextMeshProUGUI healthText,medKitAmountTxt,grenadeAmountTxt;
    public GameObject goAdsMedKitIcon, goAdsGrenadeIcon, goDanger;
    public List<SegmentProcess> segmentList;
    [SerializeField] private Image alarmFillImg;
    public CanvasGroup alarmCanvasGroup, badgeCanvasGroup;
    private List<BadgeType> queues=new List<BadgeType>();
    private void Start()
    {
        badgeCanvasGroup.alpha = 0;
        alarmCanvasGroup.alpha = 0;
    }

    public void Init(int totalWave)
    {
        ActiveSegment(totalWave);
        segmentList[0].processing.gameObject.SetActive(true);
        UpdateGrenadeText();
        UpdateMedKitText();
    }

    public void UpdateHealthText(int health)
    {
        healthText.text = health.ToString();
        if (health <= 50 && PlayerPrefs.GetInt(GameConstain.HEAL_TUT,0)== 0)
        {
            PlayerUIControl.Instance.isAim = false;
            PlayerPrefs.SetInt(GameConstain.HEAL_TUT, 1);
            Instantiate(Resources.Load<GameObject>("Tutorial/HealTutorialCanvas"));
        }
    }

    public void UpdateMedKitText()
    {
        if (DataController.Instance.MedKit>0)
        {
            medKitAmountTxt.text = DataController.Instance.MedKit.ToString();
            goAdsMedKitIcon.SetActive(false);
            medKitAmountTxt.gameObject.SetActive(true);
        }
        else
        {
            goAdsMedKitIcon.SetActive(true);
            medKitAmountTxt.gameObject.SetActive(false);
        }
    }

    public void UpdateGrenadeText()
    {
      
        if (DataController.Instance.Grenade > 0)
        {
            grenadeAmountTxt.text = DataController.Instance.Grenade.ToString();
            goAdsGrenadeIcon.SetActive(false);
            grenadeAmountTxt.gameObject.SetActive(true);
        }
        else
        {
            goAdsGrenadeIcon.SetActive(true);
            grenadeAmountTxt.gameObject.SetActive(false);
        }
    }
  
    public void ActiveSegment(int wave)
    {
        for(int i  = 0; i < segmentList.Count; i ++)
        {
            if(i < wave)
            {
                segmentList[i].root.SetActive(true);
                segmentList[i].processing.gameObject.SetActive(false);
            }
            else
            {
                segmentList[i].root.SetActive(false);
            }
               
        }

    }

    public void ShowProcessingInWave(int waveIndex)
    {
        segmentList[waveIndex].processing.gameObject.SetActive(true);
    }
    public void SetAlarm(bool isActive, float time = 0, Action OnComplete = null)
    {
        if (!alarmCanvasGroup.gameObject.activeSelf) return;
        if (isActive)
        {
            alarmCanvasGroup.DOKill();
            alarmFillImg.DOKill();
            alarmFillImg.fillAmount = 0;
            alarmCanvasGroup.alpha = 1;
            goDanger.SetActive(false);
            alarmFillImg.DOFillAmount(1, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                OnComplete?.Invoke();
                goDanger.SetActive(true);
            });
        }
        else
        {
            alarmCanvasGroup.DOFade(0, 0.5f);
        }
    }
    public void ShowBadge(BadgeType type)
    {
        if (badgeCanvasGroup.alpha != 0)
        {
            queues.Add(type);
            return;
        }
        int index = (int)type;
        badgeCanvasGroup.alpha = 1;
        badgeCanvasGroup.transform.GetChild(index).gameObject.SetActive(true);
        AudioController.Instance.PlaySfx(GameConstain.BADGE_SHOW);
        StartCoroutine(DelayHideBadge());
        IEnumerator DelayHideBadge()
        {
            yield return new WaitForSeconds(1.5f);
            badgeCanvasGroup.DOFade(0, 0.35f);
            yield return new WaitForSeconds(0.35f);
            badgeCanvasGroup.transform.GetChild(index).gameObject.SetActive(false);
            if (queues.Count > 0)
            {
                ShowBadge(queues[0]);
                queues.RemoveAt(0);
            }
        }
    }

    [Serializable]
    public class SegmentProcess
    {
        public GameObject root;

        public Image processing;
    }
  
}
public enum BadgeType
{
    HeadShoot,
    Eliminated,
    HatOff
}
