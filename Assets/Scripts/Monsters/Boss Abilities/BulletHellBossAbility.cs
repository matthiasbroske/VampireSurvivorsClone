using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class BulletHellBossAbility : BossAbility
    {
        [Header("Bullet Hell Details")]
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected float damage;
        [SerializeField] protected float knockback;
        [SerializeField] protected float bulletSpeed;
        [SerializeField] protected float fireRate;
        [SerializeField] protected float bulletCount;
        protected float timeSinceLastAttack;
        protected int projectileIndex = -1;

        public override void Init(BossMonster monster, EntityManager entityManager, Character playerCharacter)
        {
            base.Init(monster, entityManager, playerCharacter);
            projectileIndex = entityManager.AddPoolForProjectile(bulletPrefab);
        }

        // void Update()
        // {
        //     if (active)
        //     {
        //         timeSinceLastAttack += Time.deltaTime;
        //         if (timeSinceLastAttack >= 1/fireRate)
        //         {
        //             timeSinceLastAttack = Mathf.Repeat(timeSinceLastAttack, 1/fireRate);
        //             LaunchBullets();
        //         }
        //     }
        // }

        void FixedUpdate()
        {
            if (active)
            {
                Vector2 moveDirection = (playerCharacter.transform.position - monster.transform.position).normalized;
                monster.Move(moveDirection, Time.fixedDeltaTime);
            }
        }

        protected void LaunchBullets()
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float theta = i * Mathf.PI * 2.0f / bulletCount;
                Vector2 direction = new Vector2(Mathf.Sin(theta), Mathf.Cos(theta));
                Projectile projectile = entityManager.SpawnProjectile(projectileIndex, monster.CenterTransform.position, damage, knockback, bulletSpeed, targetLayer);
                projectile.Launch(direction);
            }
        }

        protected IEnumerator LaunchBulletsRoutine()
        {
            LaunchBullets();
            yield return new WaitForSeconds(1/fireRate);
        }

        public override IEnumerator Activate()
        {
            active = true;
            yield return StartCoroutine(LaunchBulletsRoutine());
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override float Score()
        {
            return 0.5f;
        }
    }
}
