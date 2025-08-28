using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPanelController : MonoBehaviour
{
    [SerializeField] protected GameObject rewardElementPrefab, weaponCardElementPrefab;
    [SerializeField] protected Transform[] rewardParents;
    protected List<GameObject> activeRewardElements;
  
    public GameObject Spawm(KeyValue[] items)
    {
        GameObject go = Instantiate(gameObject);
        go.GetComponent<RewardPanelController>().SpawnReward(items);
        return go;
    }
    public void SpawnReward(KeyValue[] items)
    {
        activeRewardElements = new List<GameObject>();
        int indexParent;
        for (int i = 0; i < items.Length; i++)
        {
            indexParent = i / 3;
            if (!rewardParents[indexParent].gameObject.activeSelf)
            {
                rewardParents[indexParent].gameObject.SetActive(true);
            }
            switch (items[i].Key)
            {
                case "Cash":
                case "Gold":
                case "Metal":
                case "Grenade":
                case "MedKit":
                case "RemoveAds":
                    activeRewardElements.Add(rewardElementPrefab.GetComponent<RewardElement>().Spawn(items[i], rewardParents[indexParent]));
                    break;
                default:
                    var data = items[i].Key.Split("_");
                    switch (data[0])
                    {
                        case "Card":
                            var wigd = DataController.Instance.GetWeaponIngameData((WeaponType)int.Parse(data[1]), int.Parse(data[2]));
                            if (wigd != null)
                            {
                                KeyValue card = new KeyValue($"Card_{(int)wigd.weaponType}_{wigd.ID}", items[i].Value);
                                activeRewardElements.Add(weaponCardElementPrefab.GetComponent<WeaponCardElement>().Spawn(wigd, card, rewardParents[indexParent]));
                            }
                            break;
                    }

                    break;

            }
          
        }
    }
    public void OnClickClose()
    {
        gameObject.SetActive(false);
        TopBarController.Instance.UpdateText();
    }
}
