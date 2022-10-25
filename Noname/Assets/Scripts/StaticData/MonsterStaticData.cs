using UnityEngine;
using UnityEngine.AddressableAssets;

namespace StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;
        
        [Range(1,100)]
        public int Hp;

        public int MinLoot;
        public int MaxLoot;
        
        [Range(1f,30f)]
        public float Damage;

        [Range(0.1f,10f)]
        public float MoveSpeed;

        [Range(0.5f,1f)]
        public float EffectiveDistance = 0.666f;

        [Range(0.5f,1f)]
        public float Cleavage;
        
        public AssetReferenceGameObject PrefabReference;
    }
}