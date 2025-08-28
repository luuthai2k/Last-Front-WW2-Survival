using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "BattlePassLevelData", menuName = "SO Data/BattlePassLevelData")]
public class SOBattlePassLevelData : ScriptableObject
{
   
    public TextAsset Source;
    public BattlePassLevelData[] battlePassLevelDatas;
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Debug.Log("Load Data");
        battlePassLevelDatas = JsonConvert.DeserializeObject <BattlePassLevelData[]>(Source.text);
    }
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        if (Source != null)
        {
            string json = JsonConvert.SerializeObject(battlePassLevelDatas, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Resources/Data/" + Source.name + ".json", json);
        }
        else
        {
            Debug.LogError("Source TextAsset is not assigned!");
        }
    }
}

