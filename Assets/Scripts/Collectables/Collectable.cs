using System.Collections;
using UnityEngine;

namespace Vampire
{
    public abstract class Collectable : MonoBehaviour
    {
        [Header("Type")]
        [field: SerializeField] public CollectableType CollectableType;
        [Header("Spawn Animation")]
        [SerializeField] protected float saSpeed = 1;
        [SerializeField] protected float saHeight = 1;
        [SerializeField] protected float saOffsetMax = 0.2f;
        [Header("Collect Animation")]
        [SerializeField] protected float juiciness = 2;
        [SerializeField] protected float lerpTime = 1;
        [Header("Attributes")]
        [SerializeField] protected bool magnetic = false;
        protected EntityManager entityManager;
        protected Character playerCharacter;
        protected ZPositioner zPositioner;
        protected Collider2D col;
        protected bool beingCollected = false;

        protected virtual void Awake()
        {
            col = GetComponent<Collider2D>();
            zPositioner = gameObject.AddComponent<ZPositioner>();
        }

        public virtual void Init(EntityManager entityManager, Character playerCharacter)
        {
            this.entityManager = entityManager;
            this.playerCharacter = playerCharacter;
            zPositioner.Init(playerCharacter.transform);
        }

        public virtual void Setup(bool spawnAnimation = true, bool collectableDuringSpawn = true) {
            col.enabled = !spawnAnimation || collectableDuringSpawn;
            beingCollected = false;
            if (magnetic)
                entityManager.MagneticCollectables.Add(this);
            if (spawnAnimation)
                StartCoroutine(SpawnAnimation());
            gameObject.SetActive(true);
        }

        public virtual void Collect(CollectionMode collectionMode = CollectionMode.FromGround)
        {
            // Don't collect again if already being collected
            if (beingCollected)
                return;

            // Don't collect if storable in inventory and inventory is full
            bool storeInInventory = CollectableType.inventoryStackSize > 0;
            bool hasInventorySlot = entityManager.Inventory.TryGetInventorySlot(this, out InventorySlot inventorySlot);
            if (storeInInventory && hasInventorySlot && inventorySlot.IsFull())
                return;
            
            // Flag that this is being collected so that it does not accidentally get collected a second time
            beingCollected = true;
            col.enabled = false;
            if (magnetic)
                entityManager.MagneticCollectables.Remove(this);

            // Fly self to player inventory if storable in inventory, otherwise send to player themselves
            if (storeInInventory && hasInventorySlot)
                StartCoroutine(FlyToInventory(inventorySlot, collectionMode));
            else
                StartCoroutine(FlyToPlayer(collectionMode));
        }

        public void Use()
        {
            OnCollected();
        }

        protected abstract void OnCollected();

        protected virtual IEnumerator FlyToPlayer(CollectionMode collectionMode = CollectionMode.FromGround)
        {
            if (collectionMode == CollectionMode.FromChest)
            {
                yield return StartCoroutine(ChestAnimation());
                zPositioner.enabled = true;
            }

            float distance = Vector2.Distance(transform.position, playerCharacter.CenterTransform.position);
            if (distance == 0.0f) distance = Mathf.Epsilon;
            float c = juiciness / distance;
            float timeScale = 1.0f/(lerpTime*Mathf.Sqrt(distance));
            float t = -Time.deltaTime*timeScale;
            Vector3 pickupPos = transform.position;
            while (t < 1)
            {
                t += Time.deltaTime*timeScale;
                float lerpT = EasingUtils.EaseInBack(t, c);
                if (lerpT >= 1) break;
                transform.position = Vector3.LerpUnclamped(pickupPos, playerCharacter.CenterTransform.position, lerpT);
                yield return null;
            }
            transform.position = playerCharacter.CenterTransform.position;
            yield return null;
            OnCollected();
        }

        protected virtual IEnumerator FlyToInventory(InventorySlot inventorySlot, CollectionMode collectionMode = CollectionMode.FromGround)
        {
            // Launch the collectable into the air before sending it to the inventory 
            // if collected from a chest
            inventorySlot.AddItemBeingCollected(this);
            if (collectionMode == CollectionMode.FromChest)
                yield return StartCoroutine(ChestAnimation());

            float t = 0;
            float c = 0;//juiciness / distance;
            float timeScale = 2.0f;
            t = -Time.deltaTime*timeScale;
            Vector3 pickupPos = transform.position;
            while (t < 1)
            {
                t += Time.deltaTime*timeScale;
                float lerpT = EasingUtils.EaseInBack(t, c);
                if (lerpT >= 1) break;
                transform.position = Vector3.LerpUnclamped(pickupPos, Camera.main.ScreenToWorldPoint(inventorySlot.transform.position), lerpT);
                yield return null;
            }
            transform.position = Camera.main.ScreenToWorldPoint(inventorySlot.transform.position);
            yield return null;
            gameObject.SetActive(false);
            inventorySlot.FinalizeAddItemBeingCollected(this);
        }

        protected virtual IEnumerator SpawnAnimation()
        {
            zPositioner.enabled = false;
            float t = 0;
            float horizontalSpeed = Random.Range(-saOffsetMax, saOffsetMax);
            Vector3 spawnPosition = transform.position;
            while (!beingCollected && t < 1)
            {
                transform.position = spawnPosition + Vector3.up*EasingUtils.Bounce(t, saHeight) + Vector3.right*horizontalSpeed*t;
                t += Time.deltaTime*saSpeed;
                yield return null;
            }
            col.enabled = true;
            zPositioner.enabled = true;
        }

        protected virtual IEnumerator ChestAnimation()
        {
            float t = 0;
            float horizontalSpeed = Random.Range(-saOffsetMax, saOffsetMax);
            Vector3 spawnPosition = transform.position;
            while (t < 0.2f)
            {
                zPositioner.enabled = false;
                transform.position = spawnPosition + Vector3.up*EasingUtils.Bounce(t, saHeight) + Vector3.right*horizontalSpeed*t;
                t += Time.deltaTime*saSpeed;
                yield return null;
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (collider == playerCharacter.CollectableCollider)
            {
                Collect();
            }
        }

        public enum CollectionMode
        {
            FromGround,
            FromChest
        }
    }
}
