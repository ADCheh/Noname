using Data;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;

namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }
        public void Exit()
        {
            
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadSceneState,string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress(); 
        }

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress("Main");

            progress.HeroState.MaxHP = 50;
            progress.Stats.Damage = 1f;
            progress.Stats.DamageRadius = 0.5f;
            progress.HeroState.ResetHP();
            
            return progress;
        }
    }
}