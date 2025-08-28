using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.adjust.sdk;
using Firebase.Analytics;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using GoogleMobileAds.Ump.Api;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Runtime.InteropServices;
using Facebook.Unity;
using System.Globalization;
//using System.Drawing;

public class ManagerAds : MonoBehaviour
{
    public static ManagerAds ins;

    [Header("Advertisement Key")]
    public string SDK_KEY;

    //ID Banner
    [Header("Banner Options")]
    public string bannerAdUnitId = "YOUR_ANDROID_BANNER_AD_UNIT_ID";
    public Color colorBanner = new Color(255, 255, 255);

    //ID Interstitial
    [Header("Interstitial Options")]
    public string interstitialAdUnitId = "YOUR_ANDROID_INTERSTITIAL_AD_UNIT_ID";
    //ID Interstitial
    [Header("RewardedAds Options")]
    public string rewardedAdsAdUnitId = "YOUR_ANDROID_REWARDEDADS_AD_UNIT_ID";

    [Header("Other")]
    private bool receivedRewardComplete = false;
    private UnityAction OnInterstitialClosed;
    private UnityAction<bool> OnCompleteRewardMethod;
    public int timeShowAds = 30;

    public bool isOpenAds = false;

    private bool isAdsShow = false;
    private void Awake()
    {
        ins = this;
        DontDestroyOnLoad(this.gameObject);
        Debug.Log(isOpenAds + "and" + isAdsShow);
    }

    private void OnEnable()
    {
       
        InitAds();
        InitFaceBook();
        InitFbase();
    }

    public void Start()
    {
      
        StarCMP_Admob();
    }

    void InitAds()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
#if UNITY_IOS
        AudienceNetwork.IOSAdSettings.SetAdvertiserTrackingEnabled(true);
#endif
    }

    public void InitLoadAds()
    {
        MobileAds.Initialize((initStatus) =>
        {
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
            LoadAppOpenAd();
  
        });
        Debug.Log("ManagerAds MAX Start");
        InitializeMAX();

    }
    //
    void InitializeMAX()
    {
        MaxSdk.SetSdkKey(SDK_KEY);
        //MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        //Add Init Event
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads
            InitEvents();
            InitializeMRecAds();
        };

    }

    private void LoadAds()
    {

        //LoadBanner();
        //HideBanner();
        //LoadInterstitial();
        //LoadRewardedAd();
    }


    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }


    /////////////////////////interstitialAd////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////

    int retryAttemptInterstitialAd;
    public void InitializeInterstitialAds()
    {
        // Load the first interstitial
        LoadInterstitial();
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttemptInterstitialAd = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttemptInterstitialAd++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptInterstitialAd));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        ShowBanner();
        LoadInterstitial();
        isAdsShow = false;
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        ShowBanner();
        LoadInterstitial();
        CompleteInterstitial();
        if (isOpenAds)
        {
            Debug.LogError("Close Ads and Show Open Add");
            ShowAppOpenAd();
        }
        isAdsShow = false;
    }


    /////////////////////////RewardedAds////////////////////////////
    ////////////////////////////////////////////////////////////////

    int retryAttemptRewardedAds;

    public void InitializeRewardedAds()
    {
        // Load the first rewarded ad
        LoadRewardedAd();
    }


    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttemptRewardedAds = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttemptRewardedAds++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptRewardedAds));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        ShowBanner();
        LoadRewardedAd();
        isAdsShow = false;
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        ShowBanner();
        LoadRewardedAd();
        if (isOpenAds)
        {
            ShowAppOpenAd();
        }
        isAdsShow = false;
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        ShowBanner();
        receivedRewardComplete = true;
        CompleteRewardedVideo(true);
        LoadRewardedAd();
    }


    private void InitEvents()
    {
        LoadRewardedAd();
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRevenueMAXPaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        LoadInterstitial();
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnRevenueMAXPaidEvent;


        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, colorBanner);

        LoadBanner();
        HideBanner();
        //Add AdInfo Banner Events
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnRevenueMAXPaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
    }

    public void LoadBanner()
    {
        MaxSdk.LoadBanner(bannerAdUnitId);
    }

    public void ShowBanner()
    {
        Debug.LogWarning("ShowBanner");
        if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        {
            MaxSdk.ShowBanner(bannerAdUnitId);
        }

    }

    public void HideBanner()
    {
        MaxSdk.HideBanner(bannerAdUnitId);
    }

    public void DestroyBanner()
    {
        MaxSdk.DestroyBanner(bannerAdUnitId);
    }


    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
    }


    private int timeCountDown = 0;
    private bool isActiveInter = false;
    public int startLevelShowAds = 3;
    public void ShowInterside()
    {
        if (DataController.Instance.Level < startLevelShowAds) return;
        if (PlayerPrefs.GetInt("removeAds", 0) == 1) return;
        Debug.LogWarning("Show Inter");
        if (timeCountDown <= 0)
        {
            Debug.LogWarning("Can Show Inter");
            ShowInterstitial(null);
            timeCountDown = timeShowAds;
            isActiveInter = true;
            StartCoroutine(WaitShowInter());
        }
    }
    private IEnumerator WaitShowInter()
    {
        while (isActiveInter)
        {
            yield return new WaitForSeconds(1f);
            timeCountDown -= 1;
            if (timeCountDown <= 0)
            {
                isActiveInter = false;
            }
        }
    }
    public void ShowInterstitial(UnityAction callback = null)
    {

        if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        {
            if (MaxSdk.IsInterstitialReady(interstitialAdUnitId))
            {
                HideBanner();
                MaxSdk.ShowInterstitial(interstitialAdUnitId);
                OnInterstitialClosed = callback;
                isAdsShow = true;
                FirebaseAnalytics.LogEvent("SHOW_INTER");
                Debug.LogError("Show Inter");
            }
            else
            {
                LoadInterstitial();
            }
        }
    }

    private void CompleteInterstitial()
    {
        if (OnInterstitialClosed != null)
        {
            OnInterstitialClosed();
            OnInterstitialClosed = null;
        }
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(rewardedAdsAdUnitId);
    }

    public void ShowRewarded(UnityAction<bool> callback = null)
    {
       Debug.LogWarning("ShowRewarded");
        if (MaxSdk.IsRewardedAdReady(rewardedAdsAdUnitId))
        {
            receivedRewardComplete = false;
            OnCompleteRewardMethod = callback;
            HideBanner();
            MaxSdk.ShowRewardedAd(rewardedAdsAdUnitId);
            Debug.LogWarning("isAdsShow");
            isAdsShow = true;
            FirebaseAnalytics.LogEvent("SHOW_REWARDED");
        }
        else
        {
            LoadRewardedAd();
        }
    }
    public bool isLoadedRewarded()
    {
        return MaxSdk.IsRewardedAdReady(rewardedAdsAdUnitId);
    }

    void CompleteRewardedVideo(bool canAction)
    {
        if (OnCompleteRewardMethod != null)
        {
            OnCompleteRewardMethod(canAction);
            OnCompleteRewardMethod = null;
        }
    }

    #region Rev MAX to Firebase && Adjust

    private void OnRevenueMAXPaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
        SendRevenueMAXToAdjust(adUnitId, adInfo);
        SendRevenueMAXToFirebase(adInfo);
    }

    void SendRevenueMAXToAdjust(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        var adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
        adRevenue.setRevenue(adInfo.Revenue, "USD");
        adRevenue.setAdRevenueNetwork(adInfo.NetworkName);
        adRevenue.setAdRevenueUnit(adInfo.AdUnitIdentifier);
        adRevenue.setAdRevenuePlacement(adInfo.Placement);
        Adjust.trackAdRevenue(adRevenue);
    }

    private void SendRevenueMAXToFirebase(MaxSdkBase.AdInfo adInfo)
    {
        if (adInfo == null) return;
        if (adInfo.Revenue < 0) return;

        Firebase.Analytics.Parameter[] adParameters = {
        new Firebase.Analytics.Parameter("ad_platform", "Applovin"),
        new Firebase.Analytics.Parameter("ad_source", adInfo.NetworkName),
        new Firebase.Analytics.Parameter("ad_unit_name", adInfo.AdUnitIdentifier),
        new Firebase.Analytics.Parameter("ad_format", adInfo.AdFormat),
        new Firebase.Analytics.Parameter("currency","USD"),
        new Firebase.Analytics.Parameter("value", adInfo.Revenue)
        };
        FirebaseAnalytics.LogEvent("ad_impression", adParameters);
    }
    #endregion

    #region =============== EVENT ADREVENUE ADMOD=================

    public void SendRevenueAdmob(AdValue args)
    {
        SendRevenueAdmodToAdjust(args);
        SendRevenueAdmodToFirebase(args);
    }

    public void SendRevenueAdmodToAdjust(AdValue args)
    {
        if (args == null) return;
        double value = args.Value * 0.000001f;
        AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
        adRevenue.setRevenue(value, "USD");
        Adjust.trackAdRevenue(adRevenue);
    }

    public void SendRevenueAdmodToFirebase(AdValue args)
    {
        if (args == null) return;
        double value = args.Value * 0.000001f;

        Firebase.Analytics.Parameter[] adParameters = {
        new Firebase.Analytics.Parameter("ad_source", "admob"),
        new Firebase.Analytics.Parameter("ad_format", "app_open"),
        new Firebase.Analytics.Parameter("currency","USD"),
        new Firebase.Analytics.Parameter("value", value)
        };
        FirebaseAnalytics.LogEvent("ad_impression", adParameters);
    }

    #endregion


    #region OPEN ADS

    //-------------------------------OPEN ADS----------------------------------------

    [Header("App Open Ads")]
    // These ad units are configured to always serve test ads.
    public string _adUnitId = "ca-app-pub-3940256099942544/9257395921";
    private AppOpenAd appOpenAd;

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }
    public bool IsAdAvailable()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Loads the app open ad.
    /// </summary>
    public void LoadAppOpenAd()
    {
        // Clean up the old ad before loading a new one.
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(_adUnitId, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                appOpenAd = ad;
                RegisterEventHandlers(ad);
            });
    }
    private int countLoadOpenAds = 0;
    private void ReLoadOpenAds()
    {
        if (countLoadOpenAds < 11)
        {
            countLoadOpenAds++;
            LoadAppOpenAd();
        }
    }
    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            //Debug.Log(String.Format("App open ad paid {0} {1}.",
            //    adValue.Value,
            //    adValue.CurrencyCode));
            SendRevenueAdmob(adValue);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };

        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
            ShowBanner();
            LoadAppOpenAd();
            isOpenAds = false;

        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
            ReLoadOpenAds();
            isOpenAds = false;
            ShowBanner();
        };
    }
    private void OnAppStateChanged(AppState state)
    {
        Debug.Log("App State changed to : " + state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground&&UnityEngine.SceneManagement.SceneManager.GetActiveScene().name!="Loading")
        {
            Debug.LogError("State App");
            isOpenAds = true;
            if (!isAdsShow)
            {
                Debug.LogError("Show Open Add");
                ShowAppOpenAd();
            }
        }
    }
    public void ShowAppOpenAd()
    {
        Debug.LogWarning(startLevelShowAds);
        if (DataController.Instance.Level < startLevelShowAds) return;
        if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        {
            if (IsAdAvailable())
            {
                Debug.Log("Showing app open ad.");
                appOpenAd.Show();
                HideBanner();

            }
            else
            {
                Debug.LogError("App open ad is not ready yet.");
                LoadAppOpenAd();
                //ShowBanner();

            }
        }
    }

    #endregion

    #region CMP Admob



    void StarCMP_Admob() // Chay CMP khi bat dau game
    {
        Debug.LogError("CMP: START :isCMPConsent: " + isCMPConsent());
        InitLoadAds();
        if (isCMPConsent())
        {
            MaxSdk.SetHasUserConsent(true);
            LoadAds();
        }
        else
        {
            ResetCMP_Admob();
        }
    }
    public void ResetCMP_Admob() // Show CMP 
    {
        ConsentInformation.Reset();
        ConsentRequestParameters request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
        };
        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    private void OnConsentInfoUpdated(FormError consentError)
    {

        if (consentError != null)
        {
            // Handle the error.
            MaxSdk.SetHasUserConsent(true);

            Debug.LogError(consentError);
            return;
        }
        Time.timeScale = 0;
        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
        {
            if (formError != null)
            {
                Time.timeScale = 1;
                // Consent gathering failed.
                MaxSdk.SetHasUserConsent(true);
                LoadAds();
                Debug.LogError(consentError);
                return;
            }

            // Consent has been gathered.
            if (ConsentInformation.CanRequestAds()) // Ham nay chi dam bao la da hoan thanh dc CMP
            {
                Debug.LogError("CMP: START 03 : " + isCMPConsent());
                if (isCMPConsent())
                {
                    MaxSdk.SetHasUserConsent(true);

                }
                else
                {
                    MaxSdk.SetHasUserConsent(false);

                }
            }
            else
            {
                MaxSdk.SetHasUserConsent(false);

            }
            Time.timeScale = 1;
            LoadAds();
        });
    }
    private bool isCMPConsent()
    {
        var CMPString = "";
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                CMPString = GT.Utils.PlayerPrefsNative.GetString("IABTCF_AddtlConsent", "");
                break;
            case RuntimePlatform.IPhonePlayer:
                CMPString = PlayerPrefs.GetString("IABTCF_AddtlConsent", "");
                break;
            default:
                return true;
        }
        Debug.LogError("CMPString: " + CMPString);
        return CMPString.Contains("2878") || CMPString.Length >= 4;
    }

    #endregion

    #region Init Firebase
    private void InitFbase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                FirebaseServiceController.Instance.isFirebaseInited = true;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    #endregion

    #region Init Facebook SDK
    private void InitFaceBook()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    #endregion

    #region MrecBanner
    public string mrecAdUnitId = "«Android-ad-unit-ID»"; // Retrieve the ID from your account
    private int countLoadMrecAd = 0;

    public void InitializeMRecAds()
    {
        // MRECs are sized to 300x250 on phones and tablets
        LoadMRecAds();
        //HideMRecAds();
        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnRevenueMAXPaidEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnMRecAdExpandedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnMRecAdCollapsedEvent;
        Debug.LogError("Create MREC ADS");

    }
    public void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
    }

    public void OnMRecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error)
    {
        countLoadMrecAd++;
        if (countLoadMrecAd < 4)
        {
            LoadMRecAds();
        }
    }

    public void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }


    public void OnMRecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void LoadMRecAds()
    {
        var density = MaxSdkUtils.GetScreenDensity();
        var safeAreaWidth = Screen.width / density;
        var safeAreaHeight = Screen.height / density;
        var mrecWidth = 300;
        var mrecHeight = 250;
        var bottomMargin = 120;
        var xPosition = (safeAreaWidth - mrecWidth) / 2;
        var yPosition = safeAreaHeight - mrecHeight - bottomMargin;
        MaxSdk.CreateMRec(mrecAdUnitId, xPosition, yPosition);
    }

    public void ShowMRecAds()
    {
        MaxSdk.ShowMRec(mrecAdUnitId);
        Debug.LogError("SHOW MREC ADS");
    }

    public void HideMRecAds()
    {
        MaxSdk.HideMRec(mrecAdUnitId);
        Debug.LogError("HIDE MREC ADS");
    }

    public void DestroyMRecAds()
    {
        MaxSdk.DestroyMRec(mrecAdUnitId);
    }

    #endregion
    


    public void HideMrec()
    {
        HideMRecAds();
    }


    public void ShowMrec()
    {
        if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        {
            ShowMRecAds();
        }
           
    }

    public void DestroyMrec()
    {
        DestroyMRecAds();
    }

 

}