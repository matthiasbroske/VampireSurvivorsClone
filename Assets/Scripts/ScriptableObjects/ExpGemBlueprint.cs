using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "Exp Gem", menuName = "Blueprints/Gem", order = 1)]
    public class ExpGemBlueprint : ScriptableObject
    {
        public EnumDataContainer<GemType, Sprite, Color> gemSpritesAndColors;
    }
}
