using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;
        public Action Changed;

        public List<LootPieceData> UnpickedLoot = new List<LootPieceData>();

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }

        public void Add(int loot)
        {
            Collected += loot;
            Changed?.Invoke();
        }
    }
}