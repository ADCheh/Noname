using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.Ads;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticData;
using Services.Input;
using StaticData;
using UI.Services.Factory;
using UI.Services.Windows;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Initial, EnterLoadLevel); 
        }

        public void Exit()
        {
            
        }

        private void EnterLoadLevel() => _stateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            RegisterStaticData();
            RegisterAdsService();

            IRandomService randomService = new RandomService();
            _services.RegisterSingle<IRandomService>(randomService);

            _services.RegisterSingle<IInputService>(InputService());
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            RegisterAssetProvider();
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            
            RegisterIAPService(new IAPProvider(), _services.Single<IPersistentProgressService>());

            _services.RegisterSingle<IUIFactory>(new UIFactory(
                _services.Single<IAssets>(), 
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IAdsService>(),
                _services.Single<IIAPService>()
            ));
            
            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
            
            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAssets>(),
                _services.Single<IStaticDataService>(),randomService,
                _services.Single<IPersistentProgressService>(),
                _services.Single<IWindowService>()
                ));
            
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
        }

        private void RegisterAssetProvider()
        {
            AssetProvider assetProvider = new AssetProvider();
            assetProvider.Initialize();
            _services.RegisterSingle<IAssets>(assetProvider);
        }

        private void RegisterAdsService()
        {
            AdsService adsService = new AdsService();
            adsService.Initialize();
            _services.RegisterSingle<IAdsService>(adsService);
        }
        private void RegisterIAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
        {
            IAPService iapService = new IAPService(iapProvider, progressService);
            iapService.Initialize();
            _services.RegisterSingle<IIAPService>(iapService);
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle(staticData);
        }

        private static IInputService InputService()
        {
            //With more input service types add here
            return new StandaloneInputService();
        }
    }
}