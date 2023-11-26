using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class ExplosiveProjectile : Projectile
    {
        [SerializeField] protected LayerMask explosionLayerMask;
        [SerializeField] protected float explosionRadius;
        [SerializeField] protected float explosionDamage;
        [SerializeField] protected float explosionKnockback;
        [SerializeField] protected float explosionDuration = 0.25f;
        [SerializeField] protected SpriteRenderer explosionSpriteRenderer;

        public void SetupExplosion(float explosionDamage, float explosionRadius, float explosionKnockback)
        {
            this.explosionDamage = explosionDamage;
            this.explosionRadius = explosionRadius;
            this.explosionKnockback = explosionKnockback;
        }

        protected override void HitDamageable(IDamageable damageable)
        {
            // Explode
            StartCoroutine(Explosion());
        }

        protected override void HitNothing()
        {
            // Explode
            StartCoroutine(Explosion());
        }

        protected IEnumerator Explosion()
        {
            projectileSpriteRenderer.gameObject.SetActive(false);
            destructionParticleSystem.Play();
            float t = 0;
            Dictionary<Collider2D, bool> damagedColliders = new Dictionary<Collider2D, bool>();
            while (t < 1)
            {
                Color c = explosionSpriteRenderer.color;
                c.a = EasingUtils.EaseOutQuart(1-t)*0.5f;
                explosionSpriteRenderer.color = c;
                explosionSpriteRenderer.transform.localScale = Vector3.one * EasingUtils.EaseOutQuart(t) * explosionRadius * 10;
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, t*explosionRadius, explosionLayerMask);
                foreach (Collider2D collider in hitColliders)
                {
                    if (!damagedColliders.ContainsKey(collider))
                    {
                        Vector2 dir = collider.transform.position - transform.position;
                        collider.GetComponentInParent<IDamageable>().TakeDamage(explosionDamage, dir.normalized * explosionKnockback);
                        damagedColliders[collider] = true;
                        OnHitDamageable.Invoke(explosionDamage);
                    }
                }
                t += Time.deltaTime/explosionDuration;
                yield return null;
            }
            explosionSpriteRenderer.color = explosionSpriteRenderer.color - Color.black;
            if (explosionDuration < destructionParticleSystem.main.duration)
                yield return new WaitForSeconds(destructionParticleSystem.main.duration - explosionDuration);
            projectileSpriteRenderer.gameObject.SetActive(true);
            entityManager.DespawnProjectile(projectileIndex, this);
        }
    }
}
