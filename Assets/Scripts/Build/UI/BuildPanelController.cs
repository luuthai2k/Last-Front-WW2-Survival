using InviGiant.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Playables;

public class BuildPanelController : MonoBehaviour
{
    public BuildArea buildArea;
    private BuildControl currentBuildControl;
    public BuildProcessUIControl buildProcessUIControl;
    public Transform priceViewTrans;
    public RectTransform sourceCurrencyRectTrans, metalTargetRectTrans, metalSpawnRectTrans;
    public TextMeshProUGUI cashTxt, goldTxt, metalTxt;
    public TextMeshProUGUI pirceTxt, metalBtnTxt;
    public GameObject goBuildingLayout, goBuildBtn, goRewardAdsBtn, goCloseBtn, goPreviousBtn, goNextBtn, goSparksEffectPrefab, goBuildCompleteEffectPrefab;
    public Camera camera;
    BuildProcessInGameData buildProcessInGameData;
    BuildData buildData;
    Coroutine buildCoroutine;
    int currentIndexBuild;
    public Action callBackWhenClose = null;
    public LocalizeStringEvent nameLocalizeStringEvent;
    public bool isInit;
    public void Start()
    {
        Init();
    }
    private void OnEnable()
    {
        if (isInit)
        {
            UpdateTxt();
            SetUpBtn();
        }
       
    }
    public void Init()
    {
        LoadBuild(DataController.Instance.Build);
        buildProcessInGameData = DataController.Instance.GetBuildProcessInGameData();
        buildData = DataController.Instance.GetCurrentBuildData();
        UpdateTxt();
        SetUpBtn();
        float process = (float)buildProcessInGameData.process / buildProcessInGameData.cost;
        buildProcessUIControl.SetUp(buildData.buildProcessDatas, buildProcessInGameData.indexInData, process);
        isInit = true;
    }
    public void SetUpBtn()
    {
        if (DataController.Instance.Metal > 0)
        {
            goBuildBtn.SetActive(true);
            goRewardAdsBtn.SetActive(false);
            if (DataController.Instance.Metal < (buildProcessInGameData.cost - buildProcessInGameData.process))
            {
                Debug.LogError("DFTGBHJNKM");
               StartCoroutine( DelayDisplayCloseBtn(2));
              
            }

        }
        else
        {
            
            goRewardAdsBtn.SetActive(true);
            goBuildBtn.SetActive(false);
            StartCoroutine(DelayDisplayCloseBtn(1));
        }
       
    }
    public async void LoadBuild(int index)
    {
        goPreviousBtn.SetActive(false);
        goNextBtn.SetActive(false);
        priceViewTrans.gameObject.SetActive(false);
        GameObject goBuild = await buildArea.LoadBuild(index);
        if (goBuild == null)
        {
            ClosePanel();
        }
        currentBuildControl = goBuild.GetComponent<BuildControl>();
        currentIndexBuild = index;
        if (index == DataController.Instance.Build)
        {
            priceViewTrans.gameObject.SetActive(true);
            goPreviousBtn.SetActive(index > 0);
            goNextBtn.SetActive(false);
            goBuildingLayout.gameObject.SetActive(true);
            currentBuildControl.UpdateProcess();
        }
        else
        {
            goBuildingLayout.gameObject.SetActive(false);
            goNextBtn.SetActive(true);
            goPreviousBtn.SetActive(index > 0);
            currentBuildControl.OnComplete();
        }
        var localizedString = nameLocalizeStringEvent.StringReference;
        localizedString.Arguments = new object[] { index };
        nameLocalizeStringEvent.RefreshString();
    }
    IEnumerator DelayDisplayCloseBtn(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        Debug.LogError("True");
        goCloseBtn.SetActive(true);
    }
    public void Update()
    {
        if (currentBuildControl != null && priceViewTrans.gameObject.activeSelf)
        {
            Vector3 screenPos = camera.WorldToScreenPoint(currentBuildControl.GetSnapTargetPoint());
            priceViewTrans.position = screenPos;
        }
    }
    public void UpdateTxt()
    {
        pirceTxt.text = (buildProcessInGameData.cost - buildProcessInGameData.process).ToString();
        metalBtnTxt.text = DataController.Instance.Metal.ToString();
        goldTxt.text = ToolHelper.FormatLong2(DataController.Instance.Gold);
        cashTxt.text = ToolHelper.FormatLong2(DataController.Instance.Cash);
        metalTxt.text = ToolHelper.FormatLong2(DataController.Instance.Metal);
    }
    public void OnClickHoldToBuildBtnDown()
    {
        buildCoroutine = StartCoroutine(BuildCoroutine());

    }
    IEnumerator BuildCoroutine()
    {
        int step = 0;
        while (true)
        {
            if (DataController.Instance.Metal <= 0)
            {
                ReSetUpBtn();
                yield break;
            }
            step = Mathf.Min(buildProcessInGameData.cost / 20, 500);
            step = Mathf.Min(DataController.Instance.Metal, step);
            step = Mathf.Min(buildProcessInGameData.cost - buildProcessInGameData.process, step);
            buildProcessInGameData.process += step;
            DataController.Instance.AddMetal(-step);
            UpdateTxt();
            SpawmFillEffect();
            currentBuildControl.UpdateFill(0.1f);

            float process = (float)buildProcessInGameData.process / buildProcessInGameData.cost;
            buildProcessUIControl.UpdateProcess(buildProcessInGameData.indexInData, process, 0.1f);
            yield return new WaitForSeconds(0.1f);
            if (buildProcessInGameData.process >= buildProcessInGameData.cost)
            {
                yield return null;
                BuildComplete();
                currentBuildControl.OnFilled();
                yield break;
            }
        }
    }
    public void OnClickPreviousBtn()
    {
        LoadBuild(currentIndexBuild - 1);

    }
    public void OnClickNextBtn()
    {
        LoadBuild(currentIndexBuild + 1);

    }
    public void OnClickHoldToBuildBtnUp()
    {
        if (buildCoroutine != null)
        {
            StopCoroutine(buildCoroutine);
        }

    }
    public void SpawmFillEffect()
    {
        GameObject metal = SmartPool.Instance.Spawn(UIManager.Instance.metalItemPropPrefab, transform.GetChild(0));
        metal.transform.position = sourceCurrencyRectTrans.transform.position;
        metal.GetComponent<ItemProp>().MoveTween(priceViewTrans.position, sourceCurrencyRectTrans.position, sourceCurrencyRectTrans.sizeDelta, sourceCurrencyRectTrans.sizeDelta, 0.5f, 0,
            (() =>
            {
                GameObject spark = SmartPool.Instance.Spawn(goSparksEffectPrefab, transform.GetChild(0));
                spark.transform.position = priceViewTrans.position;
                AudioController.Instance.PlaySfx(GameConstain.WEAPON_UNLOCK_COUNTER);
                SmartPool.Instance.Despawn(metal);
            }), false);
    }
    public void BuildComplete()
    {
        priceViewTrans.gameObject.SetActive(false);
        HideAllButton();
        buildProcessInGameData.process = 0;
        SmartPool.Instance.Spawn(goBuildCompleteEffectPrefab, currentBuildControl.GetSnapTargetPoint(), Quaternion.identity);
        if (buildProcessInGameData.indexInData >= buildData.buildProcessDatas.Length - 1)
        {
            DataController.Instance.Build++;


            StartCoroutine(DelaySpawmReward(buildData.buildProcessDatas[buildProcessInGameData.indexInData].reward.Key, () =>
            {
                priceViewTrans.gameObject.SetActive(true);
                if (DataController.Instance.Build <= GameConstain.MAXBUILD)
                {
                    LoadBuild(DataController.Instance.Build);
                    buildData = DataController.Instance.GetCurrentBuildData();
                    ReSetUpBtn();
                    float process = (float)buildProcessInGameData.process / buildProcessInGameData.cost;
                    buildProcessUIControl.SetUp(buildData.buildProcessDatas, buildProcessInGameData.indexInData, process);
                }
                else
                {
                    goCloseBtn.SetActive(true);
                }
                UpdateTxt();

            }));
            buildProcessInGameData.indexInData = 0;
            buildData = DataController.Instance.GetCurrentBuildData();
            buildProcessInGameData.cost = buildData.buildProcessDatas[buildProcessInGameData.indexInData].cost;
            return;
        }
        else
        {
            StartCoroutine(DelaySpawmReward(buildData.buildProcessDatas[buildProcessInGameData.indexInData].reward.Key, () =>
            {
                priceViewTrans.gameObject.SetActive(true);
                UpdateTxt();
                ReSetUpBtn();
            }));
            buildProcessInGameData.indexInData++;
            buildProcessInGameData.cost = buildData.buildProcessDatas[buildProcessInGameData.indexInData].cost;
        }


    }
    IEnumerator DelaySpawmReward(string id, Action onComplete)
    {
        yield return new WaitForSeconds(2f);
        GameObject go = UIManager.Instance.SpawnLootBox(id + "_Build");
        go.GetComponent<CallbackWhenClose>().SetActionAfterClose(() =>
        {
            gameObject.SetActive(true);
            onComplete?.Invoke();

        });
        gameObject.SetActive(false);

    }
    public void ReSetUpBtn()
    {
        if (DataController.Instance.Metal > 0)
        {
            goBuildBtn.SetActive(true);
        }
        else
        {
            goRewardAdsBtn.SetActive(true);
        }
        goCloseBtn.SetActive(true);
    }
    public void HideAllButton()
    {
        goBuildBtn.SetActive(false);
        goCloseBtn.SetActive(false);
        goRewardAdsBtn.SetActive(false);
    }
    public void OnClickCloseBtn()
    {
        ClosePanel();
    }
    public void OnClickGetMoreBtn()
    {
        ManagerAds.ins.ShowRewarded((x) =>
        {
            if (x)
            {
                DataController.Instance.AddMetal(3000);
                SpawmMetalsEffect(metalSpawnRectTrans.position, metalTargetRectTrans.position, metalSpawnRectTrans.sizeDelta, metalTargetRectTrans.sizeDelta, 20);
                UpdateTxt();
                goBuildBtn.SetActive(true);
                goRewardAdsBtn.SetActive(false);

            }
        });
    }
    public void SpawmMetalsEffect(Vector3 spawnPos, Vector3 desPos, Vector2 sizeDelta, Vector2 targetSizeDelta, int amount)
    {
        amount = Math.Min(amount, 20);
        AudioController.Instance.PlaySfx(GameConstain.REWARD_POP);
        for (int i = 0; i < amount; i++)
        {
            GameObject metal = SmartPool.Instance.Spawn(UIManager.Instance.metalItemPropPrefab, transform.GetChild(0));
            metal.transform.position = spawnPos;
            metal.GetComponent<ItemProp>().MoveTween(desPos, spawnPos, sizeDelta, targetSizeDelta, 0.8f, i * 0.05f + 0.05f,
                (() =>
                {

                    SmartPool.Instance.Despawn(metal);
                }));
        }
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
        TopBarController.Instance.UpdateText();
        callBackWhenClose?.Invoke();
    }
    public void SetActionAfterClose(Action callBackWhenClose)
    {
        this.callBackWhenClose = callBackWhenClose;
    }
}
