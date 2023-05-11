using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;


    [SerializeField] string Interstitial_Android = "Interstitial_Android";
    [SerializeField] string Interstitial_iOS = "Interstitial_iOS";
    string InterstitialUnitId;

    [SerializeField] Button _showAdButton;
    [SerializeField] string Rewardedandroid = "Rewarded_Android";
    [SerializeField] string RewardediOS = "Rewarded_iOS";
    string RewardedUnitId = null;


    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    [SerializeField] string Bannerandroid = "Banner_Android";
    [SerializeField] string BanneriOS = "Banner_iOS";
    string BannerUnitId = null;


    void Awake()
    {
        InterstitialUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? Interstitial_iOS
            : Interstitial_Android;

        RewardedUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? RewardediOS
            : Rewardedandroid;


        BannerUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? BanneriOS
            : Bannerandroid;

        _showAdButton.interactable = false;

        InitializeAds();
    }

    void Start()
    {
        Advertisement.Banner.SetPosition(_bannerPosition);
        Invoke("OpenInterstitial",7);
        
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadAd();
        LoadBanner();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    //------------------------------------------------------------------------------------------------------------------

    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + InterstitialUnitId);
        Advertisement.Load(InterstitialUnitId, this);
        Advertisement.Load(RewardedUnitId, this);
    }

    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(BannerUnitId, options);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        ShowBannerAd();
    }

    void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        Advertisement.Banner.Show(BannerUnitId, options);
    }

    public void ShowAdInterstitial()
    {
        FactoryEventServices.GameAction.PauseGame?.Invoke();
        FactoryEventServices.isPlay = false;
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + InterstitialUnitId);
        Advertisement.Show(InterstitialUnitId, this);
    }

    public void ShowAdRewared()
    {
        _showAdButton.interactable = false;
        Advertisement.Show(RewardedUnitId, this);
    }


    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string unitid)
    {
        Debug.Log("Ad Loaded: " + unitid);

        if (RewardedUnitId.Equals(unitid))
        {
            _showAdButton.interactable = true;
        }

        if (InterstitialUnitId.Equals(unitid))
        {
         // Invoke("OpenInterstitial",5);
        }
    }

    public void OpenInterstitial()
    {
        if (FactoryEventServices.isPlay)
            ShowAdInterstitial();
        else
            Invoke("OpenInterstitial",7);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string id, UnityAdsShowCompletionState showCompletionState)
    {
        if (RewardedUnitId.Equals(id) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
           
            _showAdButton.interactable = true;
            PlayerPrefs.SetInt(FactoryEnum.Coin.ToString(), PlayerPrefs.GetInt(FactoryEnum.Coin.ToString()) + 500);
            FactoryEventServices.GameAction.BuyCoin?.Invoke();
        }

        if (InterstitialUnitId.Equals(id) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            FactoryEventServices.GameAction.PlayGame?.Invoke();
            FactoryEventServices.isPlay = true;
            Invoke("OpenInterstitial",7);
        }
        else if(InterstitialUnitId.Equals(id))
        {
            FactoryEventServices.GameAction.PlayGame?.Invoke();
            FactoryEventServices.isPlay = true;
            Invoke("OpenInterstitial",7);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
    }

    //---------------------------------------------------------------------------------------


    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

    void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked()
    {
    }

    void OnBannerShown()
    {
    }

    void OnBannerHidden()
    {
    }
}