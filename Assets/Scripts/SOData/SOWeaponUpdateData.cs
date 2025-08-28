using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "WeaponUpdateData", menuName = "SO Data/WeaponUpdateData")]
public class SOWeaponUpdateData : ScriptableObject
{
    public TextAsset Source;
    public WeaponUpdateDatas data;
  
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Debug.Log("Load Data");
        data = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Source.text);
    }
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        if (Source != null)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Resources/Data/" + Source.name + ".json", json);
        }
        else
        {
            Debug.LogError("Source TextAsset is not assigned!");
        }
    }
}
