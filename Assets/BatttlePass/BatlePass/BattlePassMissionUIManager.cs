using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePassMissionUIManager : MonoBehaviour
{
    public Image fillImg;
    public TextMeshProUGUI levelText, processText;
    public List<BattlePassMissionUIItem> battleMissionItems = new List<BattlePassMissionUIItem>();
    public CompleteAlllMissionsAllItem completeAllMissionItems;
    BattlePassDataSave dataSave;
    private int xp;
    public RectTransform contentTrans;
    int totalMission;
    public void Start()
    {
        SetUpData();
    }

    public void SetUpData()
    {
        dataSave = DataController.Instance.GetGameData().battlePassDataSave;
        if (dataSave.ListMission.Count == 0 && dataSave.numberMissionComplete != -1)
        {
            BattlePassController.Instance.ResetBattlePassMissions();
        }
        xp = dataSave.xp;
        SetUpMission();
        if (dataSave.level < BattlePassHelper.MaxLevel())
        {
            processText.text = dataSave.xp.ToString() + "/" + BattlePassHelper.GetXPLevel(dataSave.level).ToString();
            fillImg.fillAmount = (float)dataSave.xp / BattlePassHelper.GetXPLevel(dataSave.level);
            levelText.text = (dataSave.level + 1).ToString();
        }
        else
        {
            processText.text = "Full";
            fillImg.fillAmount = 1;
            levelText.text = BattlePassHelper.MaxLevel().ToString();
        }
    }
    public void SetUpMission()
    {
       
        for (int i = 0; i < DataController.Instance.GetGameData().battlePassDataSave.ListMission.Count; i++)
        {
            battleMissionItems[i].gameObject.SetActive(true);
            battleMissionItems[i].SetUpMissionData(DataController.Instance.GetGameData().battlePassDataSave.ListMission[i]);
            totalMission++;
        }
        if (DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete != -1)
        {
            completeAllMissionItems.gameObject.SetActive(true);
            completeAllMissionItems.SetUpMissionData();
            totalMission++;
        }
        Vector2 sizeDelta = contentTrans.sizeDelta;
        sizeDelta.y = totalMission * (220)+200;
        contentTrans.sizeDelta = sizeDelta;
    }
    public void UpdateProcess(int xp,KeyValue mission=null)
    {
        dataSave.xp += xp;
        StartCoroutine(CoroutineUpdateProcess());
        if (mission != null)
        {
            dataSave.ListMission.Remove(mission);
        }
        MainMenuUIManager.Instance.battlePassPanelController.CheckMissionNotification(dataSave);
    }
    IEnumerator CoroutineUpdateProcess()
    {
        int level = dataSave.level;
        while (xp < dataSave.xp)
        {
            int remainingXP = dataSave.xp - xp;
            int incrementAmount = Mathf.Max(1, remainingXP / 10);
            xp += incrementAmount;
            xp = Mathf.Clamp(xp, 0, BattlePassHelper.GetXPLevel(dataSave.level));
            processText.text = xp.ToString() + "/" + BattlePassHelper.GetXPLevel(dataSave.level).ToString();
            fillImg.fillAmount = (float)xp / BattlePassHelper.GetXPLevel(dataSave.level);
            if (xp >= BattlePassHelper.GetXPLevel(dataSave.level))
            {
                xp = 0;
                dataSave.xp -= BattlePassHelper.GetXPLevel(dataSave.level);
                dataSave.rewardFreeCanCollect.Add(dataSave.level);
                dataSave.rewardVipCanCollect.Add(dataSave.level);
                dataSave.level++;
                levelText.text = (dataSave.level+1).ToString();
            }
            yield return new WaitForSeconds(0.01f);
        }
        if (level >= BattlePassHelper.MaxLevel())
        {
            levelText.text = "Max";
            fillImg.fillAmount = 1;
        }
        DataController.Instance.SaveData();
    }
}
