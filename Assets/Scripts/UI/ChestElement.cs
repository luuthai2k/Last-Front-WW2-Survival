using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestElement : MonoBehaviour
{
    public GameObject goOpenFx,goIconClose,goIconOpen;
    public RewardElement rewardElement;
    public TextMeshProUGUI multiTxt;
    public Button button;
    public void OnOpen(KeyValue reward)
    {
        rewardElement.Init(reward);
        rewardElement.gameObject.SetActive(true);
        goOpenFx.gameObject.SetActive(true);
        goIconClose.gameObject.SetActive(false);
        goIconOpen.gameObject.SetActive(true);
        button.interactable = false;
        multiTxt.transform.parent.gameObject.SetActive(false);

    }
    public void OnOpen(KeyValue reward,Sprite sprite)
    {
        rewardElement.Init(reward,sprite);
        goOpenFx.gameObject.SetActive(true);
        goIconClose.gameObject.SetActive(false);
        goIconOpen.gameObject.SetActive(true);
        button.interactable = false;
        multiTxt.transform.parent.gameObject.SetActive(false);
        StartCoroutine(DelayDisplayReward());
    }
  IEnumerator DelayDisplayReward()
    {
        yield return new WaitForSeconds(0.5f);
        rewardElement.gameObject.SetActive(true);
    }
    public void UpdateMultiTxt(int multi)
    {
      
        if (multi == 1|| !button.interactable)
        {
            multiTxt.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            multiTxt.text = $"x{multi}";
            multiTxt.transform.parent.gameObject.SetActive(true);
        }
    }
}
