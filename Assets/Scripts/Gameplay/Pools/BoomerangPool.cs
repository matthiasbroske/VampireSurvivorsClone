using UnityEngine.Pool;
using UnityEngine;

namespace Vampire
{
    public class BoomerangPool : Pool
    {
        protected ObjectPool<Boomerang> pool;

        public override void Init(EntityManager entityManager, Character playerCharacter, GameObject prefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            base.Init(entityManager, playerCharacter, prefab, collectionCheck, defaultCapacity, maxSize);
            pool = new ObjectPool<Boomerang>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionCheck, defaultCapacity, maxSize);
        }

        public Boomerang Get()
        {
            return pool.Get();
        }

        public void Release(Boomerang boomerang)
        {
            pool.Release(boomerang);
        }

        protected Boomerang CreatePooledItem()
        {
            Boomerang boomerang = Instantiate(prefab, transform).GetComponent<Boomerang>();
            boomerang.Init(entityManager, playerCharacter);
            return boomerang;
        }

        protected void OnTakeFromPool(Boomerang boomerang)
        {
            boomerang.gameObject.SetActive(true);
        }

        protected void OnReturnedToPool(Boomerang boomerang)
        {
            boomerang.gameObject.SetActive(false);
        }

        protected void OnDestroyPooledItem(Boomerang boomerang)
        {
            Destroy(boomerang.gameObject);
        }
    }
}
