using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;
        public Action Changed;

        public List<string> UnpickedLoot = new List<string>();

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }
    }
}