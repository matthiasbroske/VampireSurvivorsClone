using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class RecoveryAbility : Ability
    {
        [Header("Recovery Stats")]
        [SerializeField] protected UpgradeableRecovery recovery;
        [SerializeField] protected UpgradeableRecoveryCooldown cooldown;
        private float timeSinceLastHealed;

        protected override void Use()
        {
            base.Use();
            gameObject.SetActive(true);
        }

        void Update()
        {
            timeSinceLastHealed += Time.deltaTime;
            if (timeSinceLastHealed >= cooldown.Value)
            {
                playerCharacter.GainHealth(recovery.Value);
                timeSinceLastHealed = Mathf.Repeat(timeSinceLastHealed, cooldown.Value);
            }
        }
    }
}
