using UnityEngine;

namespace Vampire
{
    public class MonsterBlueprint : ScriptableObject
    {
        [Header("Stats")]
        public new string name;  // 名字
        public float hp;  // 血量
        public float atk;  // 攻擊力
        public float recovery;  // 血量恢復再生率
        public float armor;  // 裝甲減傷
        public float atkspeed;  // 攻擊速度
        public float movespeed;  // 移動速度
        public float acceleration;
        [Header("Drops")]
        public LootTable<GemType> gemLootTable;
        public LootTable<CoinType> coinLootTable;
        [Header("Animation")]
        public Sprite[] walkSpriteSequence;
        public float walkFrameTime;
    }
}
