namespace Vampire
{
    public class CooldownUpgradeAbility : FloatUpgradeAbility<UpgradeableWeaponCooldown>
    {
        public override bool RequirementsMet()
        {
            bool baseRequirementsMet = base.RequirementsMet();
            bool cooldownAbilitiesInUse = abilityManager.WeaponCooldownUpgradeablesCount > 0;
            return baseRequirementsMet && cooldownAbilitiesInUse;
        }
    }
}
