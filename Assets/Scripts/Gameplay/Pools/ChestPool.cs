using UnityEngine.Pool;
using UnityEngine;

namespace Vampire
{
    public class ChestPool : Pool
    {
        protected ObjectPool<Chest> pool;

        public override void Init(EntityManager entityManager, Character playerCharacter, GameObject prefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            base.Init(entityManager, playerCharacter, prefab, collectionCheck, defaultCapacity, maxSize);
            pool = new ObjectPool<Chest>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionCheck, defaultCapacity, maxSize);
        }

        public Chest Get()
        {
            return pool.Get();
        }

        public void Release(Chest chest)
        {
            pool.Release(chest);
        }

        protected Chest CreatePooledItem()
        {
            Chest chest = Instantiate(prefab, transform).GetComponent<Chest>();
            chest.Init(entityManager, playerCharacter, transform);
            return chest;
        }

        protected void OnTakeFromPool(Chest chest)
        {
            chest.gameObject.SetActive(true);
        }

        protected void OnReturnedToPool(Chest chest)
        {
            chest.gameObject.SetActive(false);
        }

        protected void OnDestroyPooledItem(Chest chest)
        {
            Destroy(chest.gameObject);
        }
    }
}
