using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampire
{
    public class AbilityCard : MonoBehaviour
    {
        [SerializeField] private Image abilityImage;
        [SerializeField] private RectTransform abilityImageRect;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private float appearSpeed = 3;
        private AbilitySelectionDialog levelUpMenu;
        private Ability ability;

        public void Init(AbilitySelectionDialog levelUpMenu, Ability ability, float waitToAppear)
        {
            abilityImage.sprite = ability.Image;
            float yHeight = abilityImageRect.rect.height;
            float xWidth = ability.Image.textureRect.width / (float) ability.Image.textureRect.height * yHeight;
            if (xWidth > abilityImageRect.rect.width)
            {
                    xWidth = abilityImageRect.rect.width;
                    yHeight = ability.Image.textureRect.height / (float) ability.Image.textureRect.width * xWidth;
            }
            ((RectTransform)abilityImage.transform).sizeDelta = new Vector2(xWidth, yHeight);
            nameText.text = ability.Name;
            descriptionText.text = ability.Description;
            levelText.text = !ability.Owned ? (ability.Level + 1).ToString() : ability.Level + " -> " + (ability.Level+1);
            this.levelUpMenu = levelUpMenu;
            this.ability = ability;
            StartCoroutine(Appear(waitToAppear));
        }

        public IEnumerator Appear(float waitToAppear)
        {
            Vector3 initialScale = transform.localScale;
            transform.localScale = Vector3.zero;
            yield return new WaitForSecondsRealtime(waitToAppear);
            float t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.LerpUnclamped(Vector3.zero, initialScale, EasingUtils.EaseOutBack(t));
                t += Time.unscaledDeltaTime * appearSpeed;
                yield return null;
            }
            transform.localScale = initialScale;
        }

        public void Selected()
        {
            ability.Select();
            levelUpMenu.Close();
        }
    }
}
