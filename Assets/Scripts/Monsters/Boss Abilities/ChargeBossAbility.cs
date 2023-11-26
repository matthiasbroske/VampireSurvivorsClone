using System.Reflection;
using System.Linq;
using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class ChargeBossAbility : BossAbility
    {
        [Header("Charge Details")]
        [SerializeField] protected SpriteRenderer chargeIndicator;
        [SerializeField] protected Color defaultColor, warningColor;
        [SerializeField] protected float chargeUpTime;
        [SerializeField] protected float chargeDelay;
        [SerializeField] protected float chargeCooldown;
        [SerializeField] protected float chargeDistance;
        [SerializeField] protected float chargeSpeed;
        [SerializeField] protected float chargeCutoff = 0.8f;
        protected float timeSinceLastAttack;
        protected float radius;
        protected bool charging = false;

        public override void Init(BossMonster monster, EntityManager entityManager, Character playerCharacter)
        {
            base.Init(monster, entityManager, playerCharacter);
            CircleCollider2D bossCollider = monster.GetComponent<CircleCollider2D>();
            radius = bossCollider.radius;
        }

        // void Update()
        // {
        //     if (active)
        //     {
        //         timeSinceLastAttack += Time.deltaTime;
        //         if (timeSinceLastAttack >= 7)
        //         {
        //             timeSinceLastAttack = Mathf.Repeat(timeSinceLastAttack, 7);
        //             StartCoroutine(ChargeAttack());
        //         }
        //     }
        // }

        void FixedUpdate()
        {
            if (active && !charging)
            {
                Vector2 moveDirection = (playerCharacter.transform.position - monster.transform.position).normalized;
                monster.Move(moveDirection, Time.fixedDeltaTime);
                entityManager.Grid.UpdateClient(monster);
            }
        }

        protected IEnumerator ChargeAttack()
        {
            charging = true;

            // Delay and draw line
            float t = 0;
            chargeIndicator.color = defaultColor;
            chargeIndicator.enabled = true;
            Vector2 direction = playerCharacter.transform.position - monster.transform.position;
            while (t < chargeUpTime)
            {
                direction = playerCharacter.transform.position - monster.transform.position;
                float length = t/chargeUpTime * chargeDistance;

                chargeIndicator.transform.localScale = new Vector3(length, chargeIndicator.transform.localScale.y, 1);
                chargeIndicator.transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, direction));
                chargeIndicator.transform.position = (Vector2) monster.transform.position + direction.normalized * t/chargeUpTime*chargeDistance/2;

                t += Time.deltaTime;
                yield return null;
            }

            // Flash warning
            chargeIndicator.color = warningColor;
            yield return new WaitForSeconds(chargeDelay/3);
            chargeIndicator.color = defaultColor;
            yield return new WaitForSeconds(chargeDelay/3);
            chargeIndicator.color = warningColor;
            yield return new WaitForSeconds(chargeDelay/3);
            chargeIndicator.enabled = false;

            // Move along charge line
            t = 0;
            float CHARGE_FAILSAFE_TIME = 2;
            Vector2 initialPosition = monster.transform.position;
            float distance = 0;
            while (distance < chargeDistance*chargeCutoff && t < CHARGE_FAILSAFE_TIME)
            {
                monster.Move(direction*chargeSpeed, Time.deltaTime);
                distance = Vector2.Distance(monster.transform.position, initialPosition);

                t += Time.deltaTime;
                yield return null;
            }

            monster.Rigidbody.drag *= chargeSpeed/2;
            //monster.Animator.StopAnimating();
            yield return new WaitForSeconds(chargeCooldown);
            //monster.Animator.StartAnimating();
            monster.Rigidbody.drag /= chargeSpeed/2;

            charging = false;
        }

        public override IEnumerator Activate()
        {
            active = true;
            yield return StartCoroutine(ChargeAttack());
        }

        public override float Score()
        {
            // float distance = Vector2.Distance(monster.transform.position, playerCharacter.transform.position);
            // float x = distance / chargeDistance;
            // float u = 0f;
            // float o = 0.25f;
            // float exp = -Mathf.Pow(x-u, 2)/(2*o*o);
            // float score = Mathf.Exp(exp);
            // return score;
            float distance = Vector2.Distance(monster.transform.position, playerCharacter.transform.position);
            return distance / (distance + 1);
        }
    }
}
