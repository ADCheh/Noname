using System;
using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Logic;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(GameObject at);
        GameObject CreateHud();
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject HeroGameObject { get; }

        event Action HeroCreated;
        void Cleanup();
        void Register(ISavedProgressReader progressReader);
    }
}