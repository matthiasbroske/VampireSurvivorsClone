using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class BazookaGunAbility : GunAbility
    {
        [Header("Bazooka Gun Stats")]
        [SerializeField] protected GameObject bazookaGun;
        [SerializeField] protected Transform launchTransform;
        [SerializeField] protected ParticleSystem launchParticles;
        [SerializeField] protected UpgradeableAOE explosionAOE;
        [SerializeField] protected Vector2 hoverOffset;
        [SerializeField] protected float targetRadius = 5;
        protected Vector2 currHoverOffset;
        protected Vector3 gunDirection = Vector2.right;
        protected float theta = 0;

        protected override void Update()
        {
            base.Update();

            // Rotate the gun if it is reloading
            float reloadRotation = 0;
            float t = timeSinceLastAttack/cooldown.Value;
            if (t > 0 && t < 1)
            {
                reloadRotation = t * 360;
            }

            //gunDirection = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            currHoverOffset = hoverOffset + Vector2.up * Mathf.Sin(Time.time*5)*0.1f;
            bazookaGun.transform.position = (Vector2)playerCharacter.CenterTransform.position + currHoverOffset;
            //bazookaGun.transform.rotation = Quaternion.Euler(0, 0, theta - reloadRotation);
        }

        protected override void LaunchProjectile()
        {
            StartCoroutine(LaunchProjecileAnimation());
        }

        protected IEnumerator LaunchProjecileAnimation()
        {
            ISpatialHashGridClient targetEntity = entityManager.Grid.FindClosestInRadius(bazookaGun.transform.position, targetRadius);
            
            Vector2 launchDirection = targetEntity == null ? Random.insideUnitCircle.normalized : (targetEntity.Position - (Vector2)bazookaGun.transform.position).normalized;

            float targetTheta = Vector2.SignedAngle(Vector2.right, launchDirection);
            float initialTheta = theta;

            float t = 0;
            float tMax = 1/firerate.Value*0.45f;
            while (t < tMax)
            {
                float tScaled = t / tMax;
                if (targetEntity != null)
                {
                    launchDirection = (targetEntity.Position - (Vector2)bazookaGun.transform.position).normalized;
                    targetTheta = Vector2.SignedAngle(Vector2.right, launchDirection);
                }
                theta = Mathf.Lerp(initialTheta, targetTheta, EasingUtils.EaseOutBack(t));
                bazookaGun.transform.rotation = Quaternion.Euler(0, 0, theta);
                //bazookaGun.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, initialTheta), Quaternion.Euler(0, 0, targetTheta), EasingUtils.EaseOutBack(t));
                t += Time.deltaTime;
                yield return null;
            }
            if (targetEntity != null)
            {
                t = 0;
                while (t < tMax)
                {
                    float tScaled = t / tMax;
                    launchDirection = (targetEntity.Position - (Vector2)bazookaGun.transform.position).normalized;
                    targetTheta = Vector2.SignedAngle(Vector2.right, launchDirection);
                    bazookaGun.transform.rotation = Quaternion.Euler(0, 0, targetTheta);
                    //bazookaGun.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, initialTheta), Quaternion.Euler(0, 0, targetTheta), EasingUtils.EaseOutBack(t));
                    t += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(tMax);
            }
            theta = targetTheta;
            bazookaGun.transform.rotation = Quaternion.Euler(0, 0, theta);

            ExplosiveProjectile projectile = (ExplosiveProjectile) entityManager.SpawnProjectile(projectileIndex, launchTransform.position, damage.Value, knockback.Value, speed.Value, monsterLayer);
            projectile.SetupExplosion(damage.Value, explosionAOE.Value, knockback.Value);
            projectile.OnHitDamageable.AddListener(playerCharacter.OnDealDamage.Invoke);
            projectile.Launch(launchDirection);
            launchParticles.Play();
        }
    }
}
