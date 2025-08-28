using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Packs
{
    public Pack[] packs;
    public int itemOfferCooldown;
    public int crateOfferCooldown;
    public Pack GetPackById(string packId)
    {
        foreach (Pack pack in packs)
            if (pack.PackID == packId)
                return pack;
        return null;
    }
    public string GetPackPrice(string packId)
    {
        foreach (var pack in packs)
            if (pack.PackID == packId)
                return pack.Price;
        return "0";
    }
}
[System.Serializable]
public class Pack
{
    public string PackID;
    public string Price;
    public string PackName;
    public KeyValue[] Items;
    public KeyValue[] Weapons;
    public KeyValue[] Crates;
    public KeyValue[] Soldier;
    public int packLifeTime = 86400;
    public Pack()
    {
        Items = new KeyValue[0];
        Weapons = new KeyValue[0];
        Crates = new KeyValue[0];
        Soldier = new KeyValue[0];
    }
    public Pack(KeyValue[] items, KeyValue[] weapons, KeyValue[] crates, KeyValue[] soldier)
    {
        Items = items;
        Weapons = weapons;
        Crates = crates;
        Soldier = soldier;
    }                                                                       

}
public class IAPPackHelper
{
    public static Packs packs = null;
    public const string IAP_VALUE = "user_iap_value";
    public static string GetPackPrice(string packId)
    {
        if (packs == null)
        {
            string packsData = Resources.Load<TextAsset>("Data/IAP_Config").text;
            packs = JsonUtility.FromJson<Packs>(packsData);
        }
        return packs.GetPackPrice(packId);
    }
    public static Pack GetPack(string packId)
    {
        if (packs == null)
        {
            string packsData = Resources.Load<TextAsset>("Data/IAP_Config").text;
            packs = JsonUtility.FromJson<Packs>(packsData);
        }
        return packs.GetPackById(packId);
    }
    public static string GetTimeStampKey(string packId)
    {
        return packId + "_timestamp";
    }
    public static int GetCrateOfferCooldown()
    {
        if (packs == null)
        {
            string packsData = Resources.Load<TextAsset>("Data/IAP_Config").text;
            packs = JsonUtility.FromJson<Packs>(packsData);
        }
        return packs.crateOfferCooldown;
    }
    public static int GetItemOfferCooldown()
    {
        if (packs == null)
        {
            string packsData = Resources.Load<TextAsset>("Data/IAP_Config").text;
            packs = JsonUtility.FromJson<Packs>(packsData);
        }
        return packs.itemOfferCooldown;
    }
}