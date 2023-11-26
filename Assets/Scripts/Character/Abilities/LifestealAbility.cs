using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class LifestealAbility : Ability
    {
        [Header("Lifesteal Stats")]
        [SerializeField] protected UpgradeableRecovery recovery;
        [SerializeField] protected UpgradeableRecoveryChance recoveryChance;
        private float timeSinceLastHealed;

        protected override void Use()
        {
            base.Use();
            playerCharacter.OnDealDamage.AddListener(PlayerDealtDamage);
            gameObject.SetActive(true);
        }

        void PlayerDealtDamage(float damage)
        {
            if (Random.Range(0.0f, 1.0f) < recoveryChance.Value)
            {
                playerCharacter.GainHealth(recovery.Value);
            }
        }
    }
}
