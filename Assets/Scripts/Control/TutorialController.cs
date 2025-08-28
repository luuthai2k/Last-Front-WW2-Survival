using InviGiant.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : LevelControl
{
    public GameObject goEnemyDummy;
    public void Start()
    {
        DataController.Instance.Grenade = 1;
        DataController.Instance.MedKit = 1;
        GamePlayUIManager.Instance.gamePlayMenu.UpdateGrenadeText();
        GamePlayUIManager.Instance.gamePlayMenu.UpdateMedKitText();
       
       
    }
    public override void StartWave()
    {
        base.StartWave();
    }
    public override void GenEnemies()
    {
        StartCoroutine(DelayGenEnemies());

    }
    IEnumerator DelayGenEnemies()
    {
        EnemyWave wave = waveDatas[currentWave].enemyWaves[currentEnemyWave];
        yield return new WaitForSeconds(wave.startDelay);
        for (int i = 0; i < wave.enemyModelList.Count; i++)
        {
          
            GameObject enemyDummy = Instantiate(goEnemyDummy);
            enemyDummy.transform.position = wave.enemyModelList[i].spawnPoint.position;
            enemyDummy.transform.rotation = wave.enemyModelList[i].spawnPoint.rotation;
            EnemyBase enemy = enemyDummy.GetComponent<EnemyBase>();
            var em = wave.enemyModelList[i];
            enemy.Init(em.id, em.moveHeight, em.canChangePatrolPoint, em.maxHP,em.weaponType, em.damage, em.fireRate);
            enemy.StartWave(null);
            enemyControlList.Add(enemy);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public override void ActiveNextWave()
    {
        currentWave++;
        currentTargetCover = 0;
        currentEnemyWave = 0;
        if (currentWave >= waveDatas.Length)
        {
            StartCoroutine(DelayLoadScene());
           
        }
        else
        {
            StartWave();
            WeaponType weaponType = waveDatas[currentWave].targetCovers[currentEnemyWave].weaponType;
            if (weaponType != WeaponType.Machinegun&& weaponType != PlayerController.Instance.currentweaponType)
            {
                var weapon = DataController.Instance.GetCurrentWeaponInGameData(weaponType);
                PlayerController.Instance.playerShootController.ChangeMainWeapon(weapon.ID);
                if (weaponType == WeaponType.Sniper)
                {
                    CameraManager.Instance.ChangeCamera(CameraType.Sniper);
                }
                PlayerUIControl.Instance.SetUpWeaponUIEquipments(new List<WeaponInGameData> { weapon });
                PlayerController.Instance.currentweaponType = weaponType;
                CameraManager.Instance.currentVirtualCamnera.SetDamping(new Vector3(0.1f, 0.5f, 1.5f), 0.1f);
            }
            GamePlayUIManager.Instance.gamePlayMenu.ShowProcessingInWave(currentWave);

        }
        FirebaseServiceController.Instance.LogEvent($"COMPLETE_{DataController.Instance.Level}_{currentWave}");

    }
    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneController.Instance.LoadScene(GameConstain.GamePlay, "Mission_loading");
    }
    public override void ActiveNextEnemiesWave()
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
  
}
