namespace Vampire
{
    public class ProjectileCountUpgradeAbility : IntUpgradeAbility<UpgradeableProjectileCount>
    {
        public override bool RequirementsMet()
        {
            bool baseRequirementsMet = base.RequirementsMet();
            bool projectileAbilitiesInUse = abilityManager.ProjectileCountUpgradeablesCount > 0;
            return baseRequirementsMet && projectileAbilitiesInUse;
        }
    }
}
