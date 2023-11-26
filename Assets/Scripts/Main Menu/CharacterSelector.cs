using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vampire
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] protected CharacterBlueprint[] characterBlueprints;
        [SerializeField] protected GameObject characterCardPrefab;
        [SerializeField] protected CoinDisplay coinDisplay;

        public void Init()
        {
            for (int i = 0; i < characterBlueprints.Length; i++)
            {
                CharacterCard characterCard = Instantiate(characterCardPrefab, this.transform).GetComponent<CharacterCard>();
                characterCard.Init(this, characterBlueprints[i], coinDisplay);
            }
        }
        
        public void StartGame(CharacterBlueprint characterBlueprint)
        {
            CrossSceneData.CharacterBlueprint = characterBlueprint;
            SceneManager.LoadScene(1);
        }
    }
}
