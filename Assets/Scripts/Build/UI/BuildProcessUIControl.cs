
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using System.Threading.Tasks;
using System.Diagnostics;
using System;

public class BuildProcessUIControl : MonoBehaviour
{
    public Image fillAmountImg;
    public BuildRewarditem[] buildRewarditems;
    int totalReward;
    public void SetUp(BuildProcessData[] buildProcessDatas, int index, float process)
    {

        SpawmReward(buildProcessDatas, index, process);
    }
    public async void SpawmReward(BuildProcessData[] buildProcessDatas, int index, float process)
    {
        totalReward = buildProcessDatas.Length;
        AsyncOperationHandle<SpriteAtlas> obj = Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Reward_Icon.spriteatlas");
        await obj.Task;
        for (int i = 0; i < buildRewarditems.Length; i++)
        {
            if(i< buildProcessDatas.Length)
            {
                buildRewarditems[i].gameObject.SetActive(true);
                KeyValue reward = buildProcessDatas[i].reward;
                buildRewarditems[i].SetUp(obj.Result.GetSprite("Icon_" + reward.Key), obj.Result.GetSprite($"Icon_{reward.Key}_Open"), reward);
                if (i < index)
                {
                    buildRewarditems[i].OnOpen();
                }
                else
                {
                    buildRewarditems[i].OnClose();
                }
            }
            else
            {
                buildRewarditems[i].gameObject.SetActive(false);
            }
           
        }
        UpdateProcess(index, process, 0.1f);
    }
    public void UpdateProcess(int index,float process,float duration)
    {
        float step = 1f / totalReward;
        fillAmountImg.DOFillAmount((index + process) * step, duration).OnComplete(() =>
        {
            if (process == 1)
            {
                buildRewarditems[index].OnOpen();
            }
        });
    }
   
}
