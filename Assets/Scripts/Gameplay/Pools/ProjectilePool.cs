using UnityEngine.Pool;
using UnityEngine;

namespace Vampire
{
    public class ProjectilePool : Pool
    {
        protected ObjectPool<Projectile> pool;

        public override void Init(EntityManager entityManager, Character playerCharacter, GameObject prefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            base.Init(entityManager, playerCharacter, prefab, collectionCheck, defaultCapacity, maxSize);
            pool = new ObjectPool<Projectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionCheck, defaultCapacity, maxSize);
        }

        public Projectile Get()
        {
            return pool.Get();
        }

        public void Release(Projectile projectile)
        {
            pool.Release(projectile);
        }

        protected Projectile CreatePooledItem()
        {
            Projectile projectile = Instantiate(prefab, transform).GetComponent<Projectile>();
            projectile.Init(entityManager, playerCharacter);
            return projectile;
        }

        protected void OnTakeFromPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        protected void OnReturnedToPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        protected void OnDestroyPooledItem(Projectile projectile)
        {
            Destroy(projectile.gameObject);
        }
    }
}
