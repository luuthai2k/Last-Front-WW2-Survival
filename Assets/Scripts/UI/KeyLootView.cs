using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyLootView: MonoBehaviour
{
    public Transform[] targetTrans;
    public RectTransform keyAmountViewRectTrans;
    public Transform  keyItemTrans;
    public Image[] keyIcons;
    int keyCount;
  
 
    IEnumerator Start()
    {
        SetUp();
        keyAmountViewRectTrans.DOAnchorPosX(-100, 0.5f);
        AudioController.Instance.PlaySfx(GameConstain.KEY_COLLECT);
        yield return new WaitForSeconds(0.5f);
        keyItemTrans.DOScale(1, 0.75f);
        keyItemTrans.DOMove(targetTrans[keyCount-1].position,0.75f);
        yield return new WaitForSeconds(1f);
        keyItemTrans.gameObject.SetActive(false);
        keyIcons[keyCount-1].color = Color.white;
        keyAmountViewRectTrans.DOAnchorPosX(100, 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);

    }
    public void SetUp()
    {
        keyCount =Mathf.Max(1,DataController.Instance.Key);
        for (int i = 0; i < keyIcons.Length; i++)
        {
            if (i <= keyCount - 2)
            {
                keyIcons[i].color = Color.white;
            }
            else
            {
                keyIcons[i].color = Color.black;
            }
        }
    }

}
