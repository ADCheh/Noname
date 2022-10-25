using System.Threading.Tasks;
using CameraLogic;
using Data;
using Enemy;
using Hero;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.StaticData;
using Logic;
using StaticData;
using UI;
using UI.Elements;
using UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class LoadSceneState : IPayloadedState<string>
    {

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;

        public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain,
            IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();
            
            InformProgressReaders();


            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUIRoot()
        {
            await _uiFactory.CreateUIRoot();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private async Task InitGameWorld()
        {
            var levelData = LevelStaticData();

            await InitSpawners(levelData);
            await InitDroppedLoot();
            
            GameObject hero = await InitHero(levelData);

            //
            await InitHud(hero);
            //


            CameraFollow(hero);
        }

        private async Task InitDroppedLoot()
        {
            foreach (LootPieceData unpickedLoot in _progressService.Progress.WorldData.LootData.UnpickedLoot)
            {
                LootPiece lootPiece = await _gameFactory.CreateLoot();
                lootPiece.GetComponent<UniqueId>().Id = unpickedLoot.Id;
                lootPiece.transform.position = unpickedLoot.LootPosition.AsUnityVector();
                lootPiece.Initialize(unpickedLoot.Loot);
            }
        }

        private async Task InitSpawners(LevelStaticData levelData)
        {
            
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private async Task InitHud(GameObject hero)
        {
            GameObject hud = await _gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
        }

        private async Task<GameObject> InitHero(LevelStaticData levelData)
        {
            return await _gameFactory.CreateHero(levelData.InitialHeroPosition);
        }

        private LevelStaticData LevelStaticData()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticData.ForLevel(sceneKey);
            return levelData;
        }

        private void CameraFollow(GameObject hero)
        {
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
        }
    }
}