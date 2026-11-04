using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Fires projectiles from the weapon: a single shot, a spread (fan) or several volleys. One component, many skills:
    /// hatchet volley, javelin, multishot, piercing shot, fan of knives and fireball differ only in these settings.
    /// </summary>
    public class FanSkill : WeaponSkillBase
    {
        [Header("Projectiles")]
        public Sprite projectileSprite;
        [Tooltip("Projectiles per volley.")]
        public int count = 3;
        [Tooltip("Total angle covered by one volley, in degrees.")]
        public float spreadDegrees = 30f;
        [Tooltip("How many volleys are fired.")]
        public int volleys = 1;
        public float volleyInterval = 0.15f;
        public float speed = 12f;
        public float lifetime = 0.9f;
        public float hitRadius = 0.35f;
        [Tooltip("Keep flying after hitting something.")]
        public bool pierce;
        [Tooltip("Radius of the blast where a projectile ends. 0 for none.")]
        public float explodeRadius;
        [Tooltip("Spin in degrees per second. 0 makes the projectile face its direction.")]
        public float spinDegrees;
        [Tooltip("Multiplies the projectile sprite size.")]
        public float spriteScale = 1f;

        [Header("Timing")]
        public float aimRange = 7f;
        public float windup = 0.12f;
        public float recover = 0.15f;

        /// <inheritdoc/>
        protected override IEnumerator Perform(float damage)
        {
            Vector2 direction = AimDirection(aimRange);
            Point(direction);
            yield return Tween.MoveTo(transform, transform.position, Chest + direction * 0.5f, windup, Easing.EaseOutQuad);

            for (int volley = 0; volley < volleys; volley++)
            {
                for (int i = 0; i < count; i++)
                {
                    float t = count == 1 ? 0f : (float)i / (count - 1) - 0.5f;
                    Fire(Rotate(direction, t * spreadDegrees), damage);
                }
                Puff(sparkSprite, Chest + direction * 0.7f, 1f, 0.15f, 1.5f);
                if (volley < volleys - 1) yield return new WaitForSeconds(volleyInterval);
            }

            yield return new WaitForSeconds(recover);
        }

        private void Fire(Vector2 direction, float damage)
        {
            GameObject shot = new GameObject("Skill Projectile", typeof(SpriteRenderer));
            shot.transform.position = new Vector3(Chest.x + direction.x * 0.6f, Chest.y + direction.y * 0.6f, 0f);
            shot.transform.localScale = Vector3.one * spriteScale;

            SpriteRenderer renderer = shot.GetComponent<SpriteRenderer>();
            renderer.sprite = projectileSprite;
            renderer.sharedMaterial = SpriteMaterial;
            renderer.sortingOrder = 9;

            shot.AddComponent<SkillProjectile>().Launch(owner, direction, speed, lifetime, damage, hitRadius, pierce,
                explodeRadius, spinDegrees, burstSprite);
        }
    }
}
