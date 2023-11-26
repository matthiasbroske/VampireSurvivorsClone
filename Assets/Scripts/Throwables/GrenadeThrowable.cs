using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Vampire
{
    public class GrenadeThrowable : Throwable
    {
        [SerializeField] GameObject fragmentPrefab;
        [SerializeField] protected float fragmentSpeed;
        [SerializeField] protected int fragmentCount;
        protected int projectileIndex = -1;

        public override void Init(EntityManager entityManager, Character playerCharacter)
        {
            base.Init(entityManager, playerCharacter);
            projectileIndex = entityManager.AddPoolForProjectile(fragmentPrefab);
        }

        public void SetupGrenade(int fragmentCount)
        {
            this.fragmentCount = fragmentCount;
        }

        protected override void Explode()
        {
            for (int i = 0; i < fragmentCount; i++)
            {
                float theta = i * Mathf.PI * 2.0f / fragmentCount;
                Vector2 direction = new Vector2(Mathf.Sin(theta), Mathf.Cos(theta));
                Projectile projectile = entityManager.SpawnProjectile(projectileIndex, transform.position, damage, knockback, fragmentSpeed, targetLayer);
                projectile.OnHitDamageable.AddListener(OnHitDamageable.Invoke);
                projectile.Launch(direction);
            }
            DestroyThrowable();
        }
    }
}
