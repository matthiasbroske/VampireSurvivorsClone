using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampire
{
    public class CharacterCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image characterImage;
        [SerializeField] private RectTransform characterImageRect;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI atkText;
        [SerializeField] private TextMeshProUGUI mvspdText;
        [SerializeField] private TextMeshProUGUI armorText;
        [SerializeField] private TextMeshProUGUI abilities;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Color selectColor, buyColor;
        private CharacterSelector characterSelector;
        private CharacterBlueprint characterBlueprint;
        private CoinDisplay coinDisplay;

        public void Init(CharacterSelector characterSelector, CharacterBlueprint characterBlueprint, CoinDisplay coinDisplay)
        {
            this.characterSelector = characterSelector;
            this.characterBlueprint = characterBlueprint;
            this.coinDisplay = coinDisplay;

            characterImage.sprite = characterBlueprint.walkSpriteSequence[0];
            float yHeight = Mathf.Abs(characterImageRect.sizeDelta.y);
            float xWidth = characterBlueprint.walkSpriteSequence[0].textureRect.width / (float) characterBlueprint.walkSpriteSequence[0].textureRect.height * yHeight;
            if (xWidth > Mathf.Abs(characterImageRect.sizeDelta.x))
            {
                    xWidth = Mathf.Abs(characterImageRect.sizeDelta.x);
                    yHeight = characterBlueprint.walkSpriteSequence[0].textureRect.height / (float) characterBlueprint.walkSpriteSequence[0].textureRect.width * xWidth;
            }
            ((RectTransform)characterImage.transform).sizeDelta = new Vector2(xWidth, yHeight);

            nameText.text = characterBlueprint.name.ToString();
            hpText.text = characterBlueprint.hp.ToString();
            atkText.text = characterBlueprint.atk.ToString();
            mvspdText.text = Mathf.RoundToInt(characterBlueprint.movespeed/1.15f * 100f).ToString()+"%";
            armorText.text = characterBlueprint.armor.ToString();
            buttonText.text = characterBlueprint.owned ? "選擇" : "$" + characterBlueprint.cost;
            buttonImage.color = characterBlueprint.owned ? selectColor : buyColor;

            for (int i = 0; i < characterBlueprint.startingAbilities.Length; i++)
            {
                abilities.text += characterBlueprint.startingAbilities[i].GetComponent<Ability>().Name;
                if (i < characterBlueprint.startingAbilities.Length - 1)
                    abilities.text += "，";
            }
        }

        public void Selected()
        {
            if (!characterBlueprint.owned)
            {
                int coinCount = PlayerPrefs.GetInt("Coins");
                if (coinCount >= characterBlueprint.cost)
                {
                    PlayerPrefs.SetInt("Coins", coinCount - characterBlueprint.cost);
                    characterBlueprint.owned = true;
                    buttonText.text = "選擇";
                    buttonImage.color = selectColor;
                    coinDisplay.UpdateDisplay();
                }
            }
            else
            {
                characterSelector.StartGame(characterBlueprint);
            }
        }
    }
}
