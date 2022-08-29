using Data;
using Enemy;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UnityEngine;

namespace Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        public string Id { get; set; }
        public bool Slain => _slain;
        [SerializeField]private bool _slain;
        private IGameFactory _gameFactory;
        private EnemyDeath _enemyDeath;

        public void Construct(IGameFactory factory)
        {
            _gameFactory = factory;
        }
        
        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                _slain = true;
            else
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            GameObject monster = _gameFactory.CreateMonster(MonsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if(_enemyDeath != null)
                _enemyDeath.Happened -= Slay;
            _slain = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if(_slain)
                progress.KillData.ClearedSpawners.Add(Id);
        }
    }
}
