using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class BoomerangAbility : Ability
    {
        [Header("Boomerang Stats")]
        [SerializeField] protected GameObject boomerangPrefab;
        [SerializeField] protected LayerMask monsterLayer;
        [SerializeField] protected float throwRadius;
        [SerializeField] protected float throwTime = 1;
        [SerializeField] protected UpgradeableDamageRate throwRate;
        [SerializeField] protected UpgradeableDamage damage;
        [SerializeField] protected UpgradeableKnockback knockback;
        [SerializeField] protected UpgradeableWeaponCooldown cooldown;
        [SerializeField] protected UpgradeableProjectileCount boomerangCount;
        protected float timeSinceLastAttack;
        protected int boomerangIndex;

        protected override void Use()
        {
            base.Use();
            gameObject.SetActive(true);
            timeSinceLastAttack = cooldown.Value;
            boomerangIndex = entityManager.AddPoolForBoomerang(boomerangPrefab);
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
            timeSinceLastAttack -= boomerangCount.Value/throwRate.Value;
            for (int i = 0; i < boomerangCount.Value; i++)
            {
                ThrowBoomerang();
                yield return new WaitForSeconds(1/throwRate.Value);
            }
        }

        protected virtual void ThrowBoomerang()
        {
            Boomerang boomerang = entityManager.SpawnBoomerang(boomerangIndex, playerCharacter.CenterTransform.position, damage.Value, knockback.Value, throwRadius, throwTime, monsterLayer);
            Vector2 throwPosition;
            // Throw randomly at nearby enemies
            List<ISpatialHashGridClient> nearbyEnemies = entityManager.Grid.FindNearbyInRadius(playerCharacter.transform.position, throwRadius);
            if (nearbyEnemies.Count > 0)
                throwPosition = nearbyEnemies[Random.Range(0, nearbyEnemies.Count)].Position;
            else
                throwPosition = (Vector2)playerCharacter.transform.position + Random.insideUnitCircle.normalized * throwRadius;
            boomerang.Throw(playerCharacter.transform, throwPosition);
            boomerang.OnHitDamageable.AddListener(playerCharacter.OnDealDamage.Invoke);
        }
    }
}
