using UnityEngine;
using TMPro;

namespace Vampire
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CoinDisplay : MonoBehaviour
    {
        private TextMeshProUGUI coinText;

        void Start()
        {
            coinText = GetComponent<TextMeshProUGUI>();
            coinText.text = PlayerPrefs.GetInt("Coins").ToString();
        }

        public void UpdateDisplay()
        {
            coinText.text = PlayerPrefs.GetInt("Coins").ToString();
        }
    }
}
