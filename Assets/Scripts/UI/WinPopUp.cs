using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class WinPopUp : MonoBehaviour
{
    public TextMeshProUGUI cashTxt, goldTxt, metalTxt;
    public TextMeshProUGUI  killProcessTxt, headShotsProcessTxt, healthProcessTxt, killRewardTxt, headShotsRewardTxt, healthRewardTxt, totalCashRewardTxt, totalMetalRewardTxt, rewardAdsTxt;
    public int totalCash;
    public int totalMetal;
    public RectTransform arrownTrans,cashSpawmTrans, metalSpawmTrans, cashAdsSpawmTrans, cashTargetTrans, metalTargetTrans;
    public Button rewardAdsBtn;
    public RewardProcess[] processes;
    int muti = 0;
    int currentRewardProcess;
    Tween tween;
    private void Start()
    {
        UpdateText();
        UpdateRewardProcess();
        tween = arrownTrans.DOAnchorPosX(-arrownTrans.anchoredPosition.x, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
    }
    
    public void UpdateText()
    {
        goldTxt.text = ToolHelper.FormatLong2(DataController.Instance.Gold);
        cashTxt.text = ToolHelper.FormatLong2(DataController.Instance.Cash);
        metalTxt.text = ToolHelper.FormatLong2(DataController.Instance.Metal);
        StartCoroutine(DelayUpdateText());
    }
    IEnumerator DelayUpdateText()
    {
        float numberEnermy = GameManager.Instance.levelControl.GetNumberEnermy();
        yield return new WaitForSeconds(0.5f);
        AudioController.Instance.PlaySfx(GameConstain.MAINREWARD);
        DOTween.To(() => 0, x =>
        {
            killProcessTxt.text = $"{(int)x}/{numberEnermy}";
            killRewardTxt.text = ((int)(100 * x / numberEnermy)).ToString();
            totalCash = (int)(100 * x / numberEnermy);
            totalCashRewardTxt.text = totalCash.ToString();
        }, numberEnermy, 0.5f).SetEase(Ease.OutSine);
        int cashHeadshots = 0;
        yield return new WaitForSeconds(0.6f);
        //
        if (GameManager.Instance.headshot > 0)
        {
            AudioController.Instance.PlaySfx(GameConstain.MAINREWARD);
            float headshot = GameManager.Instance.headshot;
            DOTween.To(() => 0, x =>
            {
                headShotsProcessTxt.text = $"{(int)x}/{numberEnermy}";
                cashHeadshots = (int)(100 * x / numberEnermy);
                headShotsRewardTxt.text = cashHeadshots.ToString();
                totalCash = 100 + cashHeadshots;
                totalCashRewardTxt.text = totalCash.ToString();
            }, headshot, 0.5f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.6f);
            
        }
        //
        int health = (int)(PlayerController.Instance.playerHealth.GetRateHealth() * 100);
        int cashHealth = 0;
        AudioController.Instance.PlaySfx(GameConstain.MAINREWARD);
        DOTween.To(() => 0, x =>
        {
            healthProcessTxt.text = $"{(int)x}%";
            cashHealth = (int)(100f * x / 100);
            healthRewardTxt.text = cashHealth.ToString();
            totalCash = 100 + cashHeadshots + cashHealth;
            totalCashRewardTxt.text = totalCash.ToString();
        }, health, 0.5f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.6f);
        totalCash = 100 + cashHeadshots + cashHealth;
        totalCashRewardTxt.text =ToolHelper.FormatLong2( totalCash);
        DataController.Instance.AddCash(totalCash);
        if (DataController.Instance.Level >= 6)
        {
            if (DataController.Instance.Level == 6)
            {
                totalMetal = 3000;
            }
            else
            {
                totalMetal = UnityEngine.Random.Range(1000, 1200);
            }
            AudioController.Instance.PlaySfx(GameConstain.MAINREWARD);
            DOVirtual.Int(0, totalMetal, 0.5f, (x) =>
            {
                totalMetalRewardTxt.text = ToolHelper.FormatLong2(x);
            });
            DataController.Instance.AddMetal(totalMetal);
            yield return new WaitForSeconds(0.5f);
        }
        SpawnReward();
    }
    public void SpawnReward()
    {
        UIManager.Instance.SpawmCashsEffect(cashSpawmTrans.position, cashTargetTrans.position, cashSpawmTrans.sizeDelta, cashTargetTrans.sizeDelta, totalCash / 10);
        UIManager.Instance.SpawmMetalsEffect(metalSpawmTrans.position, metalTargetTrans.position, metalSpawmTrans.sizeDelta, metalTargetTrans.sizeDelta, totalMetal / 10);
        UpdateTextCashUseTween(0.5f, 1f);
        UpdateTextMetalUseTween(0.5f, 1f);
    }

    void UpdateTextCashUseTween(float duration = 1f, float delay = 0f)
    {
        int cash = ToolHelper.ParseFormattedStringToInt(cashTxt.text);
        DOVirtual.Int(cash, DataController.Instance.Cash, duration, (x) =>
        {
            cashTxt.text = x.ToString();
        }).SetEase(Ease.OutSine).SetDelay(delay);
    }
    void UpdateTextMetalUseTween(float duration = 1f, float delay = 0f)
    {
        int metal = ToolHelper.ParseFormattedStringToInt(metalTxt.text);
        DOVirtual.Int(metal, DataController.Instance.Metal, duration, (x) =>
        {
            metalTxt.text = x.ToString();
        }).SetEase(Ease.OutSine).SetDelay(delay);
    }
    public void UpdateRewardProcess()
    {
        currentRewardProcess = DataController.Instance.Level % 10-1;
        if (currentRewardProcess == -1)
        {
            currentRewardProcess = 9;
        }
        for(int i = 0; i < processes.Length; i++)
        {
            if (i < currentRewardProcess-1)
            {
                processes[i].doneProcess.SetActive(true);
            }
            else if (i == currentRewardProcess-1)
            {
                processes[i].currentProcess.SetActive(true);
                break;
            }
        }
        StartCoroutine(DelayUpdateRewardProcess());
    }
    IEnumerator DelayUpdateRewardProcess()
    {
        yield return new WaitForSeconds(1f);
        if (currentRewardProcess > 0)
        {
            processes[currentRewardProcess - 1].doneProcess.SetActive(true);
            processes[currentRewardProcess - 1].currentProcess.SetActive(false);
        }
        RectTransform currentProcessRectTrans = processes[currentRewardProcess].currentProcess.GetComponent<RectTransform>();
        Vector2 targetSize = currentProcessRectTrans.sizeDelta;
        currentProcessRectTrans.sizeDelta=processes[currentRewardProcess].doneProcess.GetComponent<RectTransform>().sizeDelta;
        currentProcessRectTrans.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        AudioController.Instance.PlaySfx(GameConstain.CHESTPROGRESS);
        currentProcessRectTrans.DOSizeDelta(targetSize, 0.25f).SetEase(Ease.OutBack);
        currentProcessRectTrans.transform.DOPunchScale(Vector3.one*0.2f, 0.25f);
    }
    public void OnClickContinueBtn()
    {
        Continue();
    }
    public async void Continue()
    {
        DataController.Instance.Level++;
        DataController.Instance.IsNewLevel = true;
        gameObject.SetActive(false);
        if (GameManager.Instance.IsTryWeapon(out WeaponInGameData tryWeaponData, out int index))
        {
            var shopdata = DataController.Instance.GetShopWeaponData(tryWeaponData.weaponType, index);
            Debug.LogError(shopdata.packID);
            GameObject go = await UIManager.Instance.GetOfferPanel(shopdata.packID);
            if (go != null)
            {
                go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
                {
                    CheckUnlockWeapon();
                });
                go.GetComponent<PackController>().onCompletePurchase.AddListener(LogEventTryWeapon);
                return;
            }
        }
        CheckUnlockWeapon();
        DataController.Instance.SaveData();
    }
    public void LogEventTryWeapon()
    {
        FirebaseServiceController.Instance.LogEvent($"PURCHASE_TRIAL_LEVEL_{DataController.Instance.Level}");
    }
    public void ClaimReward()
    {
        if (DataController.Instance.Level == 3)
        {
            GamePlayUIManager.Instance.gameObject.SetActive(false);
            GameObject go = UIManager.Instance.SpawnLootBox("SmallCrate",true);
            go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
            {
            
                SceneController.Instance.LoadScene(GameConstain.MainMenu, "Main_menu_loading");
                GamePlayUIManager.Instance.gameObject.SetActive(true);
            });
            return;
        }
        var data = GameManager.Instance.levelData;
        Pack pack = new Pack();
        pack.Crates = data.Crates;
        UIManager.Instance.GetPackReward(pack, () =>
        {
            SceneController.Instance.LoadScene(GameConstain.MainMenu, "Main_menu_loading");
        });
    }
    private void Update()
    {
        if (!arrownTrans.gameObject.activeSelf) return;
        float x = Mathf.Abs(arrownTrans.anchoredPosition.x);
       
        if (x >= 265)
        {
            if (muti == 2) return;
                muti = 2;
            AudioController.Instance.PlaySfx(GameConstain.RANK_PICKUP);
        }
        else if (x >= 160)
        {
            if (muti == 3) return;
            muti = 3;
            AudioController.Instance.PlaySfx(GameConstain.RANK_PICKUP);
        }
        else if (x >= 55)
        {
            if (muti == 5) return;
            muti = 5;
            AudioController.Instance.PlaySfx(GameConstain.RANK_PICKUP);
        }
        else
        {
            if (muti == 10) return;
            muti = 10;
            AudioController.Instance.PlaySfx(GameConstain.RANK_PICKUP);
        }
        rewardAdsTxt.text= ToolHelper.FormatLong2(totalCash * muti);
    }
    public void OnClickRewardAdsBtn()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
            tween.Pause();
            GetRewardAds();
        });
       
    }
    public void GetRewardAds()
    {
        int cash = totalCash * muti;
        UIManager.Instance.SpawmCashsEffect(cashAdsSpawmTrans.position, cashTargetTrans.position, cashAdsSpawmTrans.sizeDelta, cashTargetTrans.sizeDelta, cash / 10);
        DataController.Instance.AddCash(cash);
        UpdateTextCashUseTween(0.5f, 1f);
        rewardAdsBtn.interactable = false;
        rewardAdsBtn.GetComponent<CanvasGroup>().alpha = 0.5f;
        Continue();
        FirebaseServiceController.Instance.LogEvent($"REWARD_WIN_MULTIPLY");

    }
   
    public async void CheckUnlockWeapon()
    {
        int level = DataController.Instance.Level;
        int index = (level - 2) / 4;
        var uwpd = DataController.Instance.GetUnlockWeaponData(index);
        Debug.LogError(uwpd.ID);
        if (uwpd == null)
        {
            CheckKeyCrate();
            return;
        }
        var wpid = DataController.Instance.GetWeaponIngameData(uwpd.weaponType, uwpd.indexIngameData);
        if (wpid.ID != uwpd.ID)
        {
            wpid = DataController.Instance.GetWeaponIngameData(uwpd.weaponType, uwpd.ID);
        }
        if (wpid.isOwned)
        {
            CheckKeyCrate();
            return;
        }
        AsyncOperationHandle<SpriteAtlas> obj = Addressables.LoadAssetAsync<UnityEngine.U2D.SpriteAtlas>("Weapon_Icon.spriteatlas");
        await obj.Task;
        var icon = obj.Result.GetSprite("Icon_" + wpid.ID);
        int process = (((DataController.Instance.Level-1) % 4)) * 25;
        process = process == 0 ? 100 : process;
        GamePlayUIManager.Instance.GetUnlockWeaponPanel(icon, process - 25, process, uwpd.isFree, wpid, () =>
        {
            CheckKeyCrate();
        });
    }
    public void CheckKeyCrate()
    {
        if (DataController.Instance.Key >= 3)
        {
            GameObject go = UIManager.Instance.SpawnAirdropPanel();
            go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
            {
                CheckClaimProcessReward();
            });
        }
        else
        {
            CheckClaimProcessReward();
        }
    }
    public void CheckClaimProcessReward()
    {
        if (currentRewardProcess == 9)
        {
            GamePlayUIManager.Instance.gameObject.SetActive(false);
            GameObject go = UIManager.Instance.SpawnLootBox("MediumCrate");
            go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
            {
                GamePlayUIManager.Instance.gameObject.SetActive(true);
                ClaimReward();
               
            });
        }
        else
        {
            ClaimReward();
        }
    }
}
[Serializable]
public class RewardProcess
{
    public GameObject currentProcess,doneProcess;

}
