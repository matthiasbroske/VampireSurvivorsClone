using UnityEngine;

namespace Vampire
{
    [System.Serializable]
    public class LootTable<T>
    {
        public Loot<T>[] lootTable;

        /// <summary>
        /// Use this for loot tables where the odds do not add up to 100% (i.e. loot is not always dropped).
        /// </summary>
        public bool TryDropLoot(out T loot)
        {
            if (TryDropLootObject(out Loot<T> lootObject))
            {
                loot = lootObject.item;
                return true;
            }
            else
            {
                loot = default(T);
                return false;
            }
        }

        /// <summary>
        /// Use this for loot tables where the odds do not add up to 100% (i.e. loot is not always dropped).
        /// </summary>
        public bool TryDropLootObject(out Loot<T> loot)
        {
            float rand = Random.Range(0f, 1.0f);
            float cumulative = 0;
            foreach (Loot<T> drop in lootTable)
            {
                cumulative += drop.dropChance;
                if (rand < cumulative)
                {
                    loot = drop;
                    return true;
                }
            }
            loot = null;
            return false;
        }

        /// <summary>
        /// Use this for loot tables where the odds add up to 100% (i.e. loot is always dropped).
        /// </summary>
        public T DropLoot()
        {
            return DropLootObject().item;
        }

        /// <summary>
        /// Use this for loot tables where the odds add up to 100% (i.e. loot is always dropped).
        /// </summary>
        public Loot<T> DropLootObject()
        {
            float rand = Random.Range(0f, 1.0f);
            float cumulative = 0;
            foreach (Loot<T> drop in lootTable)
            {
                cumulative += drop.dropChance;
                if (rand < cumulative)
                {
                    return drop;
                }
            }
            // Failsafe in case of floating point precision errors or user error setting up the loot table
            if (lootTable.Length > 0)
            {
                Debug.LogError("Failed to drop loot, ensure drop chances add up to 100%.");
                return lootTable[lootTable.Length-1];
            }
            Debug.LogError("Failed to drop loot, loot table is empty.");
            return null;
        }
    }
}