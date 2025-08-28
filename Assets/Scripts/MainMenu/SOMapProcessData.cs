using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "MapProcessData", menuName = "SO Data/MapProcessData")]
public class SOMapProcessData : ScriptableObject
{
    public TextAsset Source;
    public float[] process;
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Debug.Log("Load Data");
        process = JsonConvert.DeserializeObject<float[]>(Source.text);
    }
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        if (Source != null)
        {
            string json = JsonConvert.SerializeObject(process, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Resources/Data/" + Source.name + ".json", json);
        }
        else
        {
            Debug.LogError("Source TextAsset is not assigned!");
        }
    }
}
