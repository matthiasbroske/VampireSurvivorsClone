using UnityEngine;

namespace Vampire
{
    public class DescriptionUtils
    {
        public static string GetUpgradeDescription(string upgradeName, float upgrade)
        {
            string plus = upgrade > 0 ? "+" : "";
            return upgradeName + ": " + plus + (upgrade*100).ToString() + "%\n";
        }

        public static string GetUpgradeDescription(string upgradeName, int upgrade)
        {
            string plus = upgrade > 0 ? "+" : "";
            return upgradeName + ": " + plus + upgrade.ToString() + "\n";
        }
    }
}
