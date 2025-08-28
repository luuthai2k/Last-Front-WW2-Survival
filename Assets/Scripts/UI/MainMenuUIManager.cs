using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MainMenuUIManager : Singleton<MainMenuUIManager>, IMessageHandle
{
    public MenuPanelManager menuPanelManager;
    public ArmoryPanelController armoryPanelController;
    public SoldierPanelController soldierPanelController;
    public StorePanelController storePanelController;
    public BuildPanelController buildPanelController;
    public BattlePassPanelController battlePassPanelController;
    public ChallengingMissionPanelController challengingMissionPanelController;
    public GameObject goRatePopUp, goDailyRewardPopUp;
    public TabUIItem[] tabUIItem;
    private TabUIItem currentTabItem;
    private GameObject currentPanel;
    private bool isFirstTime;
    private Queue<Action> queues = new Queue<Action>();
    private bool[] canUpdateWeapons = {false,false,false,false};
    private bool canUpdateSoidier = false;
    public GameObject goArmoryNotification, goSoldierNotification, goShopNotification;


    private void Start()
    {
        currentTabItem = tabUIItem[(int)TabUIType.Mission];
        currentPanel = menuPanelManager.gameObject;
        AudioController.Instance.PlayMusic(GameConstain.BGM_MAINMENU);
        CheckCanUpdateSMG();
        CheckCanUpdateShotgun();
        CheckCanUpdateSniper();
        CheckCanUpdateMachineGun();
        CheckCanUpdateSoldier();
        CheckNotification();
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnFirstTime, this);
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnSceneLoaded, this);
        MessageManager.Instance.AddSubcriber(TeeMessageType.OnCashChange, this);
    }
    public void Handle(Message message)
    {
        switch (message.type)
        {
            case TeeMessageType.OnFirstTime:
                isFirstTime = true;
                    break;
            case TeeMessageType.OnSceneLoaded:
                CheckOpenPanel();
                break;
            case TeeMessageType.OnCashChange:
                CheckCanUpdateSMG();
                CheckCanUpdateShotgun();
                CheckCanUpdateSniper();
                CheckCanUpdateMachineGun();
                CheckCanUpdateSoldier();
                CheckNotification();
                break;
        }
    }
    private void ProcessQueue()
    {
        if (queues.Count <= 0)
        {
            return;
        }
        Action action = queues.Dequeue();
        action?.Invoke();
    }
    public async void CheckOpenPanel()
    {
        if (DataController.Instance.Level >= 7 && DataController.Instance.IsNewLevel && DataController.Instance.Build <= GameConstain.MAXBUILD)
        {
            queues.Enqueue(() =>
            {
                MainMenuCameraController.Instance.SetActiveCamera(false);
                buildPanelController.gameObject.SetActive(true);
                buildPanelController.SetActionAfterClose(() =>
                {
                    gameObject.SetActive(true);
                    MainMenuCameraController.Instance.SetActiveCamera(true);
                    ProcessQueue();
                });
                gameObject.SetActive(false);
            });
        }
        if (DataController.Instance.Level >= 2 && PlayerPrefs.GetInt(GameConstain.UNLOCK_SHOTGUN_TUT, 0) == 0)
        {
            queues.Enqueue(() =>
            {
                PlayerPrefs.SetInt(GameConstain.UNLOCK_SHOTGUN_TUT, 1);
                GameObject go = Instantiate(Resources.Load<GameObject>("Tutorial/UnlockShotGunTutorialCanvas"));
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    return;
                });

            });

        }
        if (DataController.Instance.Level >= 3 && PlayerPrefs.GetInt(GameConstain.UPDATE_WEAPON_TUT, 0) == 0)
        {
            queues.Enqueue(() =>
            {
                PlayerPrefs.SetInt(GameConstain.UPDATE_WEAPON_TUT, 1);
                GameObject go = Instantiate(Resources.Load<GameObject>("Tutorial/UpdateWeaponTutorialCanvas"));
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    return;
                });

            });

        }
        if (DataController.Instance.Level >= 6 && PlayerPrefs.GetInt(GameConstain.STORE_TUT, 0) == 0)
        {
            queues.Enqueue(() =>
            {
                //playerModel = Instantiate(Resources.Load<GameObject>($"Soldier/{data.ID}"), transform);
                //UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("Assets/Tutorial/UpdateWeaponTutorialCanvas.prefab");
                //await obj.Task;
                PlayerPrefs.SetInt(GameConstain.STORE_TUT, 1);
                GameObject go = Instantiate(Resources.Load<GameObject>("Tutorial/StoreTutorialCanvas"));
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    return;
                });

            });

        }
        if (DataController.Instance.Level >= 8 && PlayerPrefs.GetInt(GameConstain.BATTLEPASS_TUT, 0) == 0)
        {
            queues.Enqueue(() =>
            {
                BattlePassController.Instance.ResetData();
                PlayerPrefs.SetInt(GameConstain.BATTLEPASS_TUT, 1);
                GameObject go = Instantiate(Resources.Load<GameObject>("Tutorial/BattlePassTutorialCanvas"));
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    return;
                });
                //OnShowBattlePassPanel();
                //battlePassPanelController.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                //{
                //    ProcessQueue();
                //});

            });

        }
        if (DataController.Instance.Level >= 11 && PlayerPrefs.GetInt(GameConstain.UPDATE_SOLDIER_TUT, 0) == 0)
        {
            queues.Enqueue(() =>
            {
                PlayerPrefs.SetInt(GameConstain.UPDATE_SOLDIER_TUT, 1);
                GameObject go = Instantiate(Resources.Load<GameObject>("Tutorial/UpdateSoldierTutorialCanvas"));
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    return;
                });

            });

        }
        if (DataController.Instance.Level > 1 && PlayerPrefs.GetInt(GameConstain.CLAIM_DAILY_REWARD, 0) == 0)
        {

            queues.Enqueue(() =>
            {
                GameObject go = Instantiate(goDailyRewardPopUp, transform);
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    ProcessQueue();
                });
            });
        }
        if (CanShowEndlessOfferPopUp())
        {
            PlayerPrefs.SetInt(GameConstain.AUTO_SHOW_ENDLESS_OFFER_POPUP, 1);
            queues.Enqueue(async () =>
            {
                GameObject go = await UIManager.Instance.GetOfferPanel("weup.ww2.duty.frontline.zone.endlessoffer");
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    ProcessQueue();
                });
            });
        }
       
        if (isFirstTime)
        {
            ProcessQueue();
            return;
        }
        await System.Threading.Tasks.Task.Delay(150);
        if (DataController.Instance.IsNewLevel)
        {
            var offers = DataController.Instance.GetCurrentLevelData().Offers;
            foreach (var offer in offers)
            {
                if (offer.Value == "0"&&!DataController.Instance.IsBuyIAPPack(offer.Key))
                {

                    queues.Enqueue(async () =>
                    {
                        GameObject go = await UIManager.Instance.GetOfferPanel(offer.Key);
                        go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                        {
                            ProcessQueue();
                        });
                    });
                }
            }
        }
        if (DataController.Instance.Level > 4 && DataController.Instance.completedLevelsThisSession == 2 && PlayerPrefs.GetInt("Rating", 1) != 5)
        {
            queues.Enqueue(() =>
            {
                PlayerPrefs.SetInt(GameConstain.AUTO_SHOW_RATE_POPUP, 1);
                goRatePopUp.SetActive(true);
                goRatePopUp.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    ProcessQueue();
                });
            });
        }
        if (DataController.Instance.Level == 4 && PlayerPrefs.GetInt(GameConstain.AUTO_SHOW_RATE_POPUP, 0) == 0)
        {
            queues.Enqueue( () =>
            {
                PlayerPrefs.SetInt(GameConstain.AUTO_SHOW_RATE_POPUP, 1);
                goRatePopUp.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    ProcessQueue();
                });
                goRatePopUp.SetActive(true);
            });
          
        }
      
        ProcessQueue();
    }
  public bool CanShowEndlessOfferPopUp()
    {
        if (DataController.Instance.Level < 5) return false;
        if (PlayerPrefs.GetInt(GameConstain.ENDLESS_OFFER_PROCESS, 0) >= 20) return false;
        if (PlayerPrefs.GetInt(GameConstain.AUTO_SHOW_ENDLESS_OFFER_POPUP, 0) == 1) return false;
        return true;
    }
    public void OnPlayBtnClick()
    {
        var levelData = DataController.Instance.GetCurrentLevelData();
        WeaponInGameData weaponInGameData = DataController.Instance.GetCurrentWeaponInGameData(levelData.mainWeaponType);
        if (weaponInGameData.specification.damage>= levelData.damageRequire)
        {
            Play();
        }
        else
        {
            UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Weapon_Icon.spriteatlas").Completed += (UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.U2D.SpriteAtlas> obj) =>
            {

                Debug.LogError(weaponInGameData.ID);
                var icon = obj.Result.GetSprite("Icon_" + weaponInGameData.ID);
                challengingMissionPanelController.gameObject.SetActive(true);
                challengingMissionPanelController.SetUp(icon, levelData.damageRequire);
            };
        }
      
    }

   public void Play()
    {
        AudioController.Instance.StopMusic();
        SceneController.Instance.LoadScene(GameConstain.GamePlay, "Mission_loading", false);
    }
    public void OnSelectArmoryTab()
    {
        if (currentTabItem != null)
        {
            currentTabItem.DeSelect();
        }
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(false);
        }

        currentTabItem = tabUIItem[(int)TabUIType.Armory];
        currentTabItem.OnClickBtn();
        MainMenuCameraController.Instance.SetActiveCamera(false);
        currentPanel = armoryPanelController.gameObject;
        currentPanel.gameObject.SetActive(true);
    }
    public void OnSelectSoldierTab()
    {
        if (DataController.Instance.Level < 11)
        {
            UIManager.Instance.SendNoti("missions_left", 11 - DataController.Instance.Level);
            return;
        }
        if (currentTabItem != null)
        {
            currentTabItem.DeSelect();
        }
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(false);
        }
        currentTabItem = tabUIItem[(int)TabUIType.Soldier];
        currentTabItem.OnClickBtn();
        MainMenuCameraController.Instance.SetActiveCamera(false);
        currentPanel = soldierPanelController.gameObject;
        currentPanel.gameObject.SetActive(true);
    }
    public void OnSelectMissionTab()
    {
        if (currentTabItem != null)
        {
            currentTabItem.DeSelect();
        }
        MainMenuCameraController.Instance.SetActiveCamera(true);
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(false);
        }
        currentTabItem = tabUIItem[(int)TabUIType.Mission];
        currentTabItem.OnClickBtn();
        currentPanel = menuPanelManager.gameObject;
        currentPanel.gameObject.SetActive(true);

    }
    public void OnSelectStoreTab()
    {
        if (currentTabItem != null)
        {
            currentTabItem.DeSelect();
        }
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(false);
        }
        currentTabItem = tabUIItem[(int)TabUIType.Store];
        currentTabItem.OnClickBtn();
        MainMenuCameraController.Instance.SetActiveCamera(true);
        currentPanel = storePanelController.gameObject;
        currentPanel.gameObject.SetActive(true);
      
    }
    public void OnShowBuildPanel()
    {
        MainMenuCameraController.Instance.SetActiveCamera(false);
        buildPanelController.gameObject.SetActive(true);
        buildPanelController.SetActionAfterClose(() =>
        {
            gameObject.SetActive(true);
            MainMenuCameraController.Instance.SetActiveCamera(true);
          
        });
        gameObject.SetActive(false);
    }
    public void OnShowBattlePassPanel()
    {
        battlePassPanelController.gameObject.SetActive(true);
    }
    public void OnSelectStoreTab(string forcus)
    {
        OnSelectStoreTab();
        storePanelController.Forcus(forcus, false);
    }
    public void OnDestroy()
    {
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnFirstTime, this);
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnSceneLoaded, this);
        MessageManager.Instance.RemoveSubcriber(TeeMessageType.OnCashChange, this);
    }
    public void CheckCanUpdateSMG()
    {
        canUpdateWeapons[0] = false;
        var SMGInGameDatas = DataController.Instance.gameData.SMGInGameDatas;
        for(int i = 0; i < SMGInGameDatas.Count; i++)
        {
           
            if (SMGInGameDatas[i].isOwned)
            {
                var updateData = DataController.Instance.GetWeaponUpdateData(WeaponType.SMG, i);
                if (SMGInGameDatas[i].level < updateData.weaponLevelDatas.Length)
                {
                    int pirce = updateData.weaponLevelDatas[SMGInGameDatas[i].level].updatePirce;
                    int cardsRequire = updateData.weaponLevelDatas[SMGInGameDatas[i].level].cardsRequired;
                    int currentCards = SMGInGameDatas[i].cards;
                    //Debug.LogError(SMGInGameDatas[i].ID + ":" + currentCards + "and" + cardsRequire);
                    //Debug.LogError(pirce);
                    if (currentCards >= cardsRequire && DataController.Instance.Cash >= pirce)
                    {
                        canUpdateWeapons[0] = true;
                        return;
                    }
                }
               
            }
        }
      
    }
    public void CheckCanUpdateSniper()
    {
        canUpdateWeapons[2] = false;
        var SniperInGameDatas = DataController.Instance.gameData.SniperInGameDatas;
        for (int i = 0; i < SniperInGameDatas.Count; i++)
        {

            if (SniperInGameDatas[i].isOwned)
            {
                var updateData = DataController.Instance.GetWeaponUpdateData(WeaponType.Sniper, i);
                if (SniperInGameDatas[i].level < updateData.weaponLevelDatas.Length)
                {
                    int pirce = updateData.weaponLevelDatas[SniperInGameDatas[i].level].updatePirce;
                    int cardsRequire = updateData.weaponLevelDatas[SniperInGameDatas[i].level].cardsRequired;
                    int currentCards = SniperInGameDatas[i].cards;
                    //Debug.LogError(SniperInGameDatas[i].ID + ":" + currentCards + "and" + cardsRequire);
                    //Debug.LogError(pirce);
                    if (currentCards >= cardsRequire && DataController.Instance.Cash >= pirce)
                    {
                        canUpdateWeapons[2] = true;
                        return;
                    }
                }
               
            }
        }
      
    }
    public void CheckCanUpdateShotgun()
    {
        canUpdateWeapons[1] = false;
        var ShotgunInGameDatas = DataController.Instance.gameData.ShotGunInGameDatas;
        for (int i = 0; i < ShotgunInGameDatas.Count; i++)
        {

            if (ShotgunInGameDatas[i].isOwned)
            {
                var updateData = DataController.Instance.GetWeaponUpdateData(WeaponType.ShotGun, i);
                if (ShotgunInGameDatas[i].level < updateData.weaponLevelDatas.Length)
                {
                    int pirce = updateData.weaponLevelDatas[ShotgunInGameDatas[i].level].updatePirce;
                    int cardsRequire = updateData.weaponLevelDatas[ShotgunInGameDatas[i].level].cardsRequired;
                    int currentCards = ShotgunInGameDatas[i].cards;
                    //Debug.LogError(SniperInGameDatas[i].ID + ":" + currentCards + "and" + cardsRequire);
                    //Debug.LogError(pirce);
                    if (currentCards >= cardsRequire && DataController.Instance.Cash >= pirce)
                    {
                        canUpdateWeapons[1] = true;
                        return;
                    }
                }

            }
        }

    }
    public void CheckCanUpdateMachineGun()
    {
        canUpdateWeapons[3] = false;
        var MachineGunInGameDatas = DataController.Instance.gameData.MachineGunInGameDatas;
        for (int i = 0; i < MachineGunInGameDatas.Count; i++)
        {

            if (MachineGunInGameDatas[i].isOwned)
            {
                var updateData = DataController.Instance.GetWeaponUpdateData(WeaponType.Machinegun, i);
                if(MachineGunInGameDatas[i].level< updateData.weaponLevelDatas.Length)
                {
                    int pirce = updateData.weaponLevelDatas[MachineGunInGameDatas[i].level].updatePirce;
                    int cardsRequire = updateData.weaponLevelDatas[MachineGunInGameDatas[i].level].cardsRequired;
                    int currentCards = MachineGunInGameDatas[i].cards;
                    //Debug.LogError(MachineGunInGameDatas[i].ID + ":" + currentCards + "and" + cardsRequire);
                    //Debug.LogError(pirce);
                    if (currentCards >= cardsRequire && DataController.Instance.Cash >= pirce)
                    {
                        canUpdateWeapons[3] = true;
                        return;
                    }
                }
               
            }
        }
    }
    public void CheckCanUpdateSoldier()
    {
        canUpdateSoidier = false;
        if (DataController.Instance.Level < 11) return;
        var SoldierInGameDatas = DataController.Instance.gameData.SoldierInGameDatas;
        for (int i = 0; i < SoldierInGameDatas.Count; i++)
        {

            if (SoldierInGameDatas[i].isOwned)
            {
                var updateData = DataController.Instance.GetSoldierUpdateData( i);
                if (SoldierInGameDatas[i].level < updateData.soldierLevelDatas.Length)
                {
                    int pirce = updateData.soldierLevelDatas[SoldierInGameDatas[i].level].updatePirce;
                    if (DataController.Instance.Cash >= pirce)
                    {
                        Debug.LogError(DataController.Instance.Cash + "and" + pirce);
                       canUpdateSoidier = true;
                        return;
                    }


                }
                   
            }
        }
    }
    public void CheckNotification()
    {
        goArmoryNotification.SetActive(false);
        for (int i = 0; i < canUpdateWeapons.Length; i++)
        {
            if (canUpdateWeapons[i])
            {
                goArmoryNotification.SetActive(true);
            }
        }
        if (canUpdateSoidier)
        {
            goSoldierNotification.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Free_Crate", 0) == 0)
        {
            goShopNotification.gameObject.SetActive(true);
        }

    }
    public bool CanUpdateWeapon(WeaponInGameData weaponInGameData)
    {
       var updateData = DataController.Instance.GetWeaponUpdateData(weaponInGameData.weaponType, weaponInGameData.ID);
        if (weaponInGameData.isOwned && weaponInGameData.level < updateData.weaponLevelDatas.Length)
        {
            int pirce = updateData.weaponLevelDatas[weaponInGameData.level].updatePirce;
            int cardsRequire = updateData.weaponLevelDatas[weaponInGameData.level].cardsRequired;
            int currentCards = weaponInGameData.cards;
            if (currentCards >= cardsRequire && DataController.Instance.Cash >= pirce)
            {
                return true;
            }
        }
        return false;
    }
    public bool CanUpdateWeapon(int index)
    {
        return canUpdateWeapons[index];
    }
}
public enum TabUIType
{
    Mission,
    Armory,
    Soldier,
    Store
}

