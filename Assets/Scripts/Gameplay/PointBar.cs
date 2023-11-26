using UnityEngine;
using UnityEngine.Events;

namespace Vampire
{
    public class PointBar : MonoBehaviour
    {
        [SerializeField] protected RectTransform barBackground, barFill;
        [SerializeField] protected UnityEvent onEmpty, onFull;

        protected float currentPoints, minPoints, maxPoints;
        protected bool clamp;

        public float CurrentPoints { get => currentPoints; set => currentPoints = value; }

        public void Setup(float currentPoints, float minPoints, float maxPoints, bool clamp = true)
        {
            this.currentPoints = currentPoints;
            this.minPoints = minPoints;
            this.maxPoints = maxPoints;
            this.clamp = clamp;
            UpdateDisplay();
        }

        public void AddPoints(float points)
        {
            currentPoints += points;
            CheckPoints();
            UpdateDisplay();
        }

        public void SubtractPoints(float points)
        {
            currentPoints -= points;
            CheckPoints();
            UpdateDisplay();
        }

        public void SetPoints(float points)
        {
            currentPoints = points;
            CheckPoints();
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            barFill.sizeDelta = new Vector2(barBackground.rect.width * (currentPoints - minPoints)/(maxPoints - minPoints), barFill.sizeDelta.y);
        }

        private void CheckPoints()
        {
            if (currentPoints >= maxPoints)
            {
                onFull.Invoke();
                if (clamp)
                    currentPoints = maxPoints;
            }
            else if (currentPoints <= minPoints)
            {
                onEmpty.Invoke();
                if (clamp)
                    currentPoints = minPoints;
            }
        }
    }
}