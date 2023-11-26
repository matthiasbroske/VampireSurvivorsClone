using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "Boss Monster", menuName = "Blueprints/Monsters/Boss Monster", order = 1)]
    public class BossMonsterBlueprint : MonsterBlueprint
    {
        [Header("Boss Abilities")]
        public GameObject[] abilityPrefabs;
        [Header("Melee Attack Stats")]
        public LayerMask meleeLayer;
        public float meleeDamage;
        public float meleeKnockback;
        public float meleeAttackDelay;
        [Header("Chest")]
        public ChestBlueprint chestBlueprint;
    }
}
