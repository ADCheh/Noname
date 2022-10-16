using System.Collections.Generic;
using System.Threading.Tasks;
using Enemy;
using Infrastructure.AssetManagement;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Infrastructure.Services.StaticData;
using Logic;
using Logic.EnemySpawners;
using StaticData;
using UI.Elements;
using UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _random;
        private readonly IPersistentProgressService _progressService;
        private readonly IWindowService _windowService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject HeroGameObject { get; set; }

        public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService random, IPersistentProgressService progressService, IWindowService windowService)
        {
            _assets = assets;
            _staticData = staticData;
            _random = random;
            _progressService = progressService;
            _windowService = windowService;
        }

        public async Task WarmUp()
        {
            await _assets.Load<GameObject>(AssetAddress.Loot);
            await _assets.Load<GameObject>(AssetAddress.Spawner);
        }
        
        public GameObject CreateHero(Vector3 at)
        {
            HeroGameObject = InstantiateRegistered(AssetAddress.HeroPath, at);
            return HeroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetAddress.HudPath);
            hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                openWindowButton.Construct(_windowService);
            }
            
            return hud;
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parentTransform)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);

            GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
            GameObject monster = Object.Instantiate(prefab, parentTransform.position, Quaternion.identity);

            IHealth health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max = monsterData.Hp;
            
            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<AgentMoveToHero>().Construct(HeroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(monsterData.MinLoot,monsterData.MaxLoot);
            lootSpawner.Consturct(this, _random);

            Attack attack = monster.GetComponent<Attack>();
            attack.Construct(HeroGameObject.transform);
            attack.Damage = monsterData.Damage;
            attack.Cleavage = monsterData.Cleavage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;
            
            monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);
            
            return monster;
        }

        public async Task<LootPiece> CreateLoot()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
            LootPiece lootPiece = InstantiateRegistered(prefab).GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);
            return lootPiece;
        }

        public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
            SpawnPoint spawner = InstantiateRegistered(prefab, at).GetComponent<SpawnPoint>();
            
            spawner.Construct(this);
            spawner.Id = spawnerId;
            spawner.MonsterTypeId = monsterTypeId;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            
            _assets.CleanUp();
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if(progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);
            
            ProgressReaders.Add(progressReader);
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
    }
}