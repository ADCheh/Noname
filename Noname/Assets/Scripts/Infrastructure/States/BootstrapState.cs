using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Services.Input;

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

        private void EnterLoadLevel() => _stateMachine.Enter<LoadLevelState, string>("Main");

        private void RegisterServices()
        {
            _services.RegisterSingle<IInputService>(InputService());
            _services.RegisterSingle<IAssets>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>()));
        }

        private static IInputService InputService()
        {
            //With more input service types add here
            return new StandaloneInputService();
        }
    }
}