using CW.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : SingletonPersistent<SceneController>
{
    bool isLoading = false;
    public GameObject loadingPanel;
    public TextMeshProUGUI processTxt,loadingTxt;
    public SceneInstance currentScene;
    public Image fillImg;
    bool isFirstOpenGame;
    public bool allowLoadingDeactivation=true;
    AsyncOperation asyncOperation;
    public void LoadScene(string sceneName,string keyInfo=null, bool allowLoadingDeactivation = true, bool currentAddressable = true)
    {
        this.allowLoadingDeactivation = allowLoadingDeactivation;
        if (isLoading) return;
        isLoading = true;
        StopAllCoroutines();
        if (currentAddressable )
            StartCoroutine(_LoadAddressableScene(sceneName));
        else 
            StartCoroutine(_LoadAddressableSceneFromNormal(sceneName));
        UpdateTextInfo(keyInfo);
    }
    async void UpdateTextInfo(string key)
    {
        loadingTxt.text = await LocalizationManager.Instance.GetLocalizedText(key);
    }
   
    private IEnumerator _LoadAddressableScene(string sceneName)
    {
        processTxt.text = "0%";
        fillImg.fillAmount = 0;
        float currentProgress = 0f;
        loadingPanel.gameObject.SetActive(true);
        ManagerAds.ins.ShowMrec();
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
            currentProgress = Mathf.Max(currentProgress, asyncOperation.progress / 10);
            processTxt.text = $"{(int)(currentProgress * 100)}%";
            fillImg.fillAmount = currentProgress;
        }
        float timeLeft =1f;
        var delay = new WaitForSecondsRealtime(0.1f);
        while (timeLeft >= 0.1f)
        {
            yield return delay;
            timeLeft -= 0.1f;
            currentProgress = 0.1f + 0.9f * (1f - timeLeft) / 1f;
            processTxt.text = $"{(int)(currentProgress * 100)}%";
            fillImg.fillAmount = currentProgress;
        }
        yield return new WaitForSeconds(0.1f);
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnSceneLoaded));
        yield return new WaitUntil(() => allowLoadingDeactivation);
        yield return new WaitForSeconds(0.5f);
        isLoading = false;
        loadingPanel.gameObject.SetActive(false);
        ManagerAds.ins.HideMrec();

    }
    private IEnumerator _LoadAddressableSceneFromNormal(string sceneName)
    {
        processTxt.text = "0%";
        fillImg.fillAmount = 0;
        float currentProgress = 0f;
        loadingPanel.gameObject.SetActive(true);
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
            currentProgress = Mathf.Max(currentProgress, asyncOperation.progress / 10);
            processTxt.text = $"{(int)(currentProgress * 100)}%";
            fillImg.fillAmount = currentProgress;
        }
        float  timeLeft=8f;
        var delay = new WaitForSecondsRealtime(0.1f);
        while (timeLeft >= 0.1f)
        {
            yield return delay;
            timeLeft -= 0.1f;
            currentProgress = 0.1f + 0.9f * (8 - timeLeft) / 8;
            processTxt.text = $"{(int)(currentProgress * 100)}%";
            fillImg.fillAmount = currentProgress;
        }
        yield return new WaitForSeconds(0.1f);
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnFirstTime));
        MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnSceneLoaded));
        yield return new WaitUntil(() => allowLoadingDeactivation);

        if (!isFirstOpenGame)
        {
            isFirstOpenGame = true;
            ManagerAds.ins.ShowAppOpenAd();
        }
        yield return new WaitForSeconds(0.5f);
        isLoading = false;
        loadingPanel.gameObject.SetActive(false);
        ManagerAds.ins.ShowBanner();


    }
    //public void ReLoadScene(bool allowLoadingDeactivation = true)
    //{
    //    this.allowLoadingDeactivation = allowLoadingDeactivation;
    //    if (isLoading) return;
    //    isLoading = true;
    //    StopAllCoroutines();
    //}
    //private IEnumerator _ReloadAddressableScene()
    //{
    //    processTxt.text = "0%";
    //    float currentProgress = 0f;
    //    loadingPanel.gameObject.SetActive(true);
    //    var asyncTask = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    //    while (!asyncTask.IsDone)
    //    {
    //        yield return null;
    //        currentProgress = Mathf.Max(currentProgress, asyncTask.PercentComplete / 2);
    //        processTxt.text = $"{(int)(currentProgress * 100)}%";
    //    }
    //    asyncTask.Result.ActivateAsync();
    //    var unloadTask = Addressables.UnloadSceneAsync(currentScene);
    //    yield return unloadTask;
    //    currentScene = asyncTask.Result;
    //    float timeLeft;
    //    timeLeft = 0.5f;
    //    var delay = new WaitForSecondsRealtime(0.1f);
    //    while (timeLeft >= 0.1f)
    //    {
    //        yield return delay;
    //        timeLeft -= 0.1f;
    //        currentProgress = 0.5f + 0.5f * (0.5f - timeLeft) / 0.5f;
    //        processTxt.text = $"{(int)(currentProgress * 100)}%";
    //    }
    //    yield return new WaitForSeconds(0.1f);
    //    MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnSceneLoaded));
    //    yield return new WaitUntil(() => allowLoadingDeactivation);
    //    isLoading = false;
    //    loadingPanel.gameObject.SetActive(false);


    //}
}
