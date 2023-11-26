using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "Boomerang Monster", menuName = "Blueprints/Monsters/Boomerang Monster", order = 1)]
    public class BoomerangMonsterBlueprint : MonsterBlueprint
    {
        [Header("Boomerang Monster")]
        public GameObject boomerangPrefab;
        public LayerMask targetLayer;
        public float boomerangDamage;
        public float boomerangAttackSpeed;
        public float range;
        public float throwRange;
        public float throwTime;
    }
}
