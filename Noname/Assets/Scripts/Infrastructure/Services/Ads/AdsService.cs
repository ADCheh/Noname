using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsShowListener, IUnityAdsLoadListener, IUnityAdsInitializationListener
    {
        private const string AndroidGameId = "4910485";
        private const string IOSGameId = "4910484";

        private const string RewardedVideoPlacementId = "rewardedVideo";

        private string _gameId;
        private Action _onVideoFinished;

        public event Action RewardedVideoReady;

        public int Reward => 13;

        public void Initialize()
        {
#if UNITY_IOS
        _gameId = IOSGameId;
#elif UNITY_ANDROID
_gameId = AndroidGameId;
#else
            _gameId = IOSGameId;
#endif
            
            Advertisement.Initialize(_gameId, true, this);
            Advertisement.Load(RewardedVideoPlacementId, this);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(RewardedVideoPlacementId, this);
            _onVideoFinished = onVideoFinished;
        }

        public bool IsRewardedVideoReady => true/*Advertisement.isInitialized*/;
        
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"OnUnityAdsShowFailure {placementId} : {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"OnUnityAdsShowStart {placementId}");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"OnUnityAdsShowClick {placementId}");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    Debug.LogError($"OnUnityAdsShowComplete {placementId} : {showCompletionState}");
                    break;
                case UnityAdsShowCompletionState.COMPLETED:
                    _onVideoFinished?.Invoke();
                    break;
                case UnityAdsShowCompletionState.UNKNOWN:
                    Debug.LogError($"OnUnityAdsShowComplete {placementId} : {showCompletionState}");
                    break;
                default:
                    Debug.LogError($"OnUnityAdsShowComplete {placementId} : {showCompletionState}");
                    break;
            }

            _onVideoFinished = null;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"OnUnityAdsAdLoaded {placementId}");
            
            if(placementId == RewardedVideoPlacementId)
                RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"OnUnityAdsFailedToLoad {placementId} : {message}");
        }

        public void OnInitializationComplete()
        {
            Debug.Log("OnInitializationComplete");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"OnInitializationFailed : {message}");
        }
    }
}