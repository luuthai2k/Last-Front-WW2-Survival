using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildRewarditem : MonoBehaviour
{
    public KeyValue reward;
    public Image closeIconImg,openIconImg;
    public GameObject goClose, goOpend;
    public void SetUp(Sprite closeSprite, Sprite openSprite, KeyValue keyValue)
    {
        reward = keyValue;
        closeIconImg.sprite = closeSprite;
        openIconImg.sprite = openSprite;

    }
    public void OnOpen()
    {
        goClose.SetActive(false);
        goOpend.SetActive(true);
    }
    public void OnClose()
    {
        goClose.SetActive(true);
        goOpend.SetActive(false);
    }
}
