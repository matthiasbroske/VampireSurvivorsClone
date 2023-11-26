using UnityEngine.Pool;
using UnityEngine;

namespace Vampire
{
    public class CoinPool : Pool
    {
        protected ObjectPool<Coin> pool;

        public override void Init(EntityManager entityManager, Character playerCharacter, GameObject prefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            base.Init(entityManager, playerCharacter, prefab, collectionCheck, defaultCapacity, maxSize);
            pool = new ObjectPool<Coin>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionCheck, defaultCapacity, maxSize);
        }

        public Coin Get()
        {
            return pool.Get();
        }

        public void Release(Coin coin)
        {
            pool.Release(coin);
        }

        protected Coin CreatePooledItem()
        {
            Coin coin = Instantiate(prefab, transform).GetComponent<Coin>();
            coin.Init(entityManager, playerCharacter);
            return coin;
        }

        protected void OnTakeFromPool(Coin coin)
        {
            coin.gameObject.SetActive(true);
        }

        protected void OnReturnedToPool(Coin coin)
        {
            coin.gameObject.SetActive(false);
        }

        protected void OnDestroyPooledItem(Coin coin)
        {
            Destroy(coin.gameObject);
        }
    }
}
