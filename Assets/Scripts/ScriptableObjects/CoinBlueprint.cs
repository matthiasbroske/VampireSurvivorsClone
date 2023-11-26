using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "Coin", menuName = "Blueprints/Coin", order = 1)]
    public class CoinBlueprint : ScriptableObject
    {
        public EnumDataContainer<CoinType, Sprite> coinSprites;
    }
}
