using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Vampire
{
    public class MolotovFire : MonoBehaviour
    {
        [SerializeField] protected ParticleSystem fireParticles;
        
        public IEnumerator Burn(MolotovThrowable molotov, float damage, float knockback, float duration, float fireRadius, float fireDamageRate, LayerMask targetLayer)
        {
            float t = 0;
            while (t < 1.0f)
            {
                transform.localScale = Vector2.one * fireRadius * 2 * EasingUtils.EaseOutQuart(t);
                fireParticles.transform.localScale = transform.localScale;
                t += Time.deltaTime * 3;
                yield return null;
            }
            transform.localScale = Vector2.one * fireRadius * 2;
            fireParticles.transform.localScale = transform.localScale;
            int burnCount = Mathf.CeilToInt(duration * fireDamageRate);
            for (int i = 0; i < burnCount; i++)
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, fireRadius, targetLayer);
                foreach (Collider2D collider in hitColliders)
                {
                    molotov.Damage(collider.GetComponentInParent<IDamageable>());
                }
                yield return new WaitForSeconds(1/fireDamageRate);
            }
            t = 1;
            while (t > 0.0)
            {
                transform.localScale = Vector2.one * fireRadius * 2 * EasingUtils.EaseOutQuart(t);
                fireParticles.transform.localScale = transform.localScale;
                t -= Time.deltaTime * 4;
                yield return null;
            }
        }
    }
}