namespace Vampire
{
    public class ProjectileSpeedAbilityUpgrade : FloatUpgradeAbility<UpgradeableProjectileSpeed>
    {
        public override bool RequirementsMet()
        {
            bool baseRequirementsMet = base.RequirementsMet();
            bool projectileAbilitiesInUse = abilityManager.ProjectileSpeedUpgradeablesCount > 0;
            return baseRequirementsMet && projectileAbilitiesInUse;
        }
    }
}
