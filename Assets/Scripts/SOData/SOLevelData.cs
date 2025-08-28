using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "LevelDataSave", menuName = "SO Data/LevelDataSave")]
public class SOLevelData : ScriptableObject
{
    public TextAsset Source;
    public LevelData[] levelData;
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Debug.Log("Load Data");
        levelData = JsonConvert.DeserializeObject<LevelData[]>(Source.text);
    }
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        if (Source != null)
        {
            string json = JsonConvert.SerializeObject(levelData, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Resources/Data/" + Source.name + ".json", json);
        }
        else
        {
            Debug.LogError("Source TextAsset is not assigned!");
        }
    }
}
