using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DataDefine : MonoBehaviour
{
    public static AirdropRate[] GetAirdropRate()
    {
        return JsonConvert.DeserializeObject<AirdropRate[]>(Resources.Load<TextAsset>("Data/Airdrop_Rate").text);
    }
}

public interface ITimer
{
    void OnTick();

    void OnAddTime(int sec);

    bool IsNull();
}
[System.Serializable]
public class GameDataSave
{
    public int level;
    public int gold;
    public int cash;
    public int metal;
    public int medKid;
    public int grenade;
    public int key;
    public int build;
    public int soldierEquipedID;
    public int[] weaponEquipedID;
    public List<WeaponInGameData> SMGInGameDatas;
    public List<DifficultyData> unlockDifficultyData;
    public List<WeaponInGameData> ShotGunInGameDatas;
    public List<WeaponInGameData> SniperInGameDatas;
    public List<WeaponInGameData> MachineGunInGameDatas;
    public List<SoldierInGameData> SoldierInGameDatas;
    public List<KeyValue> ListIAP = new List<KeyValue>();
    public List<KeyValue> ListTimeStamp = new List<KeyValue>();
    public List<KeyValue> ListEvent = new List<KeyValue>();
    public List<string> ListCrate;
    public BuildProcessInGameData buildProcessInGameData=new BuildProcessInGameData();
    public BattlePassDataSave battlePassDataSave = new BattlePassDataSave();
   
    public double saveTime;
    public GameDataSave()
    {
        level = 0;
        grenade = 1;
        medKid = 1;
        soldierEquipedID = 0;
        weaponEquipedID = new int[4] { 0, 0, 0, 0 };
        SMGInGameDatas = new List<WeaponInGameData>();
        ShotGunInGameDatas = new List<WeaponInGameData>();
        SniperInGameDatas = new List<WeaponInGameData>();
        MachineGunInGameDatas = new List<WeaponInGameData>();
        SoldierInGameDatas = new List<SoldierInGameData>();
        ListIAP = new List<KeyValue>();
        ListTimeStamp = new List<KeyValue>();
        ListCrate = new List<string>();
    }
    public int GetWeaponSelectID(WeaponType type)
    {
        return weaponEquipedID[(int)type];
    }
    public void SetWeaponSelectID(WeaponType type,int index)
    {
        weaponEquipedID[(int)type]=index;
    }


}
[System.Serializable]
public class BattlePassDataSave
{
    public List<KeyValue> ListMission = new List<KeyValue>();
    public int xp;
    public int level,numberMissionComplete;
    public List<int> rewardFreeCanCollect = new List<int>();
    public List<int> rewardVipCanCollect = new List<int>();
    
}
[System.Serializable]
public class LevelData
{
    public string levelID;
    public int region;
    public WeaponType mainWeaponType;
    public int damageRequire;
    public float safeTime=3;
    public Pack[] difficultyBonus = new Pack[3];
    public KeyValue TryWeapon;
    //public KeyValue[] Items;
    //public KeyValue[] Weapons;
    public KeyValue[] Crates;
    //public KeyValue[] Soldiers;
    public KeyValue[] Offers;
}
[System.Serializable]
public class WaveData
{
   
    public TargetCover[] targetCovers;
    public EnemyWave[] enemyWaves;

}
[System.Serializable]
public class EnemyModelData
{
    public int id;
    public EnemyType enemyType;
    public WeaponType weaponType;
    public MoveType moveHeight;
    public Transform spawnPoint;
    public EnemyPoint[] patrolPoints;
    public int maxHP,damage=10;
    public float fireRate = 1;
    public bool canChangePatrolPoint;
    public EnemyPoint GetPatrolPoint()
    {
        return patrolPoints[UnityEngine.Random.Range(0, patrolPoints.Length)];
    }
}

[System.Serializable]
public class TargetCover
{
    public WeaponType weaponType;
    public MoveType moveType;
    public Transform[] paths;
    public PlayerPoint targetPoint;
    public int[] targetEnemyIndex;

}
[System.Serializable]
public class WeaponInGameData
{
    public string ID;
    public WeaponType weaponType;
    public bool isOwned;
    public int level;
    public int unlockLevel;
    public WeaponSpecifications specification;
    public int cards;
  
   
    public void AddCard(int amount)
    {
        cards =Math.Max( cards+amount,0);
    }
    public float GetShootDelay()
    {
        return 60f / specification.fireRate;
    }

}
[System.Serializable]
public class WeaponUpdateDatas
{
    public WeaponUpdateData[] SMGData;
    public WeaponUpdateData[] ShotGunData;
    public WeaponUpdateData[] SniperData;
    public WeaponUpdateData[] MachineGunData;
}
[System.Serializable]
public class WeaponUpdateData
{
    public string ID;
    public WeaponLevelData[] weaponLevelDatas;
}
[System.Serializable]
public class WeaponLevelData
{
    public string level;
    public WeaponSpecifications weaponSpecifications;
    public int cardsRequired, updatePirce;
   
}
[System.Serializable]
public class WeaponSpecifications
{
    public int damage, magazine, fireRate;
    public WeaponSpecifications(int damage, int magazine, int fireRate)
    {
        this.damage = damage;
        this.magazine = magazine;
        this.fireRate = fireRate;
    }
}
[System.Serializable]
public class SoldierInGameData
{
    public string ID;
    public bool isOwned;
    public int level;
    public int unlockLevel;
    public SoldierStat soldierStat;

}
[System.Serializable]
public class SoldierUpdateDatas
{
    public SoldierUpdateData[] soldierData;
}
[System.Serializable]
public class SoldierUpdateData
{
    public string ID;
    public SoldierLevelData[] soldierLevelDatas;
}
[System.Serializable]
public class SoldierLevelData
{
    public string level;
    public SoldierStat soldierStat;
    public int updatePirce;
}

[System.Serializable]
public class SoldierStat
{
    public int health, handStability, reloadSpeed;
    public SoldierStat(int health, int handStability, int reloadSpeed)
    {
        this.health = health;
        this.handStability = handStability;
        this.reloadSpeed = reloadSpeed;
    }
}
[System.Serializable]
public class ShopWeaponDatas
{
    public ShopWeaponData[] SMGData;
    public ShopWeaponData[] ShotGunData;
    public ShopWeaponData[] SniperData;
    public ShopWeaponData[] MachineGunData;
}
[System.Serializable]
public class ShopWeaponData
{
    public string ID,packID;
    public int pirce,numberAds;
    public BuyTag tag;
}
[System.Serializable]
public class UnlockWeaponDatas
{
    public UnlockWeaponData[] unlockWeaponDatas;
}
[System.Serializable]
public class UnlockWeaponData
{
    public string ID;
    public WeaponType weaponType;
    public int indexIngameData;
    public bool isFree;
}
[System.Serializable]
public class AirdropRate
{
    public KeyValue[] reward;
}
[System.Serializable]
public class BuildProcessInGameData
{
    public int indexInData;
    public int process, cost;
}
[System.Serializable]
public class BuildData
{
    public string id;
    public BuildProcessData[] buildProcessDatas;
}
[System.Serializable]
public class BuildProcessData
{
    public int cost;
    public KeyValue reward;
}
[System.Serializable]
public class DifficultyData
{
    public int[] difficultys;
    public DifficultyData()
    {
        difficultys = new int[3] { 0, -1, -1 };
    }
}
public enum WeaponType
{
    SMG=0,
    ShotGun=1,
    Sniper=2,
    Machinegun=3,
    Rocket=4,
}
public enum BuyTag
{
    Base,
    Offer,
    Premium
}
public enum WeaponEffect
{
    Own,
    Select,
    Equiped,
    Pirce,
    Offer,
    Premium,
    Lock,
   
}
public enum CoverType
{
    LowCover=0,
    HightCover=1,
    None=2,
}
public enum CoverDirection
{
    Left=0,
    Right=1,
}
public enum MoveType
{
    Walk=0,
    Sprint=1,
}
public enum CharactorType
{
    Comrade,
    Enemy
}
public enum EffectType
{
    SMGMuzzleFlash,
    ShotGunMuzzleFlash,
    SniperMuzzleFlash,
    MachinegunMuzzleFlash,
    RocketMuzzleFlash,
    MetalImpact,
    WoodImpact,
    StoneImpact,
    SoilImpact,
    PoisonBarretImpact,
    Blood,
    BloodExplosion,
    BigExplosion,
    GrenadeExplosion,
    RocketExplosion,
    PoisonCloud,
    PressurisedSteam

}
public enum MissionType
{
    KillEnemies,
    KillEnemiesWithSMG,
    KillEnemiesWithSniper,
    KillEnemiesWithMachineGun,
    KillOfficers,
    KillSoldiers,
    Headshoot,
    HeadshootWithSMG,
    HeadshootWithSniper,
    HeadshootWithMachineGun,
    ShootBullets,
    ShootHelmets,
    DestroyVehicles,
    SpendCash,
    SpendGold,
    OpenCrates,
}



