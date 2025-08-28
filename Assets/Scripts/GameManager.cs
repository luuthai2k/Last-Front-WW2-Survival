using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Cinemachine.DocumentationSortingAttribute;


public class GameManager : Singleton<GameManager>, IMessageHandle
{
    public LevelData levelData;
    public static bool isFirst;
    public CameraManager camScript;
    public int totalLevel;
    public LevelControl levelControl;
    public Transform camRoot;
    public int kill, headshot, health;
    private WeaponInGameData tryWeaponData;
    private int indexTryWeaponInData;
    public GameObject goMap;


    private void Start()
    {
        LoadLevel();
        DataController.Instance.IsNewLevel = false;
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnSceneLoaded, this);
    }
    public void Handle(Message message)
    {
        switch (message.type)
        {
            case TeeMessageType.OnSceneLoaded:
                StartGame();
                break;
        }
    }

    public void StartGame()
    {
        if (PlayerPrefs.GetInt($"SHOW_INTRO_{levelData.region}", 0) == 0)
        {
            PlayerPrefs.SetInt($"SHOW_INTRO_{levelData.region}", 1);

            GamePlayUIManager.Instance.SpawnIntro($"Intro_{levelControl.mapID}", () =>
            {
                if (!CheckShowTryWeapopPopUp())
                {
                    levelControl.StartWave();
                }
            });

        }
        else
        {
            if (!CheckShowTryWeapopPopUp())
            {
                levelControl.StartWave();
            }
        }
        AudioController.Instance.PlaySfx(GameConstain.MISSION_START);
    }
    public void LoadLevel()
    {
        int level = DataController.Instance.levelSelect;
        levelData = DataController.Instance.GetLevelData(level);
        string path = $"Level/Level_{level}";
        if (level == 0)
        {
            path = $"Level/Level_Tut_SMG";
        }
        else if (level == 3 && PlayerPrefs.GetInt(GameConstain.MACHINEGUN_CONTROL_TUT, 0) == 0)
        {
            path = $"Level/Level_Tut_MachineGun";
        }
        else if (level == 5 && PlayerPrefs.GetInt(GameConstain.SNIPER_CONTROL_TUT, 0) == 0)
        {
            path = $"Level/Level_Tut_Sniper";
        }
        GameObject goLevel = Instantiate(Resources.Load<GameObject>(path), Vector3.zero, Quaternion.identity);
        if (goLevel == null)
        {
            Debug.LogError($"Không load được Level {level} từ Resource!");
            return;
        }
        levelControl = goLevel.GetComponent<LevelControl>();
        GamePlayUIManager.Instance.gamePlayMenu.Init(levelControl.waveDatas.Length);
        LoadMap();
        FirebaseServiceController.Instance.LogEvent($"ENTER_LEVEL_{level}");
    }
    public bool CheckShowTryWeapopPopUp()
    {
        if (levelData.TryWeapon.Key != "")
        {
            var data = levelData.TryWeapon.Key.Split("_");
            var wpid = DataController.Instance.GetWeaponIngameData((WeaponType)int.Parse(data[0]), int.Parse(data[1]));
            if (wpid.isOwned)
            {
                return false;
            }
            tryWeaponData = wpid;
            indexTryWeaponInData = int.Parse(data[1]);
            GamePlayUIManager.Instance.ShowTryWeapon(wpid, levelData.TryWeapon.GetValueToInt());
            return true;
        }
        return false;


    }
    public void DontTryWeapon()
    {
        tryWeaponData = null;

    }
    public void LoadMap()
    {
        goMap= Instantiate(Resources.Load<GameObject>($"Map/Map_{levelControl.mapID}"), Vector3.zero, Quaternion.identity);
        levelControl.InitWave();
    }
    public void AddBonusCoin(int _coin)
    {
        int coin = PlayerPrefs.GetInt("Coin");
        coin += _coin;
        PlayerPrefs.SetInt("Coin", coin);
    }
    public void CamShake()
    {
        StartCoroutine(CamShakeIE(0.5f, 0.2f));
    }

    IEnumerator CamShakeIE(float duration, float magnitude)
    {

        Vector3 originalPos = camRoot.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            camRoot.position = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        camRoot.position = originalPos;
    }

    public int GetCurrentLevel()
    {
        int _level = PlayerPrefs.GetInt("CurrentLevel");

        if (_level == 0)
        {
            _level = 1;
        }
        else if (_level > totalLevel)
        {
            _level = 1;
            PlayerPrefs.SetInt("CurrentLevel", _level);

        }

        else
        {
            _level = PlayerPrefs.GetInt("CurrentLevel");

        }

        return _level;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    public bool IsTryWeapon(out WeaponInGameData weaponInGameData, out int index)
    {
        weaponInGameData = tryWeaponData;
        index = indexTryWeaponInData;
        if (tryWeaponData != null) return true;
        return false;
    }

    void OnDestroy()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnSceneLoaded, this);
    }

}
