using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "ShopWeaponData", menuName = "SO Data/ShopWeaponData")]
public class SOShopWeaponData : ScriptableObject
{
    public TextAsset Source;
    public ShopWeaponDatas data;

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        Debug.Log("Load Data");
        data = JsonConvert.DeserializeObject<ShopWeaponDatas>(Source.text);
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
