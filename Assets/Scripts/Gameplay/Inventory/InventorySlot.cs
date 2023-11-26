using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampire
{
    public class InventorySlot : MonoBehaviour
    {
        [field: SerializeField] public CollectableType CollectableType;
        [SerializeField] private GameObject countObject;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Image itemImage;
        private List<Collectable> items;
        private FastList<Collectable> itemsBeingAdded;

        public void Init()
        {
            items = new List<Collectable>();
            itemsBeingAdded = new FastList<Collectable>();
            countObject.SetActive(false);
            itemImage.color = new Color(1, 1, 1, 0.5f);
        }

        public void AddItemBeingCollected(Collectable item)
        {
            itemsBeingAdded.Add(item);
        }

        public void FinalizeAddItemBeingCollected(Collectable item)
        {
            if (itemsBeingAdded.Contains(item))
                itemsBeingAdded.Remove(item);
            AddItem(item);
        }

        public void AddItem(Collectable item)
        {
            items.Add(item);
            countText.text = items.Count.ToString();
            if (!countObject.activeInHierarchy)
            {
                countObject.SetActive(true);
                itemImage.color = Color.white;
            }
        }

        public void UseItem()
        {
            if (items.Count > 0)
            {
                items[0].Use();
                items.RemoveAt(0);
                countText.text = items.Count.ToString();

                if (items.Count == 0)
                {
                    countObject.SetActive(false);
                    itemImage.color = new Color(1, 1, 1, 0.5f);
                }
            }
        }

        public bool IsFull()
        {
            return items.Count + itemsBeingAdded.Count >= CollectableType.inventoryStackSize;
        }
    }
}
