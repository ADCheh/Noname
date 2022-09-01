using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsShowListener, IUnityAdsLoadListener
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
            Advertisement.Load(RewardedVideoPlacementId, this);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(RewardedVideoPlacementId);
            _onVideoFinished = onVideoFinished;
        }

        public bool IsRewardedVideoReady => Advertisement.isInitialized;
        
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
    }
}