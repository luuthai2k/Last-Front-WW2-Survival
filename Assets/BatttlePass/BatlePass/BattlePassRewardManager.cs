using DG.Tweening;
using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class BattlePassRewardManager : MonoBehaviour
{
   
    [SerializeField] private Scrollbar verticalScrollbar;
    public Image fillImg;
    public BattlePassLevel[] battlePassLevels;
    public Image[] levelIcon;
    public Sprite levelSprite;
    public GameObject goLock;
    private BattlePassDataSave dataSave;
    public RectTransform processRectTrans;
    private bool isUnlockVipPass = false;
    private int maxLevel;
  

    void OnEnable()
    {
        SetUpData();

    }
    public void SetUpData()
    {
        dataSave = DataController.Instance.GetGameData().battlePassDataSave;
        isUnlockVipPass = DataController.Instance.IsBuyIAPPack("weup.ww2.duty.frontline.zone.battlepasspremium");
        maxLevel = 40;
        CheckNewLevelComplete();
        if (dataSave.level > 0)
        {
            SetUpLevel();
            Vector2 anchoredPos = processRectTrans.anchoredPosition;
            anchoredPos.y = (-35) - 380 * dataSave.level;
            processRectTrans.anchoredPosition = anchoredPos;
        }
       
    }
    void CheckNewLevelComplete()
    {
        if (dataSave.level >= maxLevel) return;
        if (dataSave.xp >= BattlePassHelper.GetXPLevel(dataSave.level))
        {
            dataSave.xp -= BattlePassHelper.GetXPLevel(dataSave.level);
            dataSave.rewardFreeCanCollect.Add(dataSave.level);
            dataSave.rewardVipCanCollect.Add(dataSave.level);
            dataSave.level++;
            dataSave = new BattlePassDataSave();
            dataSave = DataController.Instance.GetGameData().battlePassDataSave;
            CheckNewLevelComplete();
        }
    }
    void SetUpLevel()
    {
        for (int i = 0; i < dataSave.level; i++)
        {
            levelIcon[i].sprite = levelSprite;
            if (dataSave.rewardFreeCanCollect.Contains(i))
            {
                battlePassLevels[i].CanClaimRevardFree();
            }
            else
            {
                battlePassLevels[i].OnCollectedFreeReward();
            }
            if (isUnlockVipPass)
            {
                goLock.SetActive(false);
                if (dataSave.rewardVipCanCollect.Contains(i))
                {
                    battlePassLevels[i].CanClaimRevardVip();
                }
                else
                {
                    battlePassLevels[i].OnCollectedVipReward();
                }

            }
        }
    }
    public void UnLockVipPass()
    {
        for (int i = 0; i < dataSave.level; i++)
        {

            if (dataSave.rewardVipCanCollect.Contains(i))
            {
                battlePassLevels[i].CanClaimRevardVip();
            }
            else
            {
                battlePassLevels[i].OnCollectedVipReward();
            }


        }
    }
   
}
