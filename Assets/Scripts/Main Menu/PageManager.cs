using UnityEngine;

namespace Vampire
{
    public class PageManager : MonoBehaviour
    {
        [SerializeField] private int defaultPage;
        [SerializeField] private GameObject[] pages;

        void Awake()
        {
            SwitchToPage(defaultPage);
        }

        public void SwitchToPage(int pageIndex)
        {
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].SetActive(i == pageIndex);
            }
        }
    }
}
