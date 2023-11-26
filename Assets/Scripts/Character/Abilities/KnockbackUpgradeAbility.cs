namespace Vampire
{
    public class KnockbackUpgradeAbility : FloatUpgradeAbility<UpgradeableKnockback>
    {
        public override bool RequirementsMet()
        {
            bool baseRequirementsMet = base.RequirementsMet();
            bool aoeAbilitiesInUse = abilityManager.KnockbackUpgradeablesCount > 0;
            return baseRequirementsMet && aoeAbilitiesInUse;
        }
    }
}
