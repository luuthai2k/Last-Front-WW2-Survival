using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "FirstWeaponInGameData", menuName = "SO Data/FirstWeaponInGameData")]
public class SOFirstWeaponInGameData : ScriptableObject
{
    public TextAsset Source;
    public List<WeaponInGameData> WeaponInGameDatas;
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Debug.Log("Load Data");
        WeaponInGameDatas = JsonConvert.DeserializeObject<List<WeaponInGameData>>(Source.text);
    }
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        if (Source != null)
        {
            string json = JsonConvert.SerializeObject(WeaponInGameDatas, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Resources/Data/" + Source.name + ".json", json);
        }
        else
        {
            Debug.LogError("Source TextAsset is not assigned!");
        }
    }
}
