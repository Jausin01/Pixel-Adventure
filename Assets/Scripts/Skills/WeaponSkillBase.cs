using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Base class for the skills of every weapon except the sword. Takes control of the weapon while the skill runs
    /// (no normal attacks, no following the player) and gives subclasses the common helpers: aiming, placing the weapon,
    /// hitting targets and spawning effects. One weapon can carry several skills; each is told apart by <see cref="skillType"/>.
    /// Subclasses implement only the skill itself in <see cref="Perform"/>.
    /// </summary>
    public abstract class WeaponSkillBase : MonoBehaviour, ISkillExecutor
    {
        [Tooltip("Which skill this component performs.")]
        public SkillType skillType;

        public Transform owner;
        public PlayerFacing facing;
        public HoverFollower follower;
        public AimRotation aim;
        [Tooltip("Optional. Weapons that point themselves while idle (the bow) must hand that over during a skill.")]
        public IdleFacingRotator idleRotator;
        [Tooltip("Optional. Plays a short slow-motion effect when a hit connects.")]
        public HitStop hitStop;

        [Header("Effects")]
        [Tooltip("Dust, sparks and bursts. Optional.")]
        public Sprite dustSprite;
        public Sprite sparkSprite;
        public Sprite burstSprite;

        private IWeapon weapon;

        /// <inheritdoc/>
        public SkillType Type { get { return skillType; } }

        /// <inheritdoc/>
        public void Execute(SkillDefinition skill, float damage)
        {
            StartCoroutine(Run(damage));
        }

        /// <summary>The skill itself. The weapon is already under control when this starts.</summary>
        protected abstract IEnumerator Perform(float damage);

        private void OnDisable()
        {
            if (weapon != null) weapon.Locked = false;
            if (follower != null) follower.Paused = false;
            if (idleRotator != null) idleRotator.Paused = false;
        }

        private IEnumerator Run(float damage)
        {
            weapon = GetComponent<IWeapon>();
            weapon.Locked = true;
            follower.Paused = true;
            if (idleRotator != null) idleRotator.Paused = true;

            yield return Perform(damage);

            if (idleRotator == null) aim.Aim(Vector2.up);
            else idleRotator.Paused = false;
            follower.Paused = false;
            weapon.Locked = false;
        }

        /// <summary>The attacker's chest, where most skills start.</summary>
        protected Vector2 Chest { get { return (Vector2)owner.position + Vector2.up * 0.5f; } }

        /// <summary>Material of the weapon's sprite, for the effects.</summary>
        protected Material SpriteMaterial { get { return GetComponent<SpriteRenderer>().sharedMaterial; } }

        /// <summary>Places the weapon in the world, keeping its depth.</summary>
        protected void SetPosition(Vector2 position)
        {
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        /// <summary>Points the weapon along a direction.</summary>
        protected void Point(Vector2 direction)
        {
            aim.Aim(direction);
        }

        /// <summary>Direction towards the nearest target within range, or the way the player faces.</summary>
        protected Vector2 AimDirection(float range)
        {
            Transform target = TargetFinder.FindNearest(owner.position, range, owner);
            Vector2 direction = target != null ? (Vector2)target.position - Chest : facing.Vector;
            return direction.sqrMagnitude > 0.001f ? direction.normalized : facing.Vector;
        }

        /// <summary>Damages new targets inside a circle. Plays the hit stop when something is hit.</summary>
        protected bool Hit(HitScanner scanner, Vector2 center, float radius, float damage)
        {
            bool hit = scanner.Scan(center, radius, damage, owner);
            if (hit && hitStop != null) hitStop.Trigger();
            return hit;
        }

        /// <summary>Rotates a direction by an angle in degrees.</summary>
        protected static Vector2 Rotate(Vector2 direction, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            return new Vector2(direction.x * cos - direction.y * sin, direction.x * sin + direction.y * cos);
        }

        /// <summary>Shows a small fading sprite (dust, spark, burst) at a position.</summary>
        protected void Puff(Sprite sprite, Vector2 position, float scale, float lifetime, float growth)
        {
            if (sprite == null) return;
            EffectFade.Spawn(sprite, SpriteMaterial, position, 0f, Vector2.one * scale, lifetime, growth, 9);
        }
    }
}
