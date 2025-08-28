using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "AirdropWeaponData ", menuName = "SO Data/AirdropWeaponData ")]

public class SOAirdropWeaponData : ScriptableObject
{
    public TextAsset Source;
    public KeyValue[]  datas;
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Debug.Log("Load Data");
        datas = JsonConvert.DeserializeObject<KeyValue[]>(Source.text);
    }
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        if (Source != null)
        {
            string json = JsonConvert.SerializeObject(datas, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Resources/Data/" + Source.name + ".json", json);
        }
        else
        {
            Debug.LogError("Source TextAsset is not assigned!");
        }
    }
}
