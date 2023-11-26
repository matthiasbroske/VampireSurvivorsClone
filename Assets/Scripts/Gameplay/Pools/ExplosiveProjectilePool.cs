using UnityEngine.Pool;
using UnityEngine;

namespace Vampire
{
    public class ExplosiveProjectilePool : Pool
    {
        protected ObjectPool<ExplosiveProjectile> pool;

        public override void Init(EntityManager entityManager, Character playerCharacter, GameObject prefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            base.Init(entityManager, playerCharacter, prefab, collectionCheck, defaultCapacity, maxSize);
            pool = new ObjectPool<ExplosiveProjectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionCheck, defaultCapacity, maxSize);
        }

        public ExplosiveProjectile Get()
        {
            return pool.Get();
        }

        public void Release(ExplosiveProjectile projectile)
        {
            pool.Release(projectile);
        }

        protected ExplosiveProjectile CreatePooledItem()
        {
            ExplosiveProjectile projectile = Instantiate(prefab, transform).GetComponent<ExplosiveProjectile>();
            projectile.Init(entityManager, playerCharacter);
            return projectile;
        }

        protected void OnTakeFromPool(ExplosiveProjectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        protected void OnReturnedToPool(ExplosiveProjectile projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        protected void OnDestroyPooledItem(ExplosiveProjectile projectile)
        {
            Destroy(projectile.gameObject);
        }
    }
}
