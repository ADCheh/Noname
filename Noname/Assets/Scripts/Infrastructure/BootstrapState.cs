using System;
using Services.Input;

namespace Infrastructure
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegisterServices();
            _sceneLoader.Load(Initial, EnterLoadLevel); 
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState>();
        }

        private void RegisterServices()
        {
            Game.InputService = RegisterInputService();
        }

        public void Exit()
        {
            
        }
        
        private static IInputService RegisterInputService()
        {
            //With more input service types add here
            return new StandaloneInputService();
        }
    }
}