using Infrastructure.AssetManagement;
using Infrastructure.Services;
using Infrastructure.Services.StaticData;
using StaticData.Windows;
using UI.Services.Windows;
using UnityEngine;

namespace UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";
        
        private readonly IStaticDataService _staticData;
        private readonly IAssets _assets;
        
        private Transform _uiRoot;

        public UIFactory(IAssets assets, IStaticDataService staticData)
        {
            _assets = assets;
            _staticData = staticData;
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            Object.Instantiate(config.prefab, _uiRoot);
        }

        public void CreateUIRoot()
        {
            _uiRoot = _assets.Instantiate(UIRootPath).transform;
        }
    }
}