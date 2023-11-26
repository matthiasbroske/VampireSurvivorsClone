using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

namespace Vampire
{
    public class Character : IDamageable, ISpatialHashGridClient
    {
        [Header("Dependencies")]
        [SerializeField] protected Transform centerTransform;
        [SerializeField] protected Transform lookIndicator;
        [SerializeField] protected float lookIndicatorRadius;
        [SerializeField] protected TextMeshProUGUI levelText;
        [SerializeField] protected AbilitySelectionDialog abilitySelectionDialog;
        [SerializeField] protected PointBar healthBar;  // 血量條
        [SerializeField] protected PointBar expBar;  // 經驗條
        [SerializeField] protected Collider2D collectableCollider;
        [SerializeField] protected Collider2D meleeHitboxCollider;
        [SerializeField] protected ParticleSystem dustParticles;
        [SerializeField] protected Material defaultMaterial, hitMaterial, deathMaterial;
        [SerializeField] protected ParticleSystem deathParticles;
        protected CharacterBlueprint characterBlueprint;
        protected UpgradeableMovementSpeed movementSpeed;
        protected UpgradeableArmor armor;
        protected bool alive = true;
        protected int currentLevel = 1;
        protected float currentExp = 0;
        protected float nextLevelExp = 5;
        protected float expToNextLevel = 5;
        protected float currentHealth;
        protected SpriteRenderer spriteRenderer;
        protected SpriteAnimator spriteAnimator;
        protected AbilityManager abilityManager;
        protected EntityManager entityManager;
        protected StatsManager statsManager;
        protected Rigidbody2D rb;
        protected ZPositioner zPositioner;
        protected Vector2 lookDirection = Vector2.right;
        protected CoroutineQueue coroutineQueue;
        protected Coroutine hitAnimationCoroutine = null;
        protected Vector2 moveDirection;
        public Vector2 LookDirection 
        { 
            get { return lookDirection; } 
            set 
            {
                if (value != Vector2.zero)
                    lookDirection = value; 
            }
        }
        public Transform CenterTransform { get => centerTransform; }
        public Collider2D CollectableCollider { get => collectableCollider; }
        public float Luck { get => characterBlueprint.luck; }
        public int CurrentLevel { get => currentLevel; }
        public UnityEvent<float> OnDealDamage { get; } = new UnityEvent<float>();
        public UnityEvent OnDeath { get; } = new UnityEvent();
        public CharacterBlueprint Blueprint { get => characterBlueprint; }
        public Vector2 Velocity { get => rb.velocity; }
        // Spatial Hash Grid Client Interface
        public Vector2 Position => transform.position;
        public Vector2 Size => meleeHitboxCollider.bounds.size;
        public Dictionary<int, int> ListIndexByCellIndex { get; set; }
        public int QueryID { get; set; } = -1;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            zPositioner = gameObject.AddComponent<ZPositioner>();
            spriteAnimator = GetComponentInChildren<SpriteAnimator>();
            spriteRenderer = spriteAnimator.GetComponent<SpriteRenderer>();
            characterBlueprint = CrossSceneData.CharacterBlueprint;
        }

        public virtual void Init(EntityManager entityManager, AbilityManager abilityManager, StatsManager statsManager)
        {
            //this.characterBlueprint = characterBlueprint;
            this.entityManager = entityManager;
            this.abilityManager = abilityManager;
            this.statsManager = statsManager;
            // Add listener to increase damage dealt whenever player deals damage
            OnDealDamage.AddListener(statsManager.IncreaseDamageDealt);
            // Initialize the coroutine queue
            coroutineQueue = new CoroutineQueue(this);
            coroutineQueue.StartLoop();
            // Initialize starting health and exp
            currentHealth = characterBlueprint.hp;
            healthBar.Setup(currentHealth, 0, characterBlueprint.hp);
            expBar.Setup(currentExp, 0, nextLevelExp);
            currentLevel = 1;
            UpdateLevelDisplay();
            // Initialize animations
            spriteAnimator.Init(characterBlueprint.walkSpriteSequence, characterBlueprint.walkFrameTime, false);
            // Limit max speed using drag
            movementSpeed = new UpgradeableMovementSpeed();
            movementSpeed.Value = characterBlueprint.movespeed;
            abilityManager.RegisterUpgradeableValue(movementSpeed, true);
            UpdateMoveSpeed();
            // Initialize upgradeable armor
            armor = new UpgradeableArmor();
            armor.Value = characterBlueprint.armor;
            abilityManager.RegisterUpgradeableValue(armor, true);
            zPositioner.Init(transform);
        }

        protected virtual void Update()
        {
            // Look in movement direction
            lookIndicator.transform.localPosition = lookDirection * lookIndicatorRadius;
            spriteRenderer.flipX = lookDirection.x < 0;
        }

        protected virtual void FixedUpdate()
        {
            if (moveDirection != Vector2.zero)
                lookDirection = moveDirection;
            else
                StopWalkAnimation();
            if (alive)
                rb.velocity += moveDirection * characterBlueprint.acceleration * Time.deltaTime;
        }

        public void GainExp(float exp)
        {
            if (alive)
                coroutineQueue.EnqueueCoroutine(GainExpCoroutine(exp));
        }

        private IEnumerator GainExpCoroutine(float exp)
        {
            if (alive)
            {
                // Level up as many times as possible with the exp given
                while (currentExp + exp >= nextLevelExp)
                {
                    // Use only as much exp as brings us up to the next level
                    float expDiff = nextLevelExp - currentExp;
                    currentExp += expDiff;
                    exp -= expDiff;
                    expBar.Setup(currentExp, 0, nextLevelExp);  // Temp make the exp bar appear to be full
                    // Wait until the player has finished leveling up
                    yield return LevelUpCoroutine();
                    // Update the exp bar to show the progress to the next level
                    float prevLevelExp = nextLevelExp;
                    expToNextLevel += characterBlueprint.LevelToExpIncrease(currentLevel);
                    nextLevelExp += expToNextLevel;
                    expBar.Setup(currentExp, prevLevelExp, nextLevelExp);
                }
                // Add remaining exp
                currentExp += exp;
                expBar.AddPoints(exp);
            }
        }

        private IEnumerator LevelUpCoroutine()
        {
            if (alive)
            {
                // Level up
                currentLevel++;
                UpdateLevelDisplay();
                // Open the level up dialog menu
                abilitySelectionDialog.Open();
                // Wait for the menu to be closed
                while (abilitySelectionDialog.MenuOpen)
                {
                    yield return null;
                }
            }
        }

        private void UpdateLevelDisplay()
        {
            levelText.text = "LV " + currentLevel;
        }

        public override void Knockback(Vector2 knockback)
        {
            rb.velocity += knockback * Mathf.Sqrt(rb.drag);
        }

        public override void TakeDamage(float damage, Vector2 knockback = default(Vector2))
        {
            if (alive)
            {
                // Apply armor
                if (armor.Value >= damage)
                    damage = damage < 1 ? damage : 1;
                else
                    damage -= armor.Value;
                // Decrease health
                healthBar.SubtractPoints(damage);
                currentHealth -= damage;
                // Knockback
                rb.velocity += knockback * Mathf.Sqrt(rb.drag);
                statsManager.IncreaseDamageTaken(damage);
                if (currentHealth <= 0)
                {
                    StartCoroutine(DeathAnimation());
                }
                else
                {
                    if (hitAnimationCoroutine != null) StopCoroutine(hitAnimationCoroutine);
                    hitAnimationCoroutine = StartCoroutine(HitAnimation());
                }
            }
        }

        private IEnumerator HitAnimation()
        {
            spriteRenderer.sharedMaterial = hitMaterial;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.sharedMaterial = defaultMaterial;
        }

        private IEnumerator DeathAnimation()
        {
            alive = false;
            spriteRenderer.sharedMaterial = deathMaterial;

            abilityManager.DestroyActiveAbilities();
            StopWalkAnimation();
            deathParticles.Play();
            float height = spriteRenderer.bounds.size.y;
            float t = 0;
            while (t < 1)
            {
                spriteRenderer.sharedMaterial = deathMaterial;
                deathParticles.transform.position = transform.position + Vector3.up * height * (1-t);
                deathMaterial.SetFloat("_Wipe", t);
                t += Time.deltaTime;
                yield return null;
            }
            deathMaterial.SetFloat("_Wipe", 1.0f);

            yield return new WaitForSeconds(0.5f);

            OnDeath.Invoke();
            spriteRenderer.enabled = false;
        }

        public void GainHealth(float health)
        {
            healthBar.AddPoints(health);
            currentHealth += health;
            if (currentHealth > characterBlueprint.hp)
                currentHealth = characterBlueprint.hp;
        }

        public void SetLookDirecton(InputAction.CallbackContext context)
        {
            LookDirection = context.ReadValue<Vector2>();
        }

        public void UpdateMoveSpeed()
        {
            rb.drag = characterBlueprint.acceleration / (movementSpeed.Value * movementSpeed.Value);
        }

        public void Move(Vector2 moveDirection)
        {
            this.moveDirection = moveDirection;
        }

        public void StartWalkAnimation()
        {
            if (alive)
                spriteAnimator.StartAnimating();
            //dustParticles.Play();
        }

        public void StopWalkAnimation()
        {
            spriteAnimator.StopAnimating(true);
            //dustParticles.Stop();
        }

        public void SetMoveDirection(InputAction.CallbackContext context)
        {
            moveDirection = context.action.ReadValue<Vector2>().normalized;
        }
    }
}
