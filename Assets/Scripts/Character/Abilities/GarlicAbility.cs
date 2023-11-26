using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class GarlicAbility : Ability
    {
        [Header("Garlic Stats")]
        [SerializeField] protected LayerMask monsterLayer;
        [SerializeField] protected UpgradeableDamage damage;
        [SerializeField] protected UpgradeableAOE radius;
        [SerializeField] protected UpgradeableDamageRate damageRate;
        [SerializeField] protected UpgradeableKnockback knockback;
        private float timeSinceLastAttack;
        private FastList<GameObject> hitMonsters;
        private CircleCollider2D damageCollider;
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            damageCollider = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public override void Init(AbilityManager abilityManager, EntityManager entityManager, Character playerCharacter)
        {
            base.Init(abilityManager, entityManager, playerCharacter);
            transform.SetParent(playerCharacter.transform);
            transform.localPosition = Vector3.zero;
        }

        protected override void Use()
        {
            base.Use();
            gameObject.SetActive(true);
            hitMonsters = new FastList<GameObject>();
            damageCollider.radius = radius.Value;
            spriteRenderer.transform.localScale = Vector3.one * radius.Value * 2;
        }

        protected override void Upgrade()
        {
            base.Upgrade();
            damageCollider.radius = radius.Value;
            spriteRenderer.transform.localScale = Vector3.one * radius.Value * 2;
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= 1/damageRate.Value)
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius.Value, monsterLayer);
                foreach (Collider2D collider in hitColliders)
                {
                    Damage(collider.GetComponentInParent<IDamageable>());
                }
                timeSinceLastAttack = Mathf.Repeat(timeSinceLastAttack, 1/damageRate.Value);
            }
        }

        private void Damage(IDamageable damageable)
        {
            Vector2 knockbackDirection = (damageable.transform.position - transform.position).normalized;
            damageable.TakeDamage(damage.Value, knockback.Value * knockbackDirection);
            playerCharacter.OnDealDamage.Invoke(damage.Value);
        }

        private void DeregisterMonster(Monster monster)
        {
            hitMonsters.Remove(monster.gameObject);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!hitMonsters.Contains(collider.gameObject) && (monsterLayer & (1 << collider.gameObject.layer)) != 0)
            {
                hitMonsters.Add(collider.gameObject);
                Monster monster = collider.gameObject.GetComponentInParent<Monster>();
                monster.OnKilled.AddListener(DeregisterMonster);
                Damage(monster);
            }
        }
    }
}
