using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Base class for sword skills. Takes control of the sword while the skill runs (no normal attacks, no weapon swap,
    /// no following the player) and gives subclasses the common helpers: aiming, moving the sword and hitting targets.
    /// Subclasses implement only the movement in <see cref="Perform"/>.
    /// </summary>
    public abstract class SwordSkillBase : MonoBehaviour, ISkillExecutor
    {
        public Transform owner;
        public SwordAttack sword;
        public SwordVisual visual;
        public HoverFollower follower;
        public PlayerFacing facing;
        [Tooltip("Optional. Plays a short slow-motion effect when a hit connects.")]
        public HitStop hitStop;

        /// <inheritdoc/>
        public abstract SkillType Type { get; }

        /// <inheritdoc/>
        public void Execute(SkillDefinition skill, float damage)
        {
            StartCoroutine(Run(damage));
        }

        /// <summary>The skill's movement and hits. The sword is already under control when this starts.</summary>
        protected abstract IEnumerator Perform(float damage);

        private void OnDisable()
        {
            sword.Locked = false;
            follower.Paused = false;
        }

        private IEnumerator Run(float damage)
        {
            sword.Locked = true;
            follower.Paused = true;

            yield return Perform(damage);

            visual.ShowIdle();
            follower.Paused = false;
            sword.Locked = false;
        }

        /// <summary>Places the sword in the world, keeping its depth.</summary>
        protected void SetPosition(Vector2 position)
        {
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        /// <summary>Direction towards the nearest target within range, or the way the player faces.</summary>
        protected Vector2 AimDirection(float range)
        {
            Transform target = TargetFinder.FindNearest(owner.position, range, owner);
            Vector2 direction = target != null ? (Vector2)target.position - (Vector2)owner.position : facing.Vector;
            return direction.sqrMagnitude > 0.001f ? direction.normalized : facing.Vector;
        }

        /// <summary>Damages new targets inside a circle. Plays the hit stop when something is hit.</summary>
        protected bool Hit(HitScanner scanner, Vector2 center, float radius, float damage)
        {
            bool hit = scanner.Scan(center, radius, damage, owner);
            if (hit && hitStop != null) hitStop.Trigger();
            return hit;
        }
    }
}
