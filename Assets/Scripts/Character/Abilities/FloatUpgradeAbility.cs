
using UnityEngine;

namespace Vampire
{
    public class FloatUpgradeAbility<T> : Ability where T : UpgradeableFloat
    {
        [SerializeField] protected float[] upgrades;
        public override string Description
        {
            get {
                return GetUpgradeDescription();
            }
        }

        protected override void Use()
        {
            base.Use();
            gameObject.SetActive(true);
            abilityManager.UpgradeValue<T, float>(upgrades[level]);
        }

        protected override void Upgrade()
        {
            abilityManager.UpgradeValue<T, float>(upgrades[level]);
        }

        public override bool RequirementsMet()
        {
            return level < upgrades.Length;
        }

        protected string GetUpgradeDescription()
        {
            return DescriptionUtils.GetUpgradeDescription(this.description, upgrades[level]);
        }
    }
}