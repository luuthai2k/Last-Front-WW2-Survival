using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class GamePlayUIManager : Singleton<GamePlayUIManager>
{
    public GamePlayMenu gamePlayMenu;
    public WinPopUp winPopUp;
    public FailPopUp failPopUp;
    public RevivePopUp revivePopUp;
    //public UnlockWeaponPanel unlockWeaponPanel;
    public GameObject goSettingPanel,tryWeaponPrefab,unLockWeaponPrefab;
    public IntroPanelManager introPanelManager;
    private void Start()
    {
        winPopUp.gameObject.SetActive(false);
        failPopUp.gameObject.SetActive(false);
    }
    public void OnWin()
    {

      
        StartCoroutine(DelayShowWinPopUp());
    }
    IEnumerator DelayShowWinPopUp()
    {
        yield return new WaitForSeconds(3);
        ManagerAds.ins.ShowInterside();
        winPopUp.gameObject.SetActive(true);
        AudioController.Instance.StopMusic();
    }
    public void OnFail()
    {
        StartCoroutine(DelayShowRevivePopUp());
        FirebaseServiceController.Instance.LogEvent($"FAIL_{DataController.Instance.Level}_{GameManager.Instance.levelControl.currentWave}");
    }
    IEnumerator DelayShowRevivePopUp()
    {
        yield return new WaitForSeconds(3.5f);

        revivePopUp.gameObject.SetActive(true);
    }
    public void OpenFailPopUp()
    {
        ManagerAds.ins.ShowInterside();
        AudioController.Instance.StopMusic();
        failPopUp.gameObject.SetActive(true);
    }
    public void OnClickSettingBtn()
    {
        Time.timeScale = 0;
        goSettingPanel.SetActive(true);
    }
  public void GetUnlockWeaponPanel(Sprite icon, int currentProcess, int targetProcess,bool isFree, WeaponInGameData data, Action onComplete = null)
    {
        GameObject go = Instantiate(unLockWeaponPrefab, transform);
        go.GetComponent<UnlockWeaponPanel>().SetUp(icon, currentProcess, targetProcess, isFree, data, () =>
        {
            onComplete?.Invoke();
        });
    }
    public async void ShowTryWeapon(WeaponInGameData data, int tagId)
    {
        
        AsyncOperationHandle<SpriteAtlas> obj = Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Weapon_Icon.spriteatlas");
        await obj.Task;
        var icon = obj.Result.GetSprite("Icon_" + data.ID);
        GetTryWeaponPanel(icon, data, tagId);
    }
    public void GetTryWeaponPanel(Sprite icon, WeaponInGameData weaponInGameData, int tagId)
    {
        GameObject go = Instantiate(tryWeaponPrefab, transform);
        go.GetComponent<TryWeaponPanelControl>().SetUp(icon, weaponInGameData, tagId);
       
    }
    public async void SpawnIntro(string key,Action onComplete)
    {
      
        string intro= await LocalizationManager.Instance.GetLocalizedText(key);
        if (intro == null || intro == "")
        {
            onComplete?.Invoke();
            return;
        }
        var introSplit = intro.Split("_");
        introPanelManager.Spawn(introSplit[1], introSplit[0], onComplete);
    }
   
}
