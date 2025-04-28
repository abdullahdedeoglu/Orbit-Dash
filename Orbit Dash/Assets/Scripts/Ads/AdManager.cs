using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string androidGameId = "5843486";
    private string iosGameId = "5843487";
    private string gameId;

    private string interstitialPlacementId = "Interstitial_Android";
    private string rewardedPlacementId = "Rewarded_Android";

    private bool testMode = true;

    private void Awake()
    {
#if UNITY_ANDROID
        gameId = androidGameId;
#elif UNITY_IOS
        gameId = iosGameId;
#endif
        Advertisement.Initialize(gameId, testMode, this);
    }

    // Load Ads
    public void LoadInterstitial() => Advertisement.Load(interstitialPlacementId, this);
    public void LoadRewarded() => Advertisement.Load(rewardedPlacementId, this);

    //// Show Ads
    //public void ShowInterstitial() => Advertisement.Show(interstitialPlacementId, this);
    //public void ShowRewarded() => Advertisement.Show(rewardedPlacementId, this);

    // Show Interstitial
    public void ShowInterstitial()
    {
        Debug.Log("Interstitial Ad requested.");
        Advertisement.Show(interstitialPlacementId, this);
    }

    // Show Rewarded
    public void ShowRewarded()
    {
        Debug.Log("Rewarded Ad requested.");
        Advertisement.Show(rewardedPlacementId, this);
    }

    // Initialization callbacks
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialization Complete.");
        LoadInterstitial();
        LoadRewarded();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Initialization Failed: {error} - {message}");
    }

    // Load callbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Ad Failed to Load: {placementId} - {error} - {message}");
    }

    // Show callbacks
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(rewardedPlacementId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Rewarded Ad Completed - Continue game.");
            GameManager.Instance.ContinueAfterAd();
        }

        if (placementId.Equals(interstitialPlacementId))
        {
            Debug.Log("Interstitial Ad Closed.");
            GameManager.Instance.OnInterstitialAdClosed();
        }
    }



    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Ad Show Failed: {placementId} - {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
}
