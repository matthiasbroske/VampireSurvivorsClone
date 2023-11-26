using UnityEngine.Pool;
using UnityEngine;

namespace Vampire
{
    public class ThrowablePool : Pool
    {
        protected ObjectPool<Throwable> pool;

        public override void Init(EntityManager entityManager, Character playerCharacter, GameObject prefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            base.Init(entityManager, playerCharacter, prefab, collectionCheck, defaultCapacity, maxSize);
            pool = new ObjectPool<Throwable>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionCheck, defaultCapacity, maxSize);
        }

        public Throwable Get()
        {
            return pool.Get();
        }

        public void Release(Throwable throwable)
        {
            pool.Release(throwable);
        }

        protected Throwable CreatePooledItem()
        {
            Throwable throwable = Instantiate(prefab, transform).GetComponent<Throwable>();
            throwable.Init(entityManager, playerCharacter);
            return throwable;
        }

        protected void OnTakeFromPool(Throwable throwable)
        {
            throwable.gameObject.SetActive(true);
        }

        protected void OnReturnedToPool(Throwable throwable)
        {
            throwable.gameObject.SetActive(false);
        }

        protected void OnDestroyPooledItem(Throwable throwable)
        {
            Destroy(throwable.gameObject);
        }
    }
}
