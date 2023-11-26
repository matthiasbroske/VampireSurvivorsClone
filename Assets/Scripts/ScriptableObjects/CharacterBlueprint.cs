using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "Character", menuName = "Blueprints/Character", order = 1)]
    public class CharacterBlueprint : ScriptableObject
    {
        public new string name;  // 名字
        public bool owned = false;
        public int cost = 999;
        public float hp;  // 血量
        public float atk;  // 攻擊力
        public float recovery;  // 血量恢復再生率
        public int armor;  // 裝甲減傷
        public float movespeed;  // 移動速度
        public float atkspeed;  // 攻擊速度
        public float luck;  // 運氣
        public float acceleration;
        public Sprite[] walkSpriteSequence;
        public float walkFrameTime;
        public GameObject[] startingAbilities;

        public float LevelToExpIncrease(int level)
        {
            if (level < 10)
                return 10;
            if (level < 20)
                return 13;
            if (level < 30)
                return 16;
            else
                return 20;
        }
    }
}
