using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.Services.PersistentProgress;
using Logic;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        public GameObject Skull;
        public GameObject PickupFxPrefab;
        public TextMeshPro LootText;
        public GameObject PickupPopup;
        
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }
        
        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other)
        {
            PickUp();
        }

        private void PickUp()
        {
            if(_picked)
                return;
            
            _picked = true;

            UpdateWorldData();
            HideSkull();
            PlayPickupFx();
            ShowText();
            
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData()
        {
            _worldData.LootData.Collect(_loot);
            RemoveLootPieceFromData();
        }

        private void RemoveLootPieceFromData()
        {
            List<LootPieceData> lootOnScene = _worldData.LootData.UnpickedLoot;

            if (lootOnScene.Any(x => x.Id == GetLootUniqueId()))
            {
                LootPieceData piece = lootOnScene.FirstOrDefault(x => x.Id == GetLootUniqueId());
                lootOnScene.Remove(piece);
            }
                
        }

        private void HideSkull()
        {
            Skull.SetActive(false);
        }

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(1.5f);
            
            Destroy(gameObject);
        }

        private void PlayPickupFx()
        {
            Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);
        }

        private void ShowText()
        {
            LootText.text = $"{_loot.Value}";
            PickupPopup.SetActive(true);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_picked)
                return;

            List<LootPieceData> lootOnScene = progress.WorldData.LootData.UnpickedLoot;
            
            if(!lootOnScene.Any(x=>x.Id == GetLootUniqueId()))
                lootOnScene.Add( new LootPieceData(GetLootUniqueId(),_loot, transform.position.AsVectorData()));
                
        }

        private string GetLootUniqueId()
        {
            return gameObject.GetComponent<UniqueId>().Id;
        }

        public void LoadProgress(PlayerProgress progress)
        {
        }
    }
}