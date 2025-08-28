using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static BattlePassMissionUIItem;

[System.Serializable]
public class BattlePassLevelData
{
    public int level;
    public int xpRequired;
    public Pack freeRewards;
    public Pack vipRewards;
}
[System.Serializable]
public class BattlePassMissionsData
{
    public int id;
    public MissionType type;
    public int amount;
    public int xp;
}
public class BattlePassHelper
{
    public static BattlePassLevelData[] battlePassLevelDatas = null;
    public static BattlePassMissionsData[] battlePassMissionsDatas = null;
    public const string IAP_VALUE = "user_iap_value";
    public static BattlePassLevelData GetLevelData(int index)
    {
        if (battlePassLevelDatas == null)
        {
            string packsData = Resources.Load<TextAsset>("Data/BattlePass_Level_Data").text;
            battlePassLevelDatas = JsonConvert.DeserializeObject<BattlePassLevelData[]>(packsData);
        }
        return battlePassLevelDatas[index];
    }
    public static int GetXPLevel(int index)
    {
        return GetLevelData(index).xpRequired;
    }
    public static int MaxLevel()
    {
        if (battlePassLevelDatas == null)
        {
            string packsData = Resources.Load<TextAsset>("Data/BattlePass_Level_Data").text;
            battlePassLevelDatas = JsonConvert.DeserializeObject<BattlePassLevelData[]>(packsData);
        }
        return battlePassLevelDatas.Length;
    }
    public static BattlePassMissionsData[] GetMission()
    {
        if (battlePassMissionsDatas == null)
        {
            string packsData = Resources.Load<TextAsset>("Data/BattlePass_Mission_Data").text;
            battlePassMissionsDatas = JsonConvert.DeserializeObject<BattlePassMissionsData[]>(packsData);
        }
        return battlePassMissionsDatas;
    }

}