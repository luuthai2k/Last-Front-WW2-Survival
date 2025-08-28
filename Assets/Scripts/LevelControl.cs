using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class LevelControl : MonoBehaviour
{
    public int mapID;
    public WaveData[] waveDatas;
    public Transform playerSpawnPoint;
    public int currentWave = 0, currentEnemyWave;
    public int currentPlayerPath,currentTargetCover;
    public List<EnemyBase> enemyControlList = new List<EnemyBase>();
    private bool isInit;

    public async void InitWave()
    {
        currentWave = 0;
        currentPlayerPath = 0;
        PlayerController.Instance.Init(playerSpawnPoint,GameManager.Instance.levelData.mainWeaponType, GameManager.Instance.levelData.safeTime);
        await Task.Yield();
        isInit = true;
    }
    public virtual void StartWave()
    {

        StartCoroutine(AwaitInit());
        IEnumerator AwaitInit()
        {
            yield return new WaitUntil(() => isInit);
            PlayerController.Instance.StartWave(GetCurrentTargetCover());
            GenEnemies();
        }
      
    }
    public void StartWaveWithTryWeapon(string weaponID)
    {
        StartCoroutine(AwaitInit());
        IEnumerator AwaitInit()
        {
            yield return new WaitUntil(() => isInit);
            PlayerController.Instance.TryWeapon(weaponID);
            PlayerController.Instance.StartWave(GetCurrentTargetCover());
            GenEnemies();
        }
    }
    public virtual void GenEnemies()
    {

        StartCoroutine(DelayGenEnemies());
    }
    IEnumerator DelayGenEnemies()
    {
        EnemyWave wave = waveDatas[currentWave].enemyWaves[currentEnemyWave];
        yield return new WaitForSeconds(wave.startDelay);
        int multi = 1;
        for (int i = 0; i < wave.enemyModelList.Count; i++)
        {
            GameObject enemyCharacter = ResourceHelper.Instance.GetEnemyModel(wave.enemyModelList[i].enemyType);
            enemyCharacter.transform.position = wave.enemyModelList[i].spawnPoint.position;
            enemyCharacter.transform.rotation = wave.enemyModelList[i].spawnPoint.rotation;
            EnemyBase enemy = enemyCharacter.GetComponent<EnemyBase>();
            var em = wave.enemyModelList[i];
            enemy.Init(em.id, em.moveHeight, em.canChangePatrolPoint, em.maxHP+(590 * multi) ,em.weaponType, em.damage, em.fireRate);
            enemy.StartWave(wave.enemyModelList[i].GetPatrolPoint());
            enemyControlList.Add(enemy);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public virtual void ActiveNextWave()
    {  
        currentWave++;
        currentTargetCover = 0;
        currentEnemyWave = 0;
        if (currentWave >= waveDatas.Length)
        {
            PlayerController.Instance.OnWin();
            GamePlayUIManager.Instance.OnWin();
        }
        else
        {
            StartWave();
            GamePlayUIManager.Instance.gamePlayMenu.ShowProcessingInWave(currentWave);

        }
        FirebaseServiceController.Instance.LogEvent($"COMPLETE_{DataController.Instance.Level}_{currentWave}");

    }
    public virtual void ActiveNextEnemiesWave()
    {
        currentEnemyWave++;
        if (currentEnemyWave >= waveDatas[currentWave].enemyWaves.Length)
        {
            ActiveNextWave();
        }
        else
        {
            StartWave();
        }


    }
    public void ChangePlayerCoverPoint()
    {
        if (CanChangePlayerCoverPoint())
        {
            currentTargetCover++;
            PlayerController.Instance.StartWave(GetCurrentTargetCover());
        }
    }
    public bool CanChangePlayerCoverPoint()
    {
        if (currentTargetCover < waveDatas[currentWave].targetCovers.Length - 1)
        {
            var tei = waveDatas[currentWave].targetCovers[currentTargetCover + 1].targetEnemyIndex;
            for (int i = 0; i < tei.Length; i++)
            {
                for (int j = 0; j < enemyControlList.Count; j++)
                {
                    if (enemyControlList[j].id == tei[i]) return false;
                }
            }
            return true;
        }
        return false;

    }
    public int GetEnemiesTotalInSegment()
    {
        int total = 0;

        for (int j = 0; j < waveDatas[currentWave].enemyWaves.Length; j++)
                total += waveDatas[currentWave].enemyWaves[j].enemyModelList.Count;          

        return total;
    }
    public void RemoveEnemy(EnemyBase enemy)
    {
        enemyControlList.Remove(enemy);
    }
    public int GetEnemyCount()
    {
        return enemyControlList.Count;
    }

    public Transform FindClosestEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        foreach (EnemyBase enemy in enemyControlList)
        {
            if (enemy == null) continue;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);

            if (screenPos.z > 0 && screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height)
            {
                float distanceToCenter = Vector2.Distance(screenCenter, screenPos);

                if (distanceToCenter < closestDistance)
                {
                    closestDistance = distanceToCenter;
                    closestEnemy = enemy.transform;
                }
            }
        }

        return closestEnemy;
    }
    public int GetNumberEnermy()
    {
        int number = 0;
        for(int i = 0; i < waveDatas.Length; i++)
        {
            for(int j=0;j< waveDatas[i].enemyWaves.Length; j++)
            {
                number += waveDatas[i].enemyWaves[j].enemyModelList.Count;
            }
        }
        return number;
    }
    public bool IsLastWave()
    {
        return currentWave == waveDatas.Length - 1 && currentEnemyWave == waveDatas[currentWave].enemyWaves.Length-1;
    }
    public bool IsLastEnermy()
    {
        if (IsLastWave() && enemyControlList.Count == 1) return true;
        return false;
       
    }
    public TargetCover GetCurrentTargetCover()
    {
        return waveDatas[currentWave].targetCovers[currentTargetCover];
    }
    [Serializable]
    public class LevelSegment
    {
        public Transform levelSegmentObj;

        public List<EnemyWave> enemyWaves;

    }

    [Serializable]
    public class PlayerPatrolPath
    {
        public Transform[] path;

    }
}
