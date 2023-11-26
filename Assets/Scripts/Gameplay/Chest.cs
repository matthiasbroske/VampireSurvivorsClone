using UnityEngine;
using System.Collections;

namespace Vampire
{
    public class Chest : MonoBehaviour
    {
        protected ChestBlueprint chestBlueprint; 
        protected EntityManager entityManager;
        protected Character playerCharacter;
        protected ZPositioner zPositioner;
        protected Transform chestItemsParent;
        protected SpriteRenderer spriteRenderer;
        protected bool opened = false;

        public void Init(EntityManager entityManager, Character playerCharacter, Transform chestItemsParent)
        {
            this.entityManager = entityManager;
            this.playerCharacter = playerCharacter;
            this.chestItemsParent = chestItemsParent;
            (zPositioner = gameObject.AddComponent<ZPositioner>()).Init(playerCharacter.transform);
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void Setup(ChestBlueprint chestBlueprint)
        {
            this.chestBlueprint = chestBlueprint;
            transform.localScale = Vector3.one;
            spriteRenderer.sprite = chestBlueprint.closedChest;
            opened = false;
            StartCoroutine(Appear());
        }

        private void SpawnLoot(Loot<GameObject> loot, bool openedByPlayer = true)
        {
            GameObject item = Instantiate(loot.item, chestItemsParent);
            item.transform.position = transform.position;
            transform.position += Vector3.back*0.001f;  // Nudge the collectable in front of the chest
            Collectable collectable = item.GetComponent<Collectable>();
            collectable.Init(entityManager, playerCharacter);
            Coin coin = collectable as Coin;
            if (coin != null)
                coin.Setup(transform.position, loot.coinType, true, true);
            else
                collectable.Setup(true, true);
            // Collect the item immediately if opened by player
            if (openedByPlayer)
                collectable.Collect(Collectable.CollectionMode.FromChest);
        }

        public void OpenChest(bool openedByPlayer = true)
        {
            if (!opened)
            {
                opened = true;
                StartCoroutine(Open(openedByPlayer));
            }
        }

        // This is some truly atrocious code: tread with caution.
        private IEnumerator Open(bool openedByPlayer = true)
        {
            spriteRenderer.sprite = chestBlueprint.openingChest;
            bool spawnLoot = !chestBlueprint.abilityChest || !entityManager.AbilitySelectionDialog.HasAvailableAbilities();
            if (spawnLoot)
                SpawnLoot(chestBlueprint.lootTable.DropLootObject(), openedByPlayer);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.sprite = chestBlueprint.openChest;
            if (!spawnLoot)
                entityManager.AbilitySelectionDialog.Open(false);
            yield return new WaitForSeconds(0.15f);
            float t = 0;
            while (t < 1.0f)
            {
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, EasingUtils.EaseOutQuart(t));
                t += Time.deltaTime*2;
                yield return null;
            }
            entityManager.DespawnChest(this);
        }

        private IEnumerator Appear()
        {
            GetComponent<Collider2D>().enabled = false;
            float t = 0;
            while (t < 1.0f)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, EasingUtils.EaseInQuart(t));
                t += Time.deltaTime*2;
                yield return null;
            }
            transform.localScale = Vector3.one;
            GetComponent<Collider2D>().enabled = true;
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.gameObject == playerCharacter.gameObject)
            {
                OpenChest();
            }
        }
    }
}
