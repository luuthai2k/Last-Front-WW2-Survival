using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockWeaponPanel : MonoBehaviour
{
    public Image weaponIcon,weaponFillIcon;
    public TextMeshProUGUI processTxt, nameTxt;
    public GameObject goNextUnlock, goUnlock;
    WeaponInGameData weaponInGameData;
    Action onComplete;
    bool isFree;
    public void SetUp(Sprite icon, int currentProcess, int targetProcess,bool isFree,WeaponInGameData data,Action onComplete=null)
    {
        weaponIcon.sprite = icon;
        weaponFillIcon.sprite = icon;
        processTxt.text = currentProcess.ToString();
        weaponFillIcon.fillAmount = (float)(100-currentProcess) / 100f;
        this.onComplete = onComplete;
        this.isFree = isFree;
        weaponInGameData = data;
        StartCoroutine(UpdateProcess(currentProcess, targetProcess, data.ID));
       
    }
    IEnumerator UpdateProcess(int currentProcess, int targetProcess, string ID)
    {
        Debug.Log( targetProcess);
        yield return new WaitForSeconds(0.5f);
        DOTween.To(() => currentProcess, x =>
        {
            processTxt.text =$"{x}%";
            weaponFillIcon.fillAmount = (float)(100-x) / 100;

        }, targetProcess, 1).SetEase(Ease.Linear);
        while (weaponFillIcon.fillAmount != (float)(100 - targetProcess) / 100)
        {
            AudioController.Instance.PlaySfx(GameConstain.WEAPON_UNLOCK_COUNTER);
            yield return new WaitForSeconds(0.075f);
        }
        if (targetProcess == 100)
        {
            if (isFree)
            {
                UnLockWeapon();
            }
            else
            {
                goNextUnlock.SetActive(false);
                goUnlock.SetActive(true);
                LoadNameText(ID);
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            onComplete?.Invoke();
            gameObject.SetActive(false);
           
        }

    }

    async void LoadNameText(string ID)
    {
        nameTxt.text = await LocalizationManager.Instance.GetLocalizedText(ID);
    }
    public void OnClickUnLockBtn()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
            UnLockWeapon();
          
        });
    }
    public void UnLockWeapon()
    {
        var go=UIManager.Instance.SpawmWeapon(weaponInGameData);
        weaponInGameData.isOwned = true;
        go.GetComponent<CallbackWhenClose>().SetActionAfterClose(onComplete);
        gameObject.SetActive(false);
        FirebaseServiceController.Instance.LogEvent($"UNLOCK_{weaponInGameData.ID}_REWARD");
    }
    public void OnClickLoseBtn()
    {
        onComplete?.Invoke();
        gameObject.SetActive(false);
    }
}
