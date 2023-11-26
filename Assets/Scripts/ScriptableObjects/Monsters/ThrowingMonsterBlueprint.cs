using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "Throwing Monster", menuName = "Blueprints/Monsters/Throwing Monster", order = 1)]
    public class ThrowingMonsterBlueprint : MonsterBlueprint
    {
        [Header("Throwing Monster")]
        public GameObject throwablePrefab;
        public LayerMask targetLayer;
        public float range;
        public float timeAllowedOutsideRange;
    }
}
