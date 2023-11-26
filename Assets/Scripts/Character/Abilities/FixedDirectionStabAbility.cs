using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class FixedDirectionStabAbility : StabAbility
    {
        protected override IEnumerator Stab()
        {
            hitMonsters = new FastList<GameObject>();
            timeSinceLastAttack -= stabTime;
            float t = 0;
            weaponSpriteRenderer.enabled = true;
            Vector2 dir = playerCharacter.LookDirection.x > 0 ? Vector2.right : Vector2.left;
            while (t < stabTime)
            {
                Vector2 attackBoxPosition = (Vector2)playerCharacter.CenterTransform.position + dir * (weaponSize.x/2 + stabOffset + stabDistance/stabTime*t);
                float attackAngle = Vector2.SignedAngle(Vector2.right, dir);
                Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackBoxPosition, weaponSize, attackAngle, targetLayer);
                
                weaponSpriteRenderer.transform.position = attackBoxPosition;
                weaponSpriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, attackAngle);

                foreach (Collider2D collider in hitColliders)
                {
                    if (!hitMonsters.Contains(collider.gameObject))
                    {
                        hitMonsters.Add(collider.gameObject);
                        Monster monster = collider.gameObject.GetComponentInParent<Monster>();
                        DamageMonster(monster, damage.Value, dir*knockback.Value);
                        playerCharacter.OnDealDamage.Invoke(damage.Value);
                    }
                }

                t += Time.deltaTime;
                yield return null;
            }
            Vector2 initialScale = weaponSpriteRenderer.transform.localScale;
            t = 0;
            while (t < 1)
            {
                weaponSpriteRenderer.transform.localPosition = (Vector2)playerCharacter.CenterTransform.position + dir * (weaponSpriteRenderer.transform.localScale.x/initialScale.x*weaponSize.x/2 + stabOffset + stabDistance);
                weaponSpriteRenderer.transform.localScale = Vector2.Lerp(initialScale, Vector2.zero, EasingUtils.EaseInQuart(t));
                t += Time.deltaTime * 4;
                yield return null;
            }
            weaponSpriteRenderer.transform.localScale = initialScale;
            weaponSpriteRenderer.enabled = false;
        }
    }
}
