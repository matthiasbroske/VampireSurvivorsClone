using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class AbilityManager : MonoBehaviour
    {
        private LevelBlueprint levelBlueprint;
        private Character playerCharacter;
        private WeightedAbilities newAbilities;
        private WeightedAbilities ownedAbilities;
        private FastList<IUpgradeableValue> registeredUpgradeableValues;
        public int DamageUpgradeablesCount { get; set; } = 0;
        public int KnockbackUpgradeablesCount { get; set; } = 0;
        public int WeaponCooldownUpgradeablesCount { get; set; } = 0;
        public int RecoveryCooldownUpgradeablesCount { get; set; } = 0;
        public int AOEUpgradeablesCount { get; set; } = 0;
        public int ProjectileSpeedUpgradeablesCount { get; set; } = 0;
        public int ProjectileCountUpgradeablesCount { get; set; } = 0;
        public int RecoveryUpgradeablesCount { get; set; } = 0;
        public int RecoveryChanceUpgradeablesCount { get; set; } = 0;
        public int BleedDamageUpgradeablesCount { get; set; } = 0;
        public int BleedRateUpgradeablesCount { get; set; } = 0;
        public int BleedDurationUpgradeablesCount { get; set; } = 0;
        public int MovementSpeedUpgradeablesCount { get; set; } = 0;
        public int ArmorUpgradeablesCount { get; set; } = 0;
        public int FireRateUpgradeablesCount { get; set; } = 0;
        public int DurationUpgradeablesCount { get; set; } = 0;
        public int RotationSpeedUpgradeablesCount { get; set; } = 0;

        public void Init(LevelBlueprint levelBlueprint, EntityManager entityManager, Character playerCharacter, AbilityManager abilityManager)
        {
            this.levelBlueprint = levelBlueprint;
            this.playerCharacter = playerCharacter;

            registeredUpgradeableValues = new FastList<IUpgradeableValue>();

            newAbilities = new WeightedAbilities();
            foreach (GameObject abilityPrefab in levelBlueprint.abilityPrefabs)
            {
                Ability ability = Instantiate(abilityPrefab, transform).GetComponent<Ability>();
                ability.Init(abilityManager, entityManager, playerCharacter);
                newAbilities.Add(ability);
            }

            ownedAbilities = new WeightedAbilities();
            foreach (GameObject abilityPrefab in playerCharacter.Blueprint.startingAbilities)
            {
                Ability ability = Instantiate(abilityPrefab, transform).GetComponent<Ability>();
                ability.Init(abilityManager, entityManager, playerCharacter);
                ability.Select();
                ownedAbilities.Add(ability);
            }
            
        }

        public void RegisterUpgradeableValue(IUpgradeableValue upgradeableValue, bool inUse = false)
        {
            upgradeableValue.Register(this);
            registeredUpgradeableValues.Add(upgradeableValue);
            if (inUse) upgradeableValue.RegisterInUse();
        }

        public void UpgradeValue<T, TValue>(TValue value) where T : IUpgradeableValue
        {
            UpgradeableValue<TValue>[] upgradeableValues = registeredUpgradeableValues.OfType<T>().ToArray() as UpgradeableValue<TValue>[];
            foreach (UpgradeableValue<TValue> upgradeableValue in upgradeableValues)
            {
                upgradeableValue.Upgrade(value);
            }
        }

        /// <summary>
        /// Select abilities.
        /// </summary>
        public List<Ability> SelectAbilities()
        {
            List<Ability> selectedAbilities = new List<Ability>();
            
            // Determine which abilities are currently available (have their requirements met)
            WeightedAbilities availableOwnedAbilities = ExtractAvailableAbilities(ownedAbilities);
            WeightedAbilities availableNewAbilities = ExtractAvailableAbilities(newAbilities);

            // Determine how many abilities will be selected in total (3 - 4)
            int selectedAbilitiesCount = 3 + (ResolveChance(FourthChance) ? 1 : 0);

            // Attempt to show the player up to 2 items they already own (so they can upgrade them)
            int ownedAbilitiesCount = availableOwnedAbilities.Count < 2 ? availableOwnedAbilities.Count : 2;
            for (int i = 0; i < ownedAbilitiesCount; i++)
            {
                if (ResolveChance(OwnedChance))
                    selectedAbilities.Add(PullAbility(availableOwnedAbilities));
            }

            // Select the remaining abilities from the pool of available abilities
            int availableAbilitiesCount = selectedAbilitiesCount - selectedAbilities.Count;
            if (availableAbilitiesCount > availableNewAbilities.Count) availableAbilitiesCount = availableNewAbilities.Count;
            for (int i = 0; i < availableAbilitiesCount; i++)
            {
                selectedAbilities.Add(PullAbility(availableNewAbilities));
            }

            // Fill any remaining unfilled spots with owned abilities if possible
            for (int i = selectedAbilities.Count; i < selectedAbilitiesCount && i - selectedAbilities.Count < availableOwnedAbilities.Count; i++)
            {
                selectedAbilities.Add(PullAbility(availableOwnedAbilities));
            }

            // Return any remaining available abilities that weren't selected back to the new abilities pool
            foreach (Ability ability in availableNewAbilities)
                newAbilities.Add(ability);
            foreach (Ability ability in availableOwnedAbilities)
                ownedAbilities.Add(ability);

            return selectedAbilities;   
        }
        
        public void ReturnAbilities(List<Ability> abilities)
        {
            foreach (Ability ability in abilities)
            {
                if (ability.Owned)
                {
                    ownedAbilities.Add(ability);
                }
                else
                {
                    newAbilities.Add(ability);
                }
            }
        }

        public void DestroyActiveAbilities()
        {
            foreach (Ability ability in ownedAbilities)
            {
                Destroy(ability.gameObject);
            }
        }

        public bool HasAvailableAbilities()
        {
            foreach (Ability ability in ownedAbilities)
            {
                if (ability.RequirementsMet())
                    return true;
            }

            foreach (Ability ability in newAbilities)
            {
                if (ability.RequirementsMet())
                    return true;
            }

            return false;
        }

        private WeightedAbilities ExtractAvailableAbilities(WeightedAbilities abilities)
        {
            WeightedAbilities availableAbilities = new WeightedAbilities();

            foreach (Ability ability in abilities)
            {
                if (ability.RequirementsMet())
                    availableAbilities.Add(ability);
            }

            foreach (Ability ability in availableAbilities)
            {
                abilities.Remove(ability);
            }

            return availableAbilities;
        }

        /// <summary>
        /// Pulls an ability from the list of given list abilities and its weight.
        /// </summary>
        private Ability PullAbility(WeightedAbilities abilities)
        {
            float rand = Random.Range(0f, abilities.Weight);
            float cumulative = 0;
            foreach (Ability ability in abilities)
            {
                cumulative += ability.DropWeight;
                if (rand < cumulative)
                {
                    abilities.Remove(ability);
                    return ability;
                }
            }
            Debug.LogError("Failed to pull ability!");
            return null;
        }

        /// <summary>
        /// Chance that an ability already owned by the player should appear
        /// (so that it can be upgraded)
        /// </summary>
        private float OwnedChance()
        {
            float x = playerCharacter.CurrentLevel % 2 == 0 ? 2 : 1;
            return 1 + 0.3f*x - 1/playerCharacter.Luck;
        }

        /// <summary>
        /// Chance that a fourth ability/upgrade option appears
        /// </summary>
        private float FourthChance()
        {
            return 1 - 1/playerCharacter.Luck;
        }

        /// <summary>
        /// Resolves a chance function.
        /// </summary>
        private bool ResolveChance(System.Func<float> chanceFunction)
        {
            return Random.Range(0.0f, 1.0f) < chanceFunction();
        }

        private class WeightedAbilities : IEnumerable<Ability>
        {
            private FastList<Ability> abilities;
            private float weight;
            public float Weight { get => weight; set => weight = value; }
            public int Count { get => abilities.Count; }

            public WeightedAbilities()
            {
                abilities = new FastList<Ability>();
                weight = 0;
            }

            public void Add(Ability ability)
            {
                abilities.Add(ability);
                weight += ability.DropWeight;
            }

            public void Remove(Ability ability)
            {
                weight -= ability.DropWeight;
                abilities.Remove(ability);
            }

            public IEnumerator<Ability> GetEnumerator()
            {
                foreach (Ability ability in abilities)
                {
                    yield return ability;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
