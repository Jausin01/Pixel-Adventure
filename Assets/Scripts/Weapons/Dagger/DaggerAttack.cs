using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Runs the twin daggers' attack: two very fast stabs in a row, one from each hand, short in reach.
    /// Pressing attack again during the sequence queues another pair.
    /// </summary>
    public class DaggerAttack : MonoBehaviour, IWeapon
    {
        [Header("References")]
        [Tooltip("The attacker (usually the player). Used as the origin when looking for targets.")]
        public Transform owner;
        public PlayerFacing facing;
        public HoverFollower follower;
        public AimRotation aim;
        [Tooltip("Optional. Plays a short slow-motion effect when a stab connects.")]
        public HitStop hitStop;

        [Header("Stats")]
        public float damage = 6f;
        [Tooltip("How far from the attacker the daggers reach.")]
        public float reach = 1.3f;
        public float hitRadius = 0.5f;
        [Tooltip("How far from the owner a target can be to be auto-aimed.")]
        public float aimRange = 2f;
        [Tooltip("Height of the attacker's chest above its feet. Stabs start from there.")]
        public float chestHeight = 0.5f;
        [Tooltip("How far above and below the middle line each hand stabs.")]
        public float handSpread = 0.3f;

        [Header("Timing (seconds)")]
        public float stabTime = 0.06f;
        public float betweenStabs = 0.05f;
        [Tooltip("Pause after the pair before the next one can start.")]
        public float attackCooldown = 0.12f;

        private readonly HitScanner scanner = new HitScanner();
        private bool attacking;
        private bool buffered;

        /// <inheritdoc/>
        public WeaponClass Class { get { return WeaponClass.Dagger; } }

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
                Vector2 side = new Vector2(-direction.y, direction.x);

                aim.Aim(direction);
                yield return Stab(origin, direction, side * handSpread);
                yield return new WaitForSeconds(betweenStabs);
                yield return Stab(origin, direction, -side * handSpread);
                yield return new WaitForSeconds(attackCooldown);

                again = buffered;
            }
            while (again);

            aim.Aim(Vector2.up);
            follower.Paused = false;
            attacking = false;
        }

        private IEnumerator Stab(Vector2 origin, Vector2 direction, Vector2 handOffset)
        {
            Vector2 start = origin + handOffset + direction * 0.2f;
            Vector2 end = origin + handOffset + direction * (reach - 0.4f);

            scanner.Reset();
            yield return Tween.MoveTo(transform, transform.position, start, stabTime * 0.5f, Easing.EaseOutQuad);
            yield return Tween.MoveTo(transform, start, end, stabTime, Easing.EaseInQuad, step => ScanForHits(direction));
        }

        private void ScanForHits(Vector2 direction)
        {
            Vector2 tip = (Vector2)transform.position + direction * 0.4f;
            if (scanner.Scan(tip, hitRadius, damage, owner) && hitStop != null) hitStop.Trigger();
        }
    }
}
