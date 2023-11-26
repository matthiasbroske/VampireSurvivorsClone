using UnityEngine;
using UnityEngine.UI;

namespace Vampire
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Image pauseButton;
        [SerializeField] private Sprite pauseSprite, playSprite;
        [SerializeField] private GameObject pauseMenu;
        private bool paused = false;
        private bool timeIsFrozen = false;

        public bool TimeIsFrozen { set => timeIsFrozen = value; }

        public void PlayPause()
        {
            if (paused = !paused)
            {
                if (!timeIsFrozen)
                    Time.timeScale = 0;
                pauseButton.sprite = playSprite;
                pauseMenu.SetActive(true);
            }
            else
            {
                if (!timeIsFrozen)
                    Time.timeScale = 1;
                pauseButton.sprite = pauseSprite;
                pauseMenu.SetActive(false);
            }
        }
    }
}
