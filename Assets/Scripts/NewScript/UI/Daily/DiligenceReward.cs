using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiligenceReward : MonoBehaviour
{
    public int targetDay;
    public List<KeyValue> rewardItems;
    [SerializeField] private GameObject goTick;
    public List<KeyValue> GetRewards()
    {
        return rewardItems;
    }
    public void SetUp(int totalDay)
    {
        if(totalDay>= targetDay)
        {
            goTick.SetActive(true);
        }
    }
}
