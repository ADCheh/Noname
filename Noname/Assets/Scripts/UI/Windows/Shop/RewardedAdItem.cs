using Infrastructure.Services.Ads;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class RewardedAdItem : MonoBehaviour
    {
        public Button ShowAdButton;
        public GameObject[] AdActiveObjects;
        public GameObject[] AdInactiveObjects;
        private IAdsService _adsService;
        private IPersistentProgressService _progressService;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            _adsService = adsService;
            _progressService = progressService;
        }
        
        public void Initialize()
        {
            ShowAdButton.onClick.AddListener(OnShowAddClick);

            _adsService.LoadRewardedVideo();
            
            RefreshAvailableAd();
        }

        public void Subscribe()
        {
            _adsService.RewardedVideoReady += RefreshAvailableAd;
        }

        public void Cleanup()
        {
            _adsService.RewardedVideoReady -= RefreshAvailableAd;
        }

        private void OnShowAddClick()
        {
            _adsService.ShowRewardedVideo(OnVideoFinished);
        }

        private void OnVideoFinished()
        {
            _progressService.Progress.WorldData.LootData.Add(_adsService.Reward);
        }

        private void RefreshAvailableAd()
        {

            bool videoReady = _adsService.IsRewardedVideoReady;

            foreach (GameObject adActiveObject in AdActiveObjects)
            {
                adActiveObject.SetActive(videoReady);
            }
            
            foreach (GameObject adInactiveObject in AdInactiveObjects)
            {
                adInactiveObject.SetActive(!videoReady);
            }
        }
    }
}