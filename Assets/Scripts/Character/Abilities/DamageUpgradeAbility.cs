namespace Vampire
{
    public class DamageUpgradeAbility : FloatUpgradeAbility<UpgradeableDamage>
    {
        public override bool RequirementsMet()
        {
            bool baseRequirementsMet = base.RequirementsMet();
            bool damageAbilitiesInUse = abilityManager.DamageUpgradeablesCount > 0;
            return baseRequirementsMet && damageAbilitiesInUse;
        }
    }
}
