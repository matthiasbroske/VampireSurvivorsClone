using UnityEngine;

namespace Vampire
{
    [System.Serializable]
    public class Loot<T>
    {
        public T item;
        [Range(0,1f)]
        public float dropChance;
        [HideInInspector]
        public CoinType coinType = CoinType.Bronze1;
    }
}