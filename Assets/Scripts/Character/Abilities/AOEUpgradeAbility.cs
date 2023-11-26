namespace Vampire
{
    public class AOEUpgradeAbility : FloatUpgradeAbility<UpgradeableAOE>
    {
        public override bool RequirementsMet()
        {
            bool baseRequirementsMet = base.RequirementsMet();
            bool aoeAbilitiesInUse = abilityManager.AOEUpgradeablesCount > 0;
            return baseRequirementsMet && aoeAbilitiesInUse;
        }
    }
}
