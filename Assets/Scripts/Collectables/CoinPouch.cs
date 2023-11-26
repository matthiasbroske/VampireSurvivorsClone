using UnityEngine;

namespace Vampire
{
    public class CoinPouch : MonoBehaviour
    {
        private Character playerCharacter;

        public void Init(Character playerCharacter)
        {
            this.playerCharacter = playerCharacter;
        }

        
    }
}
