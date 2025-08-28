using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;
using System.Linq;


public class DataController : SingletonPersistent<DataController>
{
    [SerializeField] private ShopWeaponDatas shopWeaponDatas;
    [SerializeField] private WeaponUpdateDatas weaponUpdateDatas;
    [SerializeField] private SoldierUpdateDatas soldierUpdateDatas;
    [SerializeField] private UnlockWeaponDatas unlockWeaponDatas;
    [SerializeField] private BuildData[] buildDatas;
 
    [SerializeField] private List<KeyValue> airdropWeaponDatas=new List<KeyValue>();
    [SerializeField] private float[] mapProcessDatas;
    [SerializeField]private LevelData[] levelDatas;
    public int completedLevelsThisSession = 0;
    private string dataPath = "";
    private string dataPathBackUp = "";
    public GameDataSave gameData;
    public int levelSelect;
    public int difficultySelect;
    private bool isInitData = false,isNewLevel;
    public bool IsNewLevel { get { return isNewLevel; } set { isNewLevel = value; } }
    public int Level
    {
        get { return gameData.level; }
        set
        {
            completedLevelsThisSession++;
            gameData.level = value;
        }
    }
    public int Cash
    {
        get { return gameData.cash; }
        set
        {
            gameData.cash = value;
        }
    }
    public int Gold
    {
        get { return gameData.gold; }
        set
        {
            gameData.gold = value;
        }
    }
    public int Metal
    {
        get { return gameData.metal; }
        set
        {
            gameData.metal = value;
        }
    }
    public int MedKit
    {
        get { return gameData.medKid; }
        set
        {
            gameData.medKid = value;
        }
    }
    public int Grenade
    {
        get { return gameData.grenade; }
        set
        {
            gameData.grenade = value;
        }
    }
    public int Key
    {
        get { return gameData.key; }
        set
        {
            gameData.key = value;
        }
    }
    public int Build
    {
        get { return gameData.build; }
        set
        {
            gameData.build = value;
        }
    }
    public void AddCash(int value, bool sendMess = true, float delaySendMess = 0f)
    {
        Cash += value;
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnCashChange, new object[] { value }));
        if (!sendMess) return;
        StartCoroutine(DelaySendMess());
        IEnumerator DelaySendMess()
        {
            yield return new WaitForSeconds(delaySendMess);
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDataChange));
        }
    }
    public void AddGold(int value, bool sendMess = true, float delaySendMess = 0f)
    {
        Gold += value;
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnGoldChange, new object[] { value }));
        if (!sendMess) return;
        StartCoroutine(DelaySendMess());
        IEnumerator DelaySendMess()
        {
            yield return new WaitForSeconds(delaySendMess);
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDataChange));
        }
    }
    public void AddMetal(int value, bool sendMess = true, float delaySendMess = 0f)
    {
        Metal += value;
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnMetalChange, new object[] { value }));
        if (!sendMess) return;
        StartCoroutine(DelaySendMess());
        IEnumerator DelaySendMess()
        {
            yield return new WaitForSeconds(delaySendMess);
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDataChange));
        }

    }
    public void AddGrenade(int value, bool sendMess = true, float delaySendMess = 0f)
    {
        Grenade += value;
        //MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnMetalChange, new object[] { value }));
        if (!sendMess) return;
        StartCoroutine(DelaySendMess());
        IEnumerator DelaySendMess()
        {
            yield return new WaitForSeconds(delaySendMess);
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDataChange));
        }

    }
    public void AddMedKit(int value, bool sendMess = true, float delaySendMess = 0f)
    {
        MedKit += value;
        //MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnMetalChange, new object[] { value }));
        if (!sendMess) return;
        StartCoroutine(DelaySendMess());
        IEnumerator DelaySendMess()
        {
            yield return new WaitForSeconds(delaySendMess);
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDataChange));
        }

    }
    public void AddKey(int value, bool sendMess = true, float delaySendMess = 0f)
    {
        Key += value;
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnKeyChange, new object[] { value }));
        if (!sendMess) return;
        StartCoroutine(DelaySendMess());
        IEnumerator DelaySendMess()
        {
            yield return new WaitForSeconds(delaySendMess);
            MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnDataChange));
        }

    }
    private void Start()
    {
        isInitData = false;
        dataPath = Path.Combine(Application.persistentDataPath, "data.dat");
        dataPathBackUp = Path.Combine(Application.persistentDataPath, "dataBackUp.dat");
        LoadData();
    }
    protected override void OnRegistration()
    {
       
    }
    public void LoadData()
    {
        Debug.LogWarning("LoadData");
        LoadLocalData();
        if (Level == 0)
        {
            SceneController.Instance.LoadScene(GameConstain.GamePlay, "Mission_loading",true, false);
        }
        else
        {
            SceneController.Instance.LoadScene(GameConstain.MainMenu, "Main_menu_loading", true, false);
        }
       

    }
    public void LoadLocalData()
    {
        Debug.LogWarning("LoadLocalData");
        if (File.Exists(dataPath))
        {
            string data = "";
            try
            {
                data = File.ReadAllText(dataPath);
                gameData = JsonUtility.FromJson<GameDataSave>(data);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("System.Exception :" + e.Message);
                ResetGameDataFromBackUp();
            }
            if (ToolHelper.IsPassNextDay(gameData.saveTime))
            {
                ResetNewDay();
            }
        }
        else
        {
            Debug.Log("ResetGameData");
            ResetGameDataFromBackUp();
        }
        isInitData = true;
    }
    private void ResetGameDataFromBackUp()
    {
        if (File.Exists(dataPathBackUp))
        {
            try
            {
                string data = File.ReadAllText(dataPathBackUp);
                gameData = JsonUtility.FromJson<GameDataSave>(data);
                SaveData();
            }
            catch (System.Exception e)
            {
                ResetData();
            }
        }
        else
            ResetData();
    }
    public void ResetData()
    {
        Debug.LogWarning("ReSetDATA");
        Caching.ClearCache();
        gameData = new GameDataSave();
        Debug.LogWarning("ReSetDATADone");
        SaveData();
    }
    public void SaveData(/*bool postData = false*/)
    {
        gameData.saveTime = ToolHelper.ConvertToUnixTimeNow();
        string origin = JsonUtility.ToJson(gameData);
        File.WriteAllText(dataPath, origin);
    }
    public GameDataSave GetGameData()
    {
        return gameData;
    }
    public void SetGameData(GameDataSave gd)
    {
        gameData = gd;
    }
    public void LoadLevelData()
    {
        levelDatas= JsonConvert.DeserializeObject<LevelData[]>(Resources.Load<TextAsset>("Data/Level_Data").text);
    }
    public LevelData GetLevelData(int level)
    {
        if (levelDatas == null||levelDatas.Length==0)
        {
            LoadLevelData();
        }
        return levelDatas[level];
    }
    public LevelData GetCurrentLevelData()
    {
        if (Level > GameConstain.MAXLEVEL)
        {
            Level = GameConstain.MAXLEVEL;
        }
        return GetLevelData(Level);
    }
    public DifficultyData GetUnlockDifficultyData(int level)
    {
        if (gameData.unlockDifficultyData.Count < level)
        {
            for(int i= gameData.unlockDifficultyData.Count; i < level; i++)
            {
                gameData.unlockDifficultyData.Add(new DifficultyData());
            }
        }
        return gameData.unlockDifficultyData[level-1];
;    }
    public DifficultyData GetCurrentUnlockDifficultyData()
    {
        return GetUnlockDifficultyData(Level);
    }
    public List<WeaponInGameData> GetWeaponIngameDatas(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.SMG:
                if (gameData.SMGInGameDatas.Count == 0)
                {
                    gameData.SMGInGameDatas= JsonConvert.DeserializeObject<List<WeaponInGameData>>(Resources.Load<TextAsset>("Data/First_SMG_Ingame_Data").text);
                }
                return gameData.SMGInGameDatas;
            case WeaponType.ShotGun:
                if (gameData.ShotGunInGameDatas.Count == 0)
                {
                    gameData.ShotGunInGameDatas = JsonConvert.DeserializeObject<List<WeaponInGameData>>(Resources.Load<TextAsset>("Data/First_ShotGun_Ingame_Data").text);
                }
                return gameData.ShotGunInGameDatas;
            case WeaponType.Sniper:
                if (gameData.SniperInGameDatas.Count == 0)
                {
                    gameData.SniperInGameDatas = JsonConvert.DeserializeObject<List<WeaponInGameData>>(Resources.Load<TextAsset>("Data/First_Sniper_Ingame_Data").text);
                }
                return gameData.SniperInGameDatas;
            default:
                if (gameData.MachineGunInGameDatas.Count == 0)
                {
                    gameData.MachineGunInGameDatas = JsonConvert.DeserializeObject<List<WeaponInGameData>>(Resources.Load<TextAsset>("Data/First_Machine_Gun_Ingame_Data").text);
                }
                return gameData.MachineGunInGameDatas;
        }
    }
    public WeaponInGameData GetWeaponIngameData(WeaponType type,int index)
    {
     
        switch (type)
        {
            case WeaponType.SMG:
                if (gameData.SMGInGameDatas.Count <= index)
                {
                    UpdateSMGIngameData();
                }
                return gameData.SMGInGameDatas[index];
            case WeaponType.ShotGun:
                Debug.LogError(index);
                if (gameData.ShotGunInGameDatas.Count <= index)
                {
                    UpdateShootGunIngameData();

                }
                return gameData.ShotGunInGameDatas[index];
            case WeaponType.Sniper:
                if (gameData.SniperInGameDatas.Count <= index)
                {
                    UpdateSniperIngameData();
                }
                return gameData.SniperInGameDatas[index];
            default:
                if (gameData.MachineGunInGameDatas.Count <= index)
                {
                    UpdateMachineGunIngameData();
                }
                return gameData.MachineGunInGameDatas[index];
        }
    }
    public WeaponInGameData GetWeaponIngameData(WeaponType type, string id)
    {

        switch (type)
        {
            case WeaponType.SMG:
                foreach (var weapon in gameData.SMGInGameDatas)
                {
                    if (weapon.ID == id)
                    {
                        return weapon;
                    }
                }
                return null;
            case WeaponType.Sniper:
                foreach (var weapon in gameData.SniperInGameDatas)
                {
                    if (weapon.ID == id)
                    {
                        return weapon;
                    }
                }
                return null;
            default:
                foreach (var weapon in gameData.MachineGunInGameDatas)
                {
                    if (weapon.ID == id)
                    {
                        return weapon;
                    }
                }
                return null;
        }
    }
    public void UpdateSMGIngameData()
    {
        var SMGData = JsonConvert.DeserializeObject<List<WeaponInGameData>>(Resources.Load<TextAsset>("Data/First_SMG_Ingame_Data").text);
        int existingCount = gameData.SMGInGameDatas.Count;
        if (existingCount < SMGData.Count)
        {
            gameData.SMGInGameDatas.AddRange(SMGData.Skip(existingCount));
        }
    }
    public void UpdateShootGunIngameData()
    {
        var ShootGunData = JsonConvert.DeserializeObject<List<WeaponInGameData>>(Resources.Load<TextAsset>("Data/First_ShotGun_Ingame_Data").text);
        int existingCount = gameData.ShotGunInGameDatas.Count;
        if (existingCount < ShootGunData.Count)
        {
            gameData.ShotGunInGameDatas.AddRange(ShootGunData.Skip(existingCount));
        }
    }
    public void UpdateSniperIngameData()
    {
        var SniperData = JsonConvert.DeserializeObject<List<WeaponInGameData>>(Resources.Load<TextAsset>("Data/First_Sniper_Ingame_Data").text);
        int existingCount = gameData.SniperInGameDatas.Count;
        if (existingCount < SniperData.Count)
        {
            gameData.SniperInGameDatas.AddRange(SniperData.Skip(existingCount));
        }
    }
    public void UpdateMachineGunIngameData()
    {
        var MachineGunData = JsonConvert.DeserializeObject<List<WeaponInGameData>>(Resources.Load<TextAsset>("Data/First_Machine_Gun_Ingame_Data").text);
        int existingCount = gameData.MachineGunInGameDatas.Count;
        if (existingCount < MachineGunData.Count)
        {
            gameData.MachineGunInGameDatas.AddRange(MachineGunData.Skip(existingCount));
        }
    }
    public ShopWeaponData GetShopWeaponData(WeaponType type,int index)
    {
        if (shopWeaponDatas == null)
        {
            shopWeaponDatas = JsonConvert.DeserializeObject<ShopWeaponDatas>(Resources.Load<TextAsset>("Data/Shop_Weapon_Data").text);
        }
        switch (type)
        {
            case WeaponType.SMG:
                if (shopWeaponDatas.SMGData.Length == 0)
                {
                    shopWeaponDatas = JsonConvert.DeserializeObject<ShopWeaponDatas>(Resources.Load<TextAsset>("Data/Shop_Weapon_Data").text);
                }
                return shopWeaponDatas.SMGData[index];
            case WeaponType.ShotGun:
                if (shopWeaponDatas.ShotGunData.Length == 0)
                {
                    shopWeaponDatas = JsonConvert.DeserializeObject<ShopWeaponDatas>(Resources.Load<TextAsset>("Data/Shop_Weapon_Data").text);
                }
                return shopWeaponDatas.ShotGunData[index];
            case WeaponType.Sniper:
                if (shopWeaponDatas.SniperData.Length == 0)
                {
                    shopWeaponDatas = JsonConvert.DeserializeObject<ShopWeaponDatas>(Resources.Load<TextAsset>("Data/Shop_Weapon_Data").text);
                }
                return shopWeaponDatas.SniperData[index];
            default:
                if (shopWeaponDatas.MachineGunData.Length == 0)
                {
                    shopWeaponDatas = JsonConvert.DeserializeObject<ShopWeaponDatas>(Resources.Load<TextAsset>("Data/Shop_Weapon_Data").text);
                }
                return shopWeaponDatas.MachineGunData[index];
        }
    }
    public WeaponUpdateData GetWeaponUpdateData(WeaponType type, int index)
    {
        if (weaponUpdateDatas == null)
        {
            weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
        }
        switch (type)
        {
            case WeaponType.SMG:
                if (weaponUpdateDatas.SMGData.Length == 0)
                {
                    weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
                }
                return weaponUpdateDatas.SMGData[index];
            case WeaponType.ShotGun:
                if (weaponUpdateDatas.ShotGunData.Length == 0)
                {
                    weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
                }
                return weaponUpdateDatas.ShotGunData[index];
            case WeaponType.Sniper:
                if (weaponUpdateDatas.SniperData.Length == 0)
                {
                    weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
                }
                return weaponUpdateDatas.SniperData[index];
            default:
                if (weaponUpdateDatas.MachineGunData.Length == 0)
                {
                    weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
                }
                return weaponUpdateDatas.MachineGunData[index];
        }
    }
    public WeaponUpdateData GetWeaponUpdateData(WeaponType type, string id)
    {
        if (weaponUpdateDatas == null)
        {
            weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
        }
        switch (type)
        {
            case WeaponType.SMG:
                if (weaponUpdateDatas.SMGData.Length == 0)
                {
                    weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
                }
                foreach(var weapon in weaponUpdateDatas.SMGData)
                {
                    if (weapon.ID == id)
                    {
                        return weapon;
                    }
                }
                return null;
            case WeaponType.ShotGun:
                if (weaponUpdateDatas.ShotGunData.Length == 0)
                {
                    weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
                }
                foreach (var weapon in weaponUpdateDatas.ShotGunData)
                {
                    if (weapon.ID == id)
                    {
                        return weapon;
                    }
                }
                return null;
            case WeaponType.Sniper:
                if (weaponUpdateDatas.SniperData.Length == 0)
                {
                    weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
                }
                foreach (var weapon in weaponUpdateDatas.SniperData)
                {
                    if (weapon.ID == id)
                    {
                        return weapon;
                    }
                }
                return null;
            default:
                if (weaponUpdateDatas.MachineGunData.Length == 0)
                {
                    weaponUpdateDatas = JsonConvert.DeserializeObject<WeaponUpdateDatas>(Resources.Load<TextAsset>("Data/Weapon_Update_Data").text);
                }
                foreach (var weapon in weaponUpdateDatas.MachineGunData)
                {
                    if (weapon.ID == id)
                    {
                        return weapon;
                    }
                }
                return null;
        }
    }
    public float GetMapProcessData( int index)
    {
        if (mapProcessDatas == null||mapProcessDatas.Length==0)
        {
            mapProcessDatas = JsonConvert.DeserializeObject <float[]>(Resources.Load<TextAsset>("Data/Map_Process_Data").text);
        }
        return mapProcessDatas[index];
    }
    public float[] GetMapProcessDatas()
    {
        if (mapProcessDatas == null || mapProcessDatas.Length == 0)
        {
            mapProcessDatas = JsonConvert.DeserializeObject<float[]>(Resources.Load<TextAsset>("Data/Map_Process_Data").text);
        }
        return mapProcessDatas;
    }
    public SoldierInGameData GetSoldierIngameData(int index)
    {
        if (gameData.SoldierInGameDatas.Count == 0)
        {
            gameData.SoldierInGameDatas = JsonConvert.DeserializeObject<List<SoldierInGameData>>(Resources.Load<TextAsset>("Data/First_Soldier_Ingame_Data").text);
        }
        return gameData.SoldierInGameDatas[index];

    }
    public SoldierUpdateData GetSoldierUpdateData(int index)
    {
        Debug.Log(index);
        if (soldierUpdateDatas == null|| soldierUpdateDatas.soldierData.Length==0)
        {
            soldierUpdateDatas = JsonConvert.DeserializeObject<SoldierUpdateDatas>(Resources.Load<TextAsset>("Data/Soldier_Update_Data").text);
        }
        return soldierUpdateDatas.soldierData[index];

    }
    public UnlockWeaponData GetUnlockWeaponData(int index)
    {
        if (unlockWeaponDatas == null || unlockWeaponDatas.unlockWeaponDatas.Length == 0)
        {
            unlockWeaponDatas = JsonConvert.DeserializeObject<UnlockWeaponDatas>(Resources.Load<TextAsset>("Data/Unlock_Weapon_Data").text);
        }
        if (index>unlockWeaponDatas.unlockWeaponDatas.Length)
        {
            return null;
        }
        return unlockWeaponDatas.unlockWeaponDatas[index];

    }
    public WeaponInGameData GetCurrentWeaponInGameData(WeaponType type)
    {
        return GetWeaponIngameData(type,gameData.weaponEquipedID[(int)type]);
    }
    public SoldierInGameData GetCurrentSoldierIngameData()
    {
        return GetSoldierIngameData(gameData.soldierEquipedID);
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
    public List<WeaponInGameData> GetAllWeaponInGameDataCanUpdate()
    {
         List<WeaponInGameData> allWeaponIsOwnedInGameDatas = new List<WeaponInGameData>();
        foreach (var smg in GetWeaponIngameDatas(WeaponType.SMG))
        {
            if (smg.isOwned && smg.level < 20)
            {
                allWeaponIsOwnedInGameDatas.Add(smg);
            }
        }
        foreach (var s in GetWeaponIngameDatas(WeaponType.ShotGun))
        {
            if (s.isOwned && s.level < 20)
            {
                allWeaponIsOwnedInGameDatas.Add(s);
            }
        }
        foreach (var s in GetWeaponIngameDatas(WeaponType.Sniper))
        {
            if (s.isOwned && s.level < 20)
            {
                allWeaponIsOwnedInGameDatas.Add(s);
            }
        }
        foreach (var mg in GetWeaponIngameDatas(WeaponType.Machinegun))
        {
            if (mg.isOwned && mg.level < 20)
            {
                allWeaponIsOwnedInGameDatas.Add(mg);
            }
        }
        return allWeaponIsOwnedInGameDatas;
    }
    public void AddListIAP(string iD)
    {
        if (gameData.ListIAP == null)
            gameData.ListIAP = new List<KeyValue>();
        if (gameData.ListIAP.Count == 0)
        for (int i = 0; i < gameData.ListIAP.Count; i++)
        {
            if (gameData.ListIAP[i].Key.Equals(iD))
            {
                int no = gameData.ListIAP[i].GetValueToInt();
                no++;
                gameData.ListIAP[i].Value = no.ToString();
                return;
            }
        }
        var kv = new KeyValue(iD, "1");
        gameData.ListIAP.Add(kv);
    }
    public void RemoveAllListIAP()
    {
        if (gameData.ListIAP == null)
            gameData.ListIAP = new List<KeyValue>();
        gameData.ListIAP.Clear();
    }
    public bool IsBuyIAPPack(string packID)
    {
        for (int i = 0; i < gameData.ListIAP.Count; i++)
            if (gameData.ListIAP[i].Key.Equals(packID))
                return true;
        return false;
    }
    public void RemoveIAPPackBought(string packID)
    {
        for (int i = 0; i < gameData.ListIAP.Count; i++)
            if (gameData.ListIAP[i].Key.Equals(packID))
                gameData.ListIAP.RemoveAt(i);
    }
    public double GetTimeStamp(string key,double defaultTime=-1)
    {
        if (gameData.ListTimeStamp == null)
            gameData.ListTimeStamp = new List<KeyValue>();
        for (int i = 0; i < gameData.ListTimeStamp.Count; i++)
        {
            if (gameData.ListTimeStamp[i].Key == key)
                return gameData.ListTimeStamp[i].GetValueToDouble();
        }
        var kv = new KeyValue(key, defaultTime == -1 ? GlobalTimer.Instance.GetUnixTimeStampNow().ToString() : defaultTime.ToString());
        gameData.ListTimeStamp.Add(kv);
        SaveData();
        return defaultTime == -1 ? GlobalTimer.Instance.GetUnixTimeStampNow() : defaultTime;
    }
    public void SetTimeStamp(string key, double timeStamp)
    {
        if (gameData.ListTimeStamp == null)
            gameData.ListTimeStamp = new List<KeyValue>();
        for (int i = 0; i < gameData.ListTimeStamp.Count; i++)
        {
            if (gameData.ListTimeStamp[i].Key == key)
                gameData.ListTimeStamp[i].Value = timeStamp.ToString();
        }
        var kv = new KeyValue(key, timeStamp.ToString());
        gameData.ListTimeStamp.Add(kv);
        SaveData();

    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveData();
        }
    }
    public void GetReward(KeyValue item)
    {
        switch (item.Key)
        {
            case "Cash":
                AddCash(item.GetValueToInt());
                break;
            case "Gold":
                AddGold(item.GetValueToInt());
                break;
            case "Metal":
                AddMetal(item.GetValueToInt());
                break;
            case "Grenade":
                AddGrenade(item.GetValueToInt());
                break;
            case "MedKit":
                AddMedKit(item.GetValueToInt());
                break;
            case "RemoveAds":
                PlayerPrefs.SetInt("RemoveAds", 1);
                ManagerAds.ins.HideBanner();
                MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnRemoveAds));
                break;
            default:
                var data = item.Key.Split("_");
                switch (data[0])
                {
                    case "Card":
                       var wigd= GetWeaponIngameData((WeaponType)int.Parse(data[1]), int.Parse(data[2]));
                        if (wigd != null)
                        {
                            wigd.AddCard(item.GetValueToInt());
                        }
                        break;
                }
               
                break;

        }
    }
    public WeaponInGameData GetRandomAirdropWeapons()
    {
        if (airdropWeaponDatas == null || airdropWeaponDatas.Count == 0)
        {
            airdropWeaponDatas = JsonConvert.DeserializeObject<List<KeyValue>>(Resources.Load<TextAsset>("Data/Airdrop_Weapon_Data").text);
        }
        List<WeaponInGameData> weaponInGameDatas = new List<WeaponInGameData>();
        for (int i = 0; i < airdropWeaponDatas.Count; i++)
        {
            var data = airdropWeaponDatas[i].Key.Split("_");
            var wpid = GetWeaponIngameData((WeaponType)int.Parse(data[0]), int.Parse(data[1]));
            if (!wpid.isOwned)
            {
                weaponInGameDatas.Add(wpid);
            }
        }
        if (weaponInGameDatas.Count > 0)
        {
            return weaponInGameDatas[UnityEngine.Random.Range(0, weaponInGameDatas.Count)];
        }
        return null;
    }
    public BuildData GetBuildData(int index)
    {
        if (buildDatas == null || buildDatas.Length == 0)
        {
            buildDatas = JsonConvert.DeserializeObject<BuildData[]>(Resources.Load<TextAsset>("Data/Build_Data").text);
        }
        return buildDatas[index];
    }
    public BuildData GetCurrentBuildData()
    {
        if (buildDatas == null || buildDatas.Length == 0)
        {
            buildDatas = JsonConvert.DeserializeObject<BuildData[]>(Resources.Load<TextAsset>("Data/Build_Data").text);
        }
        return buildDatas[Build];
    }
    public BuildProcessInGameData GetBuildProcessInGameData()
    {
        if ( gameData.buildProcessInGameData.cost ==0)
        {
            gameData.buildProcessInGameData.indexInData = 0;
            gameData.buildProcessInGameData.process = 0;
            gameData.buildProcessInGameData.cost = 1500;
        }
        return gameData.buildProcessInGameData;
    }
    public void ResetNewDay()
    {
        PlayerPrefs.SetInt(GameConstain.AUTO_SHOW_ENDLESS_OFFER_POPUP, 0);
        PlayerPrefs.SetInt(GameConstain.CLAIM_DAILY_REWARD, 0);
        PlayerPrefs.SetInt("Free_Crate", 0);
        if (Level >= 8)
        {
            BattlePassController.Instance.ResetBattlePassMissions();

        }

    }
   
}
