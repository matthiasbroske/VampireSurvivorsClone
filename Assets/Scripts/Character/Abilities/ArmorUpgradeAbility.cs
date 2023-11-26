namespace Vampire
{
    public class ArmorUpgradeAbility : IntUpgradeAbility<UpgradeableArmor>
    {
        public override bool RequirementsMet()
        {
            bool baseRequirementsMet = base.RequirementsMet();
            bool armorInUse = abilityManager.ArmorUpgradeablesCount > 0;
            return baseRequirementsMet && armorInUse;
        }
    }
}