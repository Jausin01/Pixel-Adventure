using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Runs the spear's attack: pull back, then a fast thrust in a straight line that hits everything along its length.
    /// Pressing attack again during the sequence queues another thrust.
    /// Following, rotation, targeting and hit detection are delegated to other components.
    /// </summary>
    public class SpearAttack : MonoBehaviour, IWeapon
    {
        [Header("References")]
        [Tooltip("The attacker (usually the player). Used as the origin when looking for targets.")]
        public Transform owner;
        public PlayerFacing facing;
        public HoverFollower follower;
        public AimRotation aim;
        [Tooltip("Optional. Plays a short slow-motion effect when a thrust connects.")]
        public HitStop hitStop;

        [Header("Stats")]
        public float damage = 11f;
        [Tooltip("How far from the attacker the tip of the spear reaches.")]
        public float reach = 2.0f;
        [Tooltip("Distance from the middle of the spear to its tip.")]
        public float tipOffset = 0.55f;
        public float hitRadius = 0.3f;
        [Tooltip("How far from the owner a target can be to be auto-aimed.")]
        public float aimRange = 3.4f;
        [Tooltip("Height of the attacker's chest above its feet. Thrusts start from there.")]
        public float chestHeight = 0.5f;

        [Header("Timing (seconds)")]
        public float windupTime = 0.12f;
        public float thrustTime = 0.09f;
        public float recoverTime = 0.08f;
        [Tooltip("Extra pause after each thrust before the next one can start.")]
        public float attackCooldown = 0.28f;

        private readonly HitScanner scanner = new HitScanner();
        private bool attacking;
        private bool buffered;

        /// <inheritdoc/>
        public WeaponClass Class { get { return WeaponClass.Spear; } }

        /// <inheritdoc/>
        public float Damage { set { damage = value; } }

        /// <inheritdoc/>
        public bool Locked { get; set; }

        /// <inheritdoc/>
        public bool IsBusy { get { return attacking || Locked; } }

        private void OnEnable()
        {
            aim.Aim(Vector2.up);
        }

        private void OnDisable()
        {
            attacking = false;
            buffered = false;
            if (follower != null) follower.Paused = false;
        }

        /// <inheritdoc/>
        public void RequestAttack()
        {
            if (Locked) return;

            if (attacking)
            {
                buffered = true;
                return;
            }
            StartCoroutine(AttackSequence());
        }

        private IEnumerator AttackSequence()
        {
            attacking = true;
            follower.Paused = true;

            bool again;
            do
            {
                buffered = false;

                Vector2 origin = (Vector2)owner.position + Vector2.up * chestHeight;
                Transform target = TargetFinder.FindNearest(owner.position, aimRange, owner);
                Vector2 direction = target != null ? ((Vector2)target.position - origin).normalized : facing.Vector;
                if (direction.sqrMagnitude < 0.001f) direction = facing.Vector;

                Vector2 pulledBack = origin - direction * 0.35f;
                Vector2 thrustEnd = origin + direction * (reach - tipOffset);

                aim.Aim(direction);
                yield return Tween.MoveTo(transform, transform.position, pulledBack, windupTime, Easing.EaseOutQuad);

                scanner.Reset();
                yield return Tween.MoveTo(transform, pulledBack, thrustEnd, thrustTime, Easing.EaseInQuad, step => ScanForHits(direction));
                yield return new WaitForSeconds(recoverTime + attackCooldown);

                again = buffered;
            }
            while (again);

            aim.Aim(Vector2.up);
            follower.Paused = false;
            attacking = false;
        }

        // The tip and the middle of the shaft both hurt, so a fast thrust cannot skip over a target.
        private void ScanForHits(Vector2 direction)
        {
            Vector2 middle = transform.position;
            bool hit = scanner.Scan(middle + direction * tipOffset, hitRadius, damage, owner);
            hit |= scanner.Scan(middle, hitRadius, damage, owner);
            if (hit && hitStop != null) hitStop.Trigger();
        }
    }
}
