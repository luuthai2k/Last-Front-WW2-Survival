using DG.Tweening;
using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrenadeLootView : MonoBehaviour
{
    public GameObject grenadeItemPropPrefab;
    public RectTransform targetRectTrans;
    public RectTransform grenadeAmountViewRectTrans;
    public TextMeshProUGUI  grenadeTxt;
    int amount;
    public void SpawmGrenadeEffect(Vector3 spawnPos, int amount)
    {
        AudioController.Instance.PlaySfx(GameConstain.REWARD_POP);
        this.amount = DataController.Instance.Grenade - amount;
        grenadeTxt.text = this.amount.ToString();
        StopAllCoroutines();
        grenadeAmountViewRectTrans.DOAnchorPosX(-100, 0.5f);
        for (int i = 0; i < amount; i++)
        {
            GameObject go = SmartPool.Instance.Spawn(grenadeItemPropPrefab, transform);
            go.transform.position = spawnPos;
            go.GetComponent<ItemProp>().MoveTween(targetRectTrans.position, spawnPos, new Vector2(150, 150), targetRectTrans.sizeDelta, 0.8f, i * 0.05f + 0.05f,
                (() =>
                {
                    SmartPool.Instance.Despawn(go);
                    this.amount++;
                    grenadeTxt.text = this.amount.ToString();
                    if (i == amount)
                    {
                        StartCoroutine(DelayReturn());
                    }
                }));
        }

    }
    IEnumerator DelayReturn()
    {
        yield return new WaitForSeconds(0.25f);
        grenadeAmountViewRectTrans.DOAnchorPosX(100, 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);

    }
   
   
}
