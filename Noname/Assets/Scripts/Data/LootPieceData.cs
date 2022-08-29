using System;

namespace Data
{
    [Serializable]
    public class LootPieceData
    {
        public string Id;
        public Loot Loot;
        public Vector3Data LootPosition;

        public LootPieceData(string id,Loot loot, Vector3Data lootPosition)
        {
            Id = id;
            Loot = loot;
            LootPosition = lootPosition;
        }
    }
}