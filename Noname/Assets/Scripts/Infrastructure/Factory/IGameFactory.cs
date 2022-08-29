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
        GameObject CreateHero(GameObject at);
        GameObject CreateHud();
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void Cleanup();
        void Register(ISavedProgressReader progressReader);
        GameObject CreateMonster(MonsterTypeId typeId, Transform parentTransform);
        LootPiece CreateLoot();
    }
}