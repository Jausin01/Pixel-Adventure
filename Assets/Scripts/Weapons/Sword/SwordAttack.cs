using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Runs the sword's attack sequence: wind-up, strike and recovery, looping through the combo steps.
    /// Pressing attack again while a strike is running queues the next step.
    /// Following, sprites, targeting and hit detection are delegated to other components.
    /// </summary>
    public class SwordAttack : MonoBehaviour, IWeapon
    {
        [Header("References")]
        [Tooltip("The attacker (usually the player). Used as the origin when looking for targets.")]
        public Transform owner;
        public PlayerFacing facing;
        public SwordVisual visual;
        public HoverFollower follower;
        [Tooltip("Optional. Plays a short slow-motion effect when a strike connects.")]
        public HitStop hitStop;

        [Header("Stats")]
        public float damage = 10f;
        public float hitRadius = 0.6f;
        [Tooltip("How far from the owner a target can be to be auto-aimed.")]
        public float aimRange = 3.5f;
        [Tooltip("Seconds without attacking before the combo restarts.")]
        public float comboResetTime = 1f;

        [Header("Timing (seconds)")]
        public float windupTime = 0.12f;
        public float strikeTime = 0.10f;
        public float recoverTime = 0.06f;

        private readonly HitScanner scanner = new HitScanner();
        private ComboCounter combo;
        private bool attacking;
        private bool buffered;

        /// <inheritdoc/>
        public bool IsBusy { get { return attacking; } }

        private void Awake()
        {
            combo = new ComboCounter(SwordComboPattern.StepCount, comboResetTime);
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

                Vector2 ownerPosition = owner.position;
                Transform target = TargetFinder.FindNearest(ownerPosition, aimRange);
                Vector2 forward = target != null ? ((Vector2)target.position - ownerPosition).normalized : facing.Vector;
                if (forward.sqrMagnitude < 0.001f) forward = facing.Vector;
                Vector2 center = target != null ? (Vector2)target.position : ownerPosition + forward * 1.6f;

                Vector2 start;
                Vector2 end;
                SwordComboPattern.GetPath(combo.Peek(Time.time), center, forward, out start, out end);
                Vector2 direction = (end - start).normalized;

                yield return Windup(start, direction);
                yield return Strike(start, end, direction);
                yield return new WaitForSeconds(recoverTime);

                combo.Complete(Time.time);
                again = buffered;
            }
            while (again);

            visual.ShowIdle();
            follower.Paused = false;
            attacking = false;
        }

        private IEnumerator Windup(Vector2 start, Vector2 direction)
        {
            visual.Show(direction, false);
            yield return Tween.MoveTo(transform, transform.position, start, windupTime, Easing.EaseOutQuad);
        }

        private IEnumerator Strike(Vector2 start, Vector2 end, Vector2 direction)
        {
            scanner.Reset();
            visual.Show(direction, true);
            yield return Tween.MoveTo(transform, start, end, strikeTime, Easing.EaseInQuad, ScanForHits);
        }

        private void ScanForHits(float progress)
        {
            bool hit = scanner.Scan(transform.position, hitRadius, damage);
            if (hit && hitStop != null) hitStop.Trigger();
        }
    }
}
