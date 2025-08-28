using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CompleteBattlePassMissionPopup : MonoBehaviour
{
    public List<GameObject> missionsCompleteObj = new List<GameObject>();
    public RectTransform contentRectTrans;
    private KeyValue reward;
    public int totalxp;
    public BattlePassRewardPanel rewardPanel;
    private void OnEnable()
    {
        contentRectTrans.sizeDelta = new Vector2(contentRectTrans.sizeDelta.x, 280 * missionsCompleteObj.Count);
       
       
    }
    public void AddMission(GameObject go, int amount)
    {
        go.transform.SetParent(contentRectTrans);
        go.transform.localScale = Vector3.one;
        go.SetActive(true);
        missionsCompleteObj.Add(go);
        totalxp +=amount;
    }
    public void Continue()
    {
        reward = new KeyValue();
        reward.Key = "400_1_1";
        reward.Value = totalxp.ToString();
        List<KeyValue> Items = new List<KeyValue>() { reward };
        //rewardPanel.Spawn(Items);
        StartCoroutine(CoroutineClosePanel());
    }
    IEnumerator CoroutineClosePanel()
    {
        if (GetComponentInChildren<Animator>())
            GetComponentInChildren<Animator>().Play("Disappear");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
