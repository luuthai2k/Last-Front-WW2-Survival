using DG.Tweening;
using InviGiant.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using System.Threading.Tasks;

public class UIManager : Singleton<UIManager>
{
    public LootBoxPanelController lootBoxPanelController;
    public RewardPanelController rewardPanelController;
    public CollectWeaponPanel collectWeaponPanel;
    public AirdropPanelController airdropPanelController;
    private NotiControl notiControl;
    public GameObject  cashItemPropPrefab, goldItemPropPrefab, metalItemPropPrefab, notiPrefab, keyLootViewPrefab,grenadeLootViewPrefab;
    private Queue<Action> queues = new Queue<Action>();
    private Action onQueueCompleted;
    private void Start()
    {
        Application.targetFrameRate = 60;
       
    }
    public GameObject SpawnLootBox(string key)
    {
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnOpenCrate));
        return lootBoxPanelController.Spawm(key);
    }
    public GameObject SpawnLootBox(string key,bool isTutorial)
    {
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnOpenCrate));
        return lootBoxPanelController.Spawm(key, isTutorial);
    }
  
    public GameObject SpawmReward(KeyValue[] items)
    {
        Debug.LogWarning("SpawmReward");
        return rewardPanelController.Spawm(items);
    }
    public GameObject SpawmWeapon(WeaponInGameData data)
    {
        Debug.LogWarning("SpawmWeapon");

        return collectWeaponPanel.Spawm(data);
    }
    public GameObject SpawmWeapon(Sprite icon, string nameID)
    {
        return collectWeaponPanel.Spawm(icon,nameID);
    }
    public GameObject SpawnAirdropPanel()
    {
        return airdropPanelController.Spawm();
    }
    public async void GetPackReward(Pack packData, Action onCompleted = null)
    {
        onQueueCompleted = onCompleted;
        queues.Clear();
        if (packData.Weapons.Length > 0)
        {
            AsyncOperationHandle<SpriteAtlas> obj = Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Weapon_Icon.spriteatlas");
            await obj.Task;
            foreach (var wp in packData.Weapons)
            {
                var data = wp.Key.Split('_');
                var wpid = DataController.Instance.GetWeaponIngameData((WeaponType)int.Parse(data[0]), int.Parse(data[1]));
              
                queues.Enqueue(() =>
                {
                    //if (wpid.isOwned)
                    //{
                    //    KeyValue item=new KeyValue("Card")
                    //}
                    //else
                    //{
                        wpid.isOwned = true;
                        var icon = obj.Result.GetSprite("Icon_" + wpid.ID);
                        GameObject go = SpawmWeapon(icon, wpid.ID);
                        go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                        {
                            ProcessQueue();
                        });
                    //}
                   
                });
                FirebaseServiceController.Instance.LogEvent($"UNLOCK_{wpid.ID}_IAP");
            }


        }
        //if (packData.Soldier.Count > 0)
        //{
        //    foreach (var wp in packData.Soldier)
        //    {
        //        queues.Enqueue(() =>
        //        {
        //            SpawmWeapon(wp);
        //        });
        //    }
        //}
        if (packData.Crates.Length > 0)
        {
            foreach (var c in packData.Crates)
            {
                queues.Enqueue(() =>
                {
                    if (MainMenuUIManager.Instance != null)
                    {
                        MainMenuUIManager.Instance.gameObject.SetActive(false);
                    }
                    if (GamePlayUIManager.Instance != null)
                    {
                        GamePlayUIManager.Instance.gameObject.SetActive(false);
                    }
                    GameObject go = SpawnLootBox(c.Key);
                    go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                    {

                        if (MainMenuUIManager.Instance != null)
                        {
                            MainMenuUIManager.Instance.gameObject.SetActive(true);
                        }
                        if (GamePlayUIManager.Instance != null)
                        {
                            GamePlayUIManager.Instance.gameObject.SetActive(true);
                        }
                        ProcessQueue();
                    });
                });
            }
        }
        if (packData.Items.Length > 0)
        {

            foreach (var item in packData.Items)
            {
                DataController.Instance.GetReward(item);
            }
            queues.Enqueue(() =>
            {
                GameObject go = SpawmReward(packData.Items);
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    ProcessQueue();
                });
            });
        }
        ProcessQueue();
    }
    public async void GetPackReward(Pack packData,int multi, Action onCompleted = null)
    {
        onQueueCompleted = onCompleted;
        queues.Clear();
        if (packData.Weapons.Length > 0)
        {
            AsyncOperationHandle<SpriteAtlas> obj = Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Weapon_Icon.spriteatlas");
            await obj.Task;
            foreach (var wp in packData.Weapons)
            {
                var data = wp.Key.Split('_');
                var wpid = DataController.Instance.GetWeaponIngameData((WeaponType)int.Parse(data[0]), int.Parse(data[1]));

                queues.Enqueue(() =>
                {
                    //if (wpid.isOwned)
                    //{
                    //    KeyValue item=new KeyValue("Card")
                    //}
                    //else
                    //{
                    wpid.isOwned = true;
                    var icon = obj.Result.GetSprite("Icon_" + wpid.ID);
                    GameObject go = SpawmWeapon(icon, wpid.ID);
                    go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                    {
                        ProcessQueue();
                    });
                    //}

                });
                FirebaseServiceController.Instance.LogEvent($"UNLOCK_{wpid.ID}_IAP");
            }


        }
        //if (packData.Soldier.Count > 0)
        //{
        //    foreach (var wp in packData.Soldier)
        //    {
        //        queues.Enqueue(() =>
        //        {
        //            SpawmWeapon(wp);
        //        });
        //    }
        //}
        if (packData.Crates.Length > 0)
        {
            foreach (var c in packData.Crates)
            {
                for(int i = 0; i < multi; i++)
                {
                    queues.Enqueue(() =>
                    {
                        if (MainMenuUIManager.Instance != null)
                        {
                            MainMenuUIManager.Instance.gameObject.SetActive(false);
                        }
                        if (GamePlayUIManager.Instance != null)
                        {
                            GamePlayUIManager.Instance.gameObject.SetActive(false);
                        }
                        GameObject go = SpawnLootBox(c.Key);
                        go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                        {

                            if (MainMenuUIManager.Instance != null)
                            {
                                MainMenuUIManager.Instance.gameObject.SetActive(true);
                            }
                            if (GamePlayUIManager.Instance != null)
                            {
                                GamePlayUIManager.Instance.gameObject.SetActive(true);
                            }
                            ProcessQueue();
                        });
                    });
                }
               
            }
        }
        if (packData.Items.Length > 0)
        {
            foreach (var item in packData.Items)
            {
                item.Value = (item.GetValueToInt() * multi).ToString();
                DataController.Instance.GetReward(item);
            }
            queues.Enqueue(() =>
            {
                GameObject go = SpawmReward(packData.Items);
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    ProcessQueue();
                });
            });
        }
        ProcessQueue();
    }
    public void RestorePurchase(Pack packData)
    {
        if (packData.Weapons.Length > 0)
        {
            foreach (var wp in packData.Weapons)
            {
                var data = wp.Key.Split('_');
                var wpid = DataController.Instance.GetWeaponIngameData((WeaponType)int.Parse(data[0]), int.Parse(data[1]));
                wpid.isOwned = false;
            }
        }

        if (packData.Items.Length > 0)
        {

            foreach (var item in packData.Items)
            {
                if (item.Key == "RemoveAds")
                {

                    PlayerPrefs.SetInt("RemoveAds", 0);
                    ManagerAds.ins.ShowBanner();
                }
            }
        }
    }
    private void ProcessQueue()
    {
        if (queues.Count <= 0)
        {
            onQueueCompleted?.Invoke();
            onQueueCompleted = null;
            return;
        }
        Action action = queues.Dequeue();
        action?.Invoke();
    }
    public void SpawmCashsEffect(Vector3 spawnPos, Vector3 desPos,Vector2 sizeDelta,Vector2 targetSizeDelta, int amount)
    {
        amount = Math.Min(amount, 20);
        AudioController.Instance.PlaySfx(GameConstain.REWARD_POP);
        for (int i = 0; i < amount; i++)
        {
            GameObject coin = SmartPool.Instance.Spawn(cashItemPropPrefab, transform);
            coin.transform.position = spawnPos;
            coin.GetComponent<ItemProp>().MoveTween(desPos, spawnPos,sizeDelta,targetSizeDelta, 0.8f, i * 0.05f + 0.05f,
                (() =>
                {
                   
                    SmartPool.Instance.Despawn(coin);
                }));
        }
    }
    public void SpawmGoldsEffect(Vector3 spawnPos, Vector3 desPos, Vector2 sizeDelta, Vector2 targetSizeDelta, int amount)
    {
        AudioController.Instance.PlaySfx(GameConstain.REWARD_POP);
        for (int i = 0; i < amount; i++)
        {
            GameObject coin = SmartPool.Instance.Spawn(goldItemPropPrefab, spawnPos, Quaternion.identity);
            coin.GetComponent<ItemProp>().MoveTween(desPos, spawnPos, sizeDelta, targetSizeDelta, 0.8f, i * 0.05f + 0.05f,
                (() =>
                {
                    SmartPool.Instance.Despawn(coin);
                }));
        }
    }
    public void SpawmMetalsEffect(Vector3 spawnPos, Vector3 desPos, Vector2 sizeDelta, Vector2 targetSizeDelta, int amount)
    {
        amount = Math.Min(amount, 20);
        AudioController.Instance.PlaySfx(GameConstain.REWARD_POP);
        for (int i = 0; i < amount; i++)
        {
            GameObject metal = SmartPool.Instance.Spawn(metalItemPropPrefab, transform);
            metal.transform.position = spawnPos;
            metal.GetComponent<ItemProp>().MoveTween(desPos, spawnPos, sizeDelta, targetSizeDelta, 0.8f, i * 0.05f + 0.05f,
                (() =>
                {

                    SmartPool.Instance.Despawn(metal);
                }));
        }
    }
   
    public void SpawmKeyEffect()
    {
        Instantiate(keyLootViewPrefab, transform);
    }
    public GameObject SpawmGrenadeLootView(Transform parent)
    {
        return Instantiate(grenadeLootViewPrefab, parent);
    }
    public void SendNoti(string key)
    {
        if (notiControl == null)
        {
            notiControl = Instantiate(notiPrefab, transform).GetComponent<NotiControl>();
        }
        notiControl.SendNoti(key);
    }
    public void SendNoti(string key, object arg0)
    {
        if (notiControl == null)
        {
            notiControl = Instantiate(notiPrefab, transform).GetComponent<NotiControl>();
        }
        notiControl.SendNoti(key, arg0);
    }
    public async Task<GameObject> GetOfferPanel(string packId)
    {
        AsyncOperationHandle<GameObject> obj = Addressables.LoadAssetAsync<GameObject>($"Assets/Offer/{packId}.prefab");
        await obj.Task;
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject go = Instantiate(obj.Result, transform);
            return go;
        }
        else
        {
            Debug.LogError($"Failed to load prefab: Assets/Offer/{packId}.prefab. Error: {obj.OperationException?.Message}");
            return null;
        }
    }
    public void LogEvent(string key)
    {
        FirebaseServiceController.Instance.LogEvent(key);
    }
   
}
