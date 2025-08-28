using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlePassController : Singleton<BattlePassController>, IMessageHandle
{
    public BattlePassMissionsData[] missionDatas;
  
    void Start()
    {
      
        LoadMissionData();
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnCashChange, this);
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnKillEnermy, this);
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnShoot, this);
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnShootHelmets, this);
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnDestroyVehicles, this);
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnHeadShoot, this);
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnOpenCrate, this);
    }
    public void Handle(Message message)
    {
        if (DataController.Instance.Level < 8) return;
        if (message.type == TeeMessageType.OnCashChange)
        {
            int cash = (int)message.data[0];
            if (cash < 0)
            {
                SpendCash(cash);
            }
        }
        if (message.type == TeeMessageType.OnKillEnermy)
        {
            KillEnemy(message.data);

        }
        if (message.type == TeeMessageType.OnShoot)
        {

            ShootBullet();

        }
        if (message.type == TeeMessageType.OnShootHelmets)
        {
            Debug.LogError("OnShootHelmets");
            ShootHelmets();
        }
        if (message.type == TeeMessageType.OnDestroyVehicles)
        {
            DestroyVehicles();
        }
        if (message.type == TeeMessageType.OnHeadShoot)
        {
            HeadShoot(message.data);
        }
        if (message.type == TeeMessageType.OnOpenCrate)
        {
            OpenCrate();
        }
    }
    public void SpendCash(int coins)
    {
        var dm = DataController.Instance.GetGameData().battlePassDataSave.ListMission;
        foreach(var ms in dm)
        {
            if (missionDatas[ms.GetKeyToInt()].type==MissionType.SpendCash)
            {
                int value = ms.GetValueToInt();
                ms.Value = (value + coins).ToString();
                if(value< missionDatas[ms.GetKeyToInt()].amount&& ms.GetValueToInt()>= missionDatas[ms.GetKeyToInt()].amount)
                {
                    DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                }
            }
        }
    }
    public void KillEnemy(object[] data)
    {
        var dm = DataController.Instance.GetGameData().battlePassDataSave.ListMission;
        EnemyType enemyType = (EnemyType)data[0];
        WeaponType weaponType= (WeaponType)data[1];
        foreach (var ms in dm)
        {
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.KillOfficers)
            {
                if (enemyType == EnemyType.Officer)
                {
                    int value = ms.GetValueToInt();
                    ms.Value = (value + 1).ToString();
                    if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                    {
                        DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                    }
                }
               
            }
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.KillSoldiers)
            {
                if (enemyType == EnemyType.Soldier1|| enemyType == EnemyType.Soldier2|| enemyType == EnemyType.Soldier3|| enemyType == EnemyType.Soldier4)
                {
                    int value = ms.GetValueToInt();
                    ms.Value = (value + 1).ToString();
                    if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                    {
                        DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                    }
                }
               
            }
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.KillEnemies)
            {
                int value = ms.GetValueToInt();
                ms.Value = (value + 1).ToString();
                if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                {
                    DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                }
            }
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.KillEnemiesWithSMG)
            {
                if (weaponType == WeaponType.SMG)
                {
                    int value = ms.GetValueToInt();
                    ms.Value = (value + 1).ToString();
                    if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                    {
                        DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                    }
                }
            }
        }
     
    }
    public void ShootBullet()
    {
        var dm = DataController.Instance.GetGameData().battlePassDataSave.ListMission;
        foreach (var ms in dm)
        {
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.ShootBullets)
            {
                int value = ms.GetValueToInt();
                ms.Value = (value + 1).ToString();
                if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                {
                    DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                }
            }
        }
      
    }
    public void ShootHelmets()
    {
        var dm = DataController.Instance.GetGameData().battlePassDataSave.ListMission;
        foreach (var ms in dm)
        {
            Debug.LogError(missionDatas[ms.GetKeyToInt()].type);
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.ShootHelmets)
            {     
                Debug.LogError(ms.Key);
                int value = ms.GetValueToInt();
                ms.Value = (value + 1).ToString();
                if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                {
                    DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                }
            }
        }
    }
    public void DestroyVehicles()
    {
        var dm = DataController.Instance.GetGameData().battlePassDataSave.ListMission;
        foreach (var ms in dm)
        {
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.DestroyVehicles)
            {
                int value = ms.GetValueToInt();
                ms.Value = (value + 1).ToString();
                if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                {
                    DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                }
            }
        }
    }
    public void HeadShoot(object[] data)
    {
        var dm = DataController.Instance.GetGameData().battlePassDataSave.ListMission;
        EnemyType enemyType = (EnemyType)data[0];
        WeaponType weaponType = (WeaponType)data[1];
        foreach (var ms in dm)
        {
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.Headshoot)
            {

                int value = ms.GetValueToInt();
                ms.Value = (value + 1).ToString();
                if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                {
                    DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                }


            }
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.HeadshootWithSMG)
            {
                if (weaponType == WeaponType.SMG)
                {
                    int value = ms.GetValueToInt();
                    ms.Value = (value + 1).ToString();
                    if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                    {
                        DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                    }
                }

            }
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.HeadshootWithSniper)
            {
                if (weaponType == WeaponType.Sniper)
                {
                    int value = ms.GetValueToInt();
                    ms.Value = (value + 1).ToString();
                    if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                    {
                        DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                    }
                }

            }
        }

    }
    public void OpenCrate()
    {
        var dm = DataController.Instance.GetGameData().battlePassDataSave.ListMission;
        foreach (var ms in dm)
        {
            if (missionDatas[ms.GetKeyToInt()].type == MissionType.OpenCrates)
            {
                int value = ms.GetValueToInt();
                ms.Value = (value + 1).ToString();
                if (value < missionDatas[ms.GetKeyToInt()].amount && ms.GetValueToInt() >= missionDatas[ms.GetKeyToInt()].amount)
                {
                    DataController.Instance.GetGameData().battlePassDataSave.numberMissionComplete++;
                }
            }
        }
    }
    public BattlePassMissionsData GetBattlePassMissionsData(int ID)
    {
        if(missionDatas==null|| missionDatas.Length == 0)
        {
            LoadMissionData();
        }
        return missionDatas[ID];
    }
    public void LoadMissionData()
    {
        missionDatas = BattlePassHelper.GetMission();
    }
    public KeyValue GetBattlePassMissionsSave(int ID)
    {
        var listData = DataController.Instance.GetGameData().battlePassDataSave.ListMission;
        if (listData == null || listData.Count == 0)
        {
            ResetBattlePassMissions();
            listData = DataController.Instance.GetGameData().battlePassDataSave.ListMission;
        }
        for (int i = 0; i < listData.Count; i++)
        {
            if (listData[i].GetKeyToInt() == ID)
            {
                return listData[i];
            }
        }
        return null;
    }
    public bool IsUnLockVipPass()
    {
        return DataController.Instance.IsBuyIAPPack("weup.ww2.duty.frontline.zone.battlepasspremium");
    }
    public void ResetData()
    {
        DataController.Instance.GetGameData().battlePassDataSave = new BattlePassDataSave();
        DataController.Instance.RemoveIAPPackBought("weup.ww2.duty.frontline.zone.battlepasspremium");
        ResetBattlePassMissions();
    }
    public void ResetBattlePassMissions()
    {
        var gd = DataController.Instance.GetGameData();
        gd.battlePassDataSave.ListMission.Clear();
        var mission = BattlePassHelper.GetMission();
        List<BattlePassMissionsData> listMissionsDatas = new List<BattlePassMissionsData>();
        listMissionsDatas = mission.Take(10).OrderBy(m => m.id).ToList();
        for (int i = 0; i < listMissionsDatas.Count; i++)
        {
            gd.battlePassDataSave.ListMission.Add(new KeyValue(listMissionsDatas[i].id.ToString(), "0"));
        }
    }
    void OnDestroy()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnCashChange, this);
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnKillEnermy, this);
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnShoot, this);
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnShootHelmets, this);
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnDestroyVehicles, this);
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnHeadShoot, this);
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnOpenCrate, this);
    }
}
