using Data;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace UI.Elements
{
    public class LootCounter : MonoBehaviour, ISavedProgress
    {
        public TextMeshProUGUI Counter;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
            _worldData.LootData.Changed += UpdateCounter;
        }

        private void Start()
        {
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            Counter.text = $"{_worldData.LootData.Collected}";
        }

        public void LoadProgress(PlayerProgress progress)
        {
            Construct(progress.WorldData);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.LootData.Collected = _worldData.LootData.Collected;
        }
    }
}