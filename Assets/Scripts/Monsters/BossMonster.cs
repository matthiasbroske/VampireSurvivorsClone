using System.Collections;
using System.Linq;
using UnityEngine;

namespace Vampire
{
    public class BossMonster : Monster
    {
        protected new BossMonsterBlueprint monsterBlueprint;
        protected BossAbility[] abilities;
        protected Coroutine act = null;
        public Rigidbody2D Rigidbody { get => rb; }
        public SpriteAnimator Animator { get => monsterSpriteAnimator; }
        protected float timeSinceLastMeleeAttack;

        public override void Setup(int monsterIndex, Vector2 position, MonsterBlueprint monsterBlueprint, float hpBuff = 0)
        {
            base.Setup(monsterIndex, position, monsterBlueprint, hpBuff);
            this.monsterBlueprint = (BossMonsterBlueprint) monsterBlueprint;
            abilities = new BossAbility[this.monsterBlueprint.abilityPrefabs.Length];
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i] = Instantiate(this.monsterBlueprint.abilityPrefabs[i], transform).GetComponent<BossAbility>();
                abilities[i].Init(this, entityManager, playerCharacter);
            }
            act = StartCoroutine(Act());
        }

        protected override void Update()
        {
            base.Update();
            timeSinceLastMeleeAttack += Time.deltaTime;
        }
        // protected override void FixedUpdate()
        // {
        //     base.FixedUpdate();
        //     // Vector2 moveDirection = (playerCharacter.transform.position - transform.position).normalized;
        //     // rb.velocity += moveDirection * monsterBlueprint.acceleration * Time.fixedDeltaTime;
        // }

        public void Move(Vector2 direction, float deltaTime)
        {
            rb.velocity += direction * monsterBlueprint.acceleration * deltaTime;
        }

        public void Freeze()
        {
            rb.velocity = Vector2.zero;
        }

        private IEnumerator Act()
        {
            while (true)
            {
                float[] abilityScores = abilities.Select(a => a.Score()).ToArray();
                float totalScore = abilityScores.Sum();
                float rand = Random.Range(0f, totalScore);
                float cumulative = 0;
                int abilityIndex = -1;
                for (int i = 0; i < abilities.Length; i++)
                {
                    abilities[i].Deactivate();
                    cumulative += abilityScores[i];
                    if (abilityIndex == -1 && rand < cumulative)
                        abilityIndex = i;
                }
                if (abilityIndex == -1)
                {
                    Debug.Log(totalScore);
                    yield return new WaitForSeconds(1);
                }
                else
                    yield return abilities[abilityIndex].Activate();
            }
        }

        protected override void DropLoot()
        {
            base.DropLoot();
            if (monsterBlueprint.chestBlueprint != null)
                entityManager.SpawnChest(monsterBlueprint.chestBlueprint, transform.position);
        }

        public override IEnumerator Killed(bool killedByPlayer = true)
        {
            foreach (BossAbility ability in abilities)
                Destroy(ability.gameObject);
            StopCoroutine(act);
            yield return base.Killed(killedByPlayer);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (((monsterBlueprint.meleeLayer & (1 << col.collider.gameObject.layer)) != 0))
            {
                IDamageable damageable = col.collider.GetComponentInParent<IDamageable>();
                Vector2 knockbackDirection = (damageable.transform.position - transform.position).normalized;
                if (timeSinceLastMeleeAttack > monsterBlueprint.meleeAttackDelay)
                {
                    damageable.TakeDamage(monsterBlueprint.meleeDamage, monsterBlueprint.meleeKnockback * knockbackDirection);
                    timeSinceLastMeleeAttack = 0;
                }
                else
                {
                    damageable.TakeDamage(0, monsterBlueprint.meleeKnockback * knockbackDirection);
                }
            }

            if (col.gameObject.TryGetComponent<Chest>(out Chest chest))
            {
                chest.OpenChest(false);
            }
        }
    }
}
