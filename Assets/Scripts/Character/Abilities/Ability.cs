using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public abstract class Ability : MonoBehaviour
    {
        [Header("Ability Details")]
        [SerializeField] protected Sprite image;
        [SerializeField] protected new string name;
        [TextArea]
        [SerializeField] protected string description;
        [SerializeField] protected Rarity rarity = Rarity.Common;

        protected AbilityManager abilityManager;
        protected EntityManager entityManager;
        protected Character playerCharacter;
        protected List<IUpgradeableValue> upgradeableValues;
        protected int level = 0;
        protected int maxLevel;
        protected bool owned = false;
        public int Level { get => level; }
        public bool Owned { get => owned; }
        public Sprite Image { get => image; }
        public string Name { get => name; }
        public float DropWeight { get => (float)rarity; }
        public virtual string Description 
        { 
            get { 
                if (!owned)
                    return description;
                else
                    return GetUpgradeDescriptions();
            } 
        }

        public virtual void Init(AbilityManager abilityManager, EntityManager entityManager, Character playerCharacter)
        {
            this.abilityManager = abilityManager;
            this.entityManager = entityManager;
            this.playerCharacter = playerCharacter;
            // Register any upgradeable fields attached to this object
            upgradeableValues = this.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .Where(fi => typeof(IUpgradeableValue).IsAssignableFrom(fi.FieldType))
                .Select(fi => fi.GetValue(this) as IUpgradeableValue)
                .ToList();
            upgradeableValues.ForEach(x => abilityManager.RegisterUpgradeableValue(x));
            if (upgradeableValues.Count > 0)
                maxLevel = upgradeableValues.Max(x => x.UpgradeCount) + 1;  // max level = total number upgrades + 1 for starting level
        }
        
        public virtual void Select()
        {
            if (!owned)
            {
                owned = true;
                Use();
            }
            else
            {
                Upgrade(); 
            }
            level++;
        }

        protected virtual void Use()
        {
            upgradeableValues.ForEach(x => x.RegisterInUse());
        }

        protected virtual void Upgrade()
        {
            upgradeableValues.ForEach(x => x.Upgrade());
        }

        public virtual bool RequirementsMet()
        {
            return level < maxLevel;
        }

        protected string GetUpgradeDescriptions()
        {
            string description = "";
            upgradeableValues.ForEach(x => description += x.GetUpgradeDescription());
            return description;
        }

        public enum Rarity
        {
            Common = 50,
            Uncommon = 25,
            Rare = 15,
            Legendary = 9,
            Exotic = 1
        }
    }
}
