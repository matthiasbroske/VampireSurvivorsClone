using UnityEngine.Pool;
using UnityEngine;

namespace Vampire
{
    public class DamageTextPool : Pool
    {
        protected ObjectPool<DamageText> pool;

        public override void Init(EntityManager entityManager, Character playerCharacter, GameObject prefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            base.Init(entityManager, playerCharacter, prefab, collectionCheck, defaultCapacity, maxSize);
            pool = new ObjectPool<DamageText>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionCheck, defaultCapacity, maxSize);
        }

        public DamageText Get()
        {
            return pool.Get();
        }

        public void Release(DamageText text)
        {
            pool.Release(text);
        }

        protected DamageText CreatePooledItem()
        {
            DamageText text = Instantiate(prefab, transform).GetComponent<DamageText>();
            text.Init(entityManager);
            return text;
        }

        protected void OnTakeFromPool(DamageText text)
        {
            text.gameObject.SetActive(true);
        }

        protected void OnReturnedToPool(DamageText text)
        {
            text.gameObject.SetActive(false);
        }

        protected void OnDestroyPooledItem(DamageText text)
        {
            Destroy(text.gameObject);
        }
    }
}
