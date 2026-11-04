using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Runs the axe's attack: a slow raise over the shoulder, then a heavy overhead swing along an arc around the
    /// attacker that hits everything it passes. Slow but strong.
    /// Pressing attack again during the sequence queues another swing.
    /// </summary>
    public class AxeAttack : MonoBehaviour, IWeapon
    {
        [Header("References")]
        [Tooltip("The attacker (usually the player). Used as the origin when looking for targets.")]
        public Transform owner;
        public PlayerFacing facing;
        public HoverFollower follower;
        public AimRotation aim;
        [Tooltip("Optional. Plays a short slow-motion effect when a swing connects.")]
        public HitStop hitStop;

        [Header("Stats")]
        public float damage = 15f;
        [Tooltip("Distance from the attacker's chest to the middle of the axe while swinging.")]
        public float swingRadius = 1.0f;
        [Tooltip("Distance from the middle of the axe to its blade.")]
        public float headOffset = 0.35f;
        public float hitRadius = 0.75f;
        [Tooltip("How far from the owner a target can be to be auto-aimed.")]
        public float aimRange = 2.4f;
        [Tooltip("Height of the attacker's chest above its feet. Swings are centered there.")]
        public float chestHeight = 0.5f;

        [Header("Swing (degrees from the attack direction)")]
        public float raisedAngle = 105f;
        public float finishAngle = -30f;

        [Header("Timing (seconds)")]
        public float windupTime = 0.26f;
        public float swingTime = 0.12f;
        public float recoverTime = 0.14f;
        [Tooltip("Extra pause after each swing before the next one can start.")]
        public float attackCooldown = 0.4f;

        private readonly HitScanner scanner = new HitScanner();
        private bool attacking;
        private bool buffered;

        /// <inheritdoc/>
        public WeaponClass Class { get { return WeaponClass.Axe; } }

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

                Vector2 chest = (Vector2)owner.position + Vector2.up * chestHeight;
                Transform target = TargetFinder.FindNearest(owner.position, aimRange, owner);
                Vector2 direction = target != null ? ((Vector2)target.position - chest).normalized : facing.Vector;
                if (direction.sqrMagnitude < 0.001f) direction = facing.Vector;

                // The axe comes down from above on the side the attacker is facing.
                float side = direction.x >= 0f ? 1f : -1f;
                float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float raised = baseAngle + side * raisedAngle;
                float finish = baseAngle + side * finishAngle;

                yield return Raise(chest, raised);
                yield return Swing(chest, raised, finish);
                yield return new WaitForSeconds(recoverTime + attackCooldown);

                again = buffered;
            }
            while (again);

            aim.Aim(Vector2.up);
            follower.Paused = false;
            attacking = false;
        }

        // Lifts the axe from where it hovers to the raised position, turning it to match.
        private IEnumerator Raise(Vector2 chest, float raised)
        {
            Vector2 from = transform.position;
            Vector2 to = PointOnArc(chest, raised);
            float fromAngle = transform.eulerAngles.z;

            for (float t = 0f; t < windupTime; t += Time.deltaTime)
            {
                float progress = Easing.EaseOutQuad(t / windupTime);
                Place(Vector2.Lerp(from, to, progress), Mathf.LerpAngle(fromAngle, raised, progress));
                yield return null;
            }
            Place(to, raised);
        }

        private IEnumerator Swing(Vector2 chest, float raised, float finish)
        {
            scanner.Reset();
            for (float t = 0f; t < swingTime; t += Time.deltaTime)
            {
                float angle = Mathf.Lerp(raised, finish, Easing.EaseInQuad(t / swingTime));
                Place(PointOnArc(chest, angle), angle);
                ScanForHits(angle);
                yield return null;
            }

            Place(PointOnArc(chest, finish), finish);
            ScanForHits(finish);
        }

        private Vector2 PointOnArc(Vector2 chest, float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            return chest + new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * swingRadius;
        }

        private void Place(Vector2 position, float angle)
        {
            transform.position = new Vector3(position.x, position.y, transform.position.z);
            aim.Aim(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)));
        }

        private void ScanForHits(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            Vector2 head = (Vector2)transform.position + new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * headOffset;
            if (scanner.Scan(head, hitRadius, damage, owner) && hitStop != null) hitStop.Trigger();
        }
    }
}
