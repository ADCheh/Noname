using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<MonsterTypeId,MonsterStaticData> _monsters;
        private Dictionary<string,LevelStaticData> _levels;

        public void Load()
        {
            _monsters = Resources
                .LoadAll<MonsterStaticData>("StaticData/Monsters")
                .ToDictionary(x => x.MonsterTypeId, x => x);
            
            _levels = Resources
                .LoadAll<LevelStaticData>("StaticData/Levels")
                .ToDictionary(x => x.LevelKey, x => x);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId)
        {
            return _monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;
        }

        public LevelStaticData ForLevel(string sceneKey)
        {
            return _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;
        }
    }
}