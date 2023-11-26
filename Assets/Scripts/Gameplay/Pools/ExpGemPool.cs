using UnityEngine.Pool;
using UnityEngine;

namespace Vampire
{
    public class ExpGemPool : Pool
    {
        protected ObjectPool<ExpGem> pool;

        public override void Init(EntityManager entityManager, Character playerCharacter, GameObject prefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            base.Init(entityManager, playerCharacter, prefab, collectionCheck, defaultCapacity, maxSize);
            pool = new ObjectPool<ExpGem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionCheck, defaultCapacity, maxSize);
        }

        public ExpGem Get()
        {
            return pool.Get();
        }

        public void Release(ExpGem expGem)
        {
            pool.Release(expGem);
        }

        protected ExpGem CreatePooledItem()
        {
            ExpGem expGem = Instantiate(prefab, transform).GetComponent<ExpGem>();
            expGem.Init(entityManager, playerCharacter);
            return expGem;
        }

        protected void OnTakeFromPool(ExpGem expGem)
        {
            expGem.gameObject.SetActive(true);
        }

        protected void OnReturnedToPool(ExpGem expGem)
        {
            expGem.gameObject.SetActive(false);
        }

        protected void OnDestroyPooledItem(ExpGem expGem)
        {
            Destroy(expGem.gameObject);
        }
    }
}
