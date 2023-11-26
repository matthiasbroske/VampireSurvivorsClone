using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class ShotgunBossAbility : BossAbility
    {
        [Header("Shotgun Details")]
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected float damage;
        [SerializeField] protected float knockback;
        [SerializeField] protected float bulletSpeedMin;
        [SerializeField] protected float bulletSpeedMax;
        [SerializeField] protected float fireRate;
        [SerializeField] protected float bulletCount;
        [SerializeField] protected float spreadAngle;
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
                entityManager.Grid.UpdateClient(monster);
            }
        }

        protected void LaunchBullets()
        {
            Vector2 initialDir = (playerCharacter.transform.position - monster.transform.position).normalized;
            for (int i = 0; i < bulletCount; i++)
            {
                float theta = (spreadAngle * i) / bulletCount - spreadAngle/2;
                Vector2 direction = new Vector2(
                    initialDir.x * Mathf.Cos(Mathf.Deg2Rad * theta) - initialDir.y * Mathf.Sin(Mathf.Deg2Rad * theta),
                    initialDir.x * Mathf.Sin(Mathf.Deg2Rad * theta) + initialDir.y * Mathf.Cos(Mathf.Deg2Rad * theta)
                );
                Projectile projectile = entityManager.SpawnProjectile(projectileIndex, monster.CenterTransform.position, damage, knockback, Random.Range(bulletSpeedMin, bulletSpeedMax), targetLayer);
                projectile.Launch(direction);
            }
        }

        protected IEnumerator LaunchBulletsRoutine()
        {
            for (int i = 0; i < 2; i++)
            {
                LaunchBullets();
                yield return new WaitForSeconds(1/fireRate);
            }
        }

        public override IEnumerator Activate()
        {
            active = true;
            yield return StartCoroutine(LaunchBulletsRoutine());
        }

        public override float Score()
        {
            float distance = Vector2.Distance(monster.transform.position, playerCharacter.transform.position);
            float x = distance / 6;
            float u = 0.65f;
            float o = 0.25f;
            float exp = -Mathf.Pow(x-u, 2)/(2*o*o);
            float score = Mathf.Exp(exp);
            return score;
        }
    }
}
