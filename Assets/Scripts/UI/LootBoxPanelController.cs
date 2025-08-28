using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootBoxPanelController : MonoBehaviour
{
    public Transform rewardsTrans,chestShowRoomTrans,cameraTrans;
    public ParticleSystem chestOpenFx, chestDropFx;
    public Flash flash;
    [SerializeField] protected GameObject rewardElementPrefab,weaponCardElementPrefab,selectPrefab,goCollectBtn,goBoostBtn;
    [SerializeField] protected Transform[] rewardParents;
    protected List<GameObject> activeRewardElements;
    private Chest chest;
    public float timeDelay=1;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI cashTxt, goldTxt, metalTxt;
    public void UpdateText()
    {
        goldTxt.text = ToolHelper.FormatLong2(DataController.Instance.Gold);
        cashTxt.text = ToolHelper.FormatLong2(DataController.Instance.Cash);
        metalTxt.text= ToolHelper.FormatLong2(DataController.Instance.Metal);
    }
    public GameObject Spawm(string key)
    {
        GameObject go = Instantiate(gameObject);
        go.GetComponent<LootBoxPanelController>().GetChest(key);
        return go;
    } 
    public GameObject Spawm(string key, bool isTutorial)
    {
        GameObject go = Instantiate(gameObject);
        go.GetComponent<LootBoxPanelController>().GetChest(key,true);
        return go;
    }
    void Start()
    {
      
        UpdateText();
    }
    public void GetChest(string key,bool isTutorial=false)
    {
        UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>($"{key}.prefab").Completed += (UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj) =>
        {
            GameObject goChest = Instantiate(obj.Result, chestShowRoomTrans);
            chest = goChest.GetComponentInChildren<Chest>();
            chest.Init(isTutorial);
            chest.onDropChest += OnDropChest;
            chest.onOpenChest += OnOpenChest;
        };
    }
    public void OnOpenChest()
    {
        flash.Play();
        rewardsTrans.localScale = Vector3.one * 0.75f;
        StartCoroutine(SpawnReward(chest.GetRewardItems(), chest.GetRewardCards()));
    }
    public void OnDropChest()
    {
        chestDropFx.Play();
    }
    IEnumerator SpawnReward(List<KeyValue> items, List<KeyValue> cards)
    {
        activeRewardElements = new List<GameObject>();
        var main = chestOpenFx.main;
        main.duration =Mathf.Max( (items.Count+ cards.Count)*timeDelay);
        chestOpenFx.Simulate(2, false, false);
        chestOpenFx.Play();
        int indexParent;
        for (int i = 0; i < items.Count; i ++)
        {
            indexParent = i / 3;
            if (!rewardParents[indexParent].gameObject.activeSelf)
            {
                rewardParents[indexParent].gameObject.SetActive(true);
            }
            activeRewardElements.Add(rewardElementPrefab.GetComponent<RewardElement>().Spawn(items[i], rewardParents[indexParent]));
            yield return new WaitForSeconds(timeDelay);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            indexParent = (i + items.Count) / 3;
            if (!rewardParents[indexParent].gameObject.activeSelf)
            {
                rewardParents[indexParent].gameObject.SetActive(true);
            }
            activeRewardElements.Add(weaponCardElementPrefab.GetComponent<WeaponCardElement>().Spawn(chest.weaponInGames[i], cards[i], rewardParents[indexParent]));
            yield return new WaitForSeconds(timeDelay);
        }
        rewardsTrans.DOScale(1, 0.5f);
        cameraTrans.DOLocalMove(new Vector3(1.95f, 6f, 4f),0.5f);
        canvasGroup.DOFade(1, 0.5f);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
      
    }
    public void OnClickCollectBtn()
    {
        ClosePanel();
        TopBarController.Instance.UpdateText();
    }
    public void OnClickBoostBtn()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
            RandomReward();
        });
      
    }
    public void RandomReward()
    {
        canvasGroup.alpha = 0;
        goCollectBtn.gameObject.SetActive(false);
        goBoostBtn.gameObject.SetActive(false);
        int index = Random.Range(0, activeRewardElements.Count);
        StartCoroutine(DelaySelectReward(index));
        FirebaseServiceController.Instance.LogEvent($"REWARD_CHEST_BOOST");
    }
    public IEnumerator DelaySelectReward(int index)
    {
        GameObject goSelect = Instantiate(selectPrefab, activeRewardElements[0].transform);
        for (int i = 0; i < 2; i++)
        {
            for(int j = 0; j < activeRewardElements.Count; j++)
            {
                goSelect.transform.SetParent(activeRewardElements[j].transform);
                goSelect.transform.localPosition = Vector3.zero;
                yield return new WaitForSeconds(0.1f);
                if (i == 1 && j == index)
                {
                    RewardElement rewardElement = activeRewardElements[j].GetComponent<RewardElement>();
                    rewardElement.BoostReward();
                    yield return new WaitForSeconds(0.5f);
                    goCollectBtn.gameObject.SetActive(true);
                    canvasGroup.DOFade(1, 0.5f);
                    yield break;
                }

            }
        }

    }
}
