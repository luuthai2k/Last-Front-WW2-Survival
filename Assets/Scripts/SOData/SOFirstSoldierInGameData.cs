using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "FirstSoldierInGameData ", menuName = "SO Data/FirstSoldierInGameData ")]

public class SOFirstSoldierInGameData : ScriptableObject
{
    public TextAsset Source;
    public List<SoldierInGameData> SoldierInGameDatas;
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Debug.Log("Load Data");
        SoldierInGameDatas = JsonConvert.DeserializeObject<List<SoldierInGameData>>(Source.text);
    }
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        if (Source != null)
        {
            string json = JsonConvert.SerializeObject(SoldierInGameDatas, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Resources/Data/" + Source.name + ".json", json);
        }
        else
        {
            Debug.LogError("Source TextAsset is not assigned!");
        }
    }
}
