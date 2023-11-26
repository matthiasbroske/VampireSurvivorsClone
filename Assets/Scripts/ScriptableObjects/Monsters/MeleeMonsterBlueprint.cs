using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "Melee Monster", menuName = "Blueprints/Monsters/Melee Monster", order = 1)]
    public class MeleeMonsterBlueprint : MonsterBlueprint
    {
        [Header("Melee Monster")]
        public LayerMask meleeLayer;
    }
}
