using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.Services;
using Infrastructure.Services.Ads;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.StaticData;
using StaticData.Windows;
using UI.Services.Windows;
using UI.Windows;
using UI.Windows.Shop;
using UnityEngine;

namespace UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UIRoot";

        private readonly IAssets _assets;
        private readonly IAdsService _adsService;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IIAPService _iapService;

        private Transform _uiRoot;

        public UIFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService, IAdsService adsService, IIAPService iapService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
            _iapService = iapService;
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            ShopWindow window = Object.Instantiate(config.prefab, _uiRoot) as ShopWindow;
            window.Construct(_adsService,_progressService, _iapService, _assets);
        }

        public async Task CreateUIRoot()
        {
            GameObject root = await _assets.Instantiate(UIRootPath);
            _uiRoot = root.transform;
        }
    }
}