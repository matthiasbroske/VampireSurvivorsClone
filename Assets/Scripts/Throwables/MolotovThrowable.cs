using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Vampire
{
    public class MolotovThrowable : Throwable
    {
        [SerializeField] protected MolotovFire molotovFire;
        [SerializeField] protected ParticleSystem molotovExplosion;
        protected float duration;
        protected float fireRadius;
        protected float fireDamageRate;

        public void SetupFire(float duration, float fireRadius, float fireDamageRate)
        {
            this.duration = duration;
            this.fireRadius = fireRadius;
            this.fireDamageRate = fireDamageRate;
        }

        protected override void Explode()
        {
            StartCoroutine(Burn());
        }

        protected IEnumerator Burn()
        {
            throwableSpriteRenderer.enabled = false;
            shadowSpriteRenderer.enabled = false;
            molotovFire.gameObject.SetActive(true);
            molotovExplosion.Play();
            yield return StartCoroutine(molotovFire.Burn(this, damage, knockback, duration, fireRadius, fireDamageRate, targetLayer));
            molotovFire.gameObject.SetActive(false);
            throwableSpriteRenderer.enabled = true;
            shadowSpriteRenderer.enabled = true;
            DestroyThrowable();
        }

        public void Damage(IDamageable damageable)
        {
            Vector2 knockbackDirection = (damageable.transform.position - transform.position).normalized;
            damageable.TakeDamage(damage, knockback * knockbackDirection);
            playerCharacter.OnDealDamage.Invoke(damage);
        }
    }
}
