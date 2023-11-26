using UnityEngine.Pool;
using UnityEngine;

namespace Vampire
{
    public class GravityProjectilePool : Pool
    {
        protected ObjectPool<GravityProjectile> pool;

        public override void Init(EntityManager entityManager, Character playerCharacter, GameObject prefab, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            base.Init(entityManager, playerCharacter, prefab, collectionCheck, defaultCapacity, maxSize);
            pool = new ObjectPool<GravityProjectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionCheck, defaultCapacity, maxSize);
        }

        public GravityProjectile Get()
        {
            return pool.Get();
        }

        public void Release(GravityProjectile projectile)
        {
            pool.Release(projectile);
        }

        protected GravityProjectile CreatePooledItem()
        {
            GravityProjectile projectile = Instantiate(prefab, transform).GetComponent<GravityProjectile>();
            projectile.Init(entityManager, playerCharacter);
            return projectile;
        }

        protected void OnTakeFromPool(GravityProjectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        protected void OnReturnedToPool(GravityProjectile projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        protected void OnDestroyPooledItem(GravityProjectile projectile)
        {
            Destroy(projectile.gameObject);
        }
    }
}
