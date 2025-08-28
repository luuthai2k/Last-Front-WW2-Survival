using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "IAPConfigData", menuName = "SO Data/IAPConfigData")]

public class SOIAPConfig : ScriptableObject
{
    public TextAsset Source;
    public Packs packDatas;
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Debug.Log("Load Data");
        packDatas = JsonConvert.DeserializeObject<Packs>(Source.text);
    }
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        if (Source != null)
        {
            string json = JsonConvert.SerializeObject(packDatas, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Resources/Data/" + Source.name + ".json", json);
        }
        else
        {
            Debug.LogError("Source TextAsset is not assigned!");
        }
    }
}
