using System.Collections.Generic;
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
        
        GameObject CreateMonster(MonsterTypeId typeId, Transform parentTransform);
        GameObject CreateHero(GameObject at);
        GameObject CreateHud();
        LootPiece CreateLoot();
        void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        void Cleanup();
    }
}