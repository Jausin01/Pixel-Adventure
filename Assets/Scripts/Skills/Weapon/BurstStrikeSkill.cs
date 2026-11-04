using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Melee skills made of quick strikes with the weapon itself. Three modes:
    /// Thrust (a chain of growing stabs ahead), Slam (lift, crash down and send out a shockwave) and
    /// Flurry (the weapon darts around the nearest enemy stabbing from every side).
    /// </summary>
    public class BurstStrikeSkill : WeaponSkillBase
    {
        /// <summary>The way the strikes are delivered.</summary>
        public enum StrikeMode { Thrust, Slam, Flurry }

        public StrikeMode mode = StrikeMode.Thrust;

        [Header("Common")]
        public float aimRange = 4f;
        [Tooltip("Number of strikes (Thrust and Flurry).")]
        public int strikes = 3;
        public float interval = 0.1f;
        [Tooltip("Radius of every strike's hit circle.")]
        public float hitRadius = 0.7f;
        public float recover = 0.15f;

        [Header("Thrust")]
        public float reach = 2.2f;
        [Tooltip("Every stab reaches this much further than the one before (0.25 = 25% more).")]
        public float reachGrowth = 0.25f;
        public float thrustTime = 0.06f;

        [Header("Slam")]
        public float slamDistance = 1.6f;
        public float raiseHeight = 1.8f;
        public float raiseTime = 0.3f;
        public float slamTime = 0.07f;
        [Tooltip("Radius of the shockwave that follows the impact. 0 for none.")]
        public float shockRadius;
        [Tooltip("Damage of the shockwave as a fraction of the impact.")]
        public float shockFraction = 0.6f;

        [Header("Flurry")]
        [Tooltip("How far from the enemy each stab lands.")]
        public float flurryDistance = 0.9f;

        /// <inheritdoc/>
        protected override IEnumerator Perform(float damage)
        {
            if (mode == StrikeMode.Thrust) yield return Thrust(damage);
            else if (mode == StrikeMode.Slam) yield return Slam(damage);
            else yield return Flurry(damage);

            yield return new WaitForSeconds(recover);
        }

        private IEnumerator Thrust(float damage)
        {
            Vector2 direction = AimDirection(aimRange);
            Point(direction);

            for (int i = 0; i < strikes; i++)
            {
                Vector2 start = Chest + direction * 0.4f;
                Vector2 tip = Chest + direction * (reach * (1f + reachGrowth * i));

                yield return Tween.MoveTo(transform, transform.position, start, 0.03f, Easing.EaseOutQuad);
                yield return Tween.MoveTo(transform, start, tip, thrustTime, Easing.EaseOutQuad);
                Hit(new HitScanner(), tip, hitRadius, damage);
                Puff(sparkSprite, tip, 1f, 0.15f, 1.6f);
                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator Slam(float damage)
        {
            Vector2 direction = AimDirection(aimRange);
            Vector2 landing = (Vector2)owner.position + direction * slamDistance;
            Vector2 top = landing + Vector2.up * raiseHeight;

            Point(Vector2.up);
            yield return Tween.MoveTo(transform, transform.position, top, raiseTime, Easing.EaseOutQuad);

            Point(Vector2.down);
            yield return Tween.MoveTo(transform, top, landing, slamTime, Easing.EaseInQuad);

            HitScanner scanner = new HitScanner();
            Hit(scanner, landing, hitRadius, damage);
            Ring(landing, hitRadius, 6);

            if (shockRadius > 0f)
            {
                yield return new WaitForSeconds(0.08f);
                Hit(scanner, landing, shockRadius, damage * shockFraction);
                Ring(landing, shockRadius, 10);
            }
        }

        private IEnumerator Flurry(float damage)
        {
            Transform target = TargetFinder.FindNearest(owner.position, aimRange, owner);
            Vector2 center = target != null ? (Vector2)target.position : (Vector2)owner.position + facing.Vector * 1.2f;

            for (int i = 0; i < strikes; i++)
            {
                Vector2 side = Rotate(Vector2.right, Random.Range(0f, 360f));
                SetPosition(center + side * flurryDistance);
                Point(-side);
                Hit(new HitScanner(), center, hitRadius, damage);
                Puff(sparkSprite, center, 1.2f, 0.12f, 1.6f);
                yield return new WaitForSeconds(interval);
            }
        }

        private void Ring(Vector2 center, float radius, int puffs)
        {
            for (int i = 0; i < puffs; i++)
            {
                Vector2 offset = Rotate(Vector2.right, 360f * i / puffs) * radius;
                Puff(dustSprite, center + offset, 1f, 0.35f, 1.8f);
            }
        }
    }
}
