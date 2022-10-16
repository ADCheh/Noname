using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Enemy;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Logic;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        
        Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parentTransform);
        GameObject CreateHero(Vector3 at);
        GameObject CreateHud();
        Task<LootPiece> CreateLoot();
        Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        void Cleanup();
        Task WarmUp();
    }
}