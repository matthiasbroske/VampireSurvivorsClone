using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class ThrowableAbility : Ability
    {
        [Header("Throwable Stats")]
        [SerializeField] protected GameObject throwablePrefab;
        [SerializeField] protected LayerMask monsterLayer;
        [SerializeField] protected float throwRadius;
        [SerializeField] protected UpgradeableDamageRate throwRate;
        [SerializeField] protected UpgradeableDamage damage;
        [SerializeField] protected UpgradeableKnockback knockback;
        [SerializeField] protected UpgradeableWeaponCooldown cooldown;
        [SerializeField] protected UpgradeableProjectileCount throwableCount;
        protected float timeSinceLastAttack;
        protected int throwableIndex;

        protected override void Use()
        {
            base.Use();
            gameObject.SetActive(true);
            timeSinceLastAttack = cooldown.Value;
            throwableIndex = entityManager.AddPoolForThrowable(throwablePrefab);
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= cooldown.Value)
            {
                timeSinceLastAttack = Mathf.Repeat(timeSinceLastAttack, cooldown.Value);
                StartCoroutine(Attack());
            }
        }

        protected virtual IEnumerator Attack()
        {
            timeSinceLastAttack -= throwableCount.Value/throwRate.Value;
            for (int i = 0; i < throwableCount.Value; i++)
            {
                LaunchThrowable();
                yield return new WaitForSeconds(1/throwRate.Value);
            }
        }

        protected virtual void LaunchThrowable()
        {
            Throwable throwable = entityManager.SpawnThrowable(throwableIndex, playerCharacter.CenterTransform.position, damage.Value, knockback.Value, 0, monsterLayer);
            throwable.Throw((Vector2)playerCharacter.transform.position + Random.insideUnitCircle * throwRadius);
            throwable.OnHitDamageable.AddListener(playerCharacter.OnDealDamage.Invoke);
        }
    }
}
