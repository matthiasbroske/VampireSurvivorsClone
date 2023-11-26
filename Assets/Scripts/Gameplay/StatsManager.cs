using UnityEngine;
using TMPro;

namespace Vampire
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI monstersKilledText;
        [SerializeField] private TextMeshProUGUI coinsGainedText;

        private int monstersKilled = 0;
        private float damageDealt = 0;
        private float damageTaken = 0;
        private int coinsGained = 0;

        public int MonstersKilled { get => monstersKilled; }
        public float DamageDealt { get => damageDealt; }
        public float DamageTaken { get => damageTaken; }
        public int CoinsGained { get => coinsGained; }

        public void IncrementMonstersKilled()
        {
            monstersKilled++;
            monstersKilledText.text = monstersKilled.ToString();
        }

        public void IncreaseCoinsGained(int amount)
        {
            coinsGained += amount;
            coinsGainedText.text = coinsGained.ToString();
        }

        public void IncreaseDamageDealt(float damage)
        {
            damageDealt += damage; 
        }

        public void IncreaseDamageTaken(float damage)
        {
            damageTaken += damage;
        }
    }
}
