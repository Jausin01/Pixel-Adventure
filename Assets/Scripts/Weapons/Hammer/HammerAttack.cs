using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Runs the hammer's attack: lifts it high over a spot in front of the attacker (or over the nearest enemy), then
    /// slams it down. The impact hurts everything in a circle around the spot and raises a puff of dust.
    /// Slowest weapon, but it hits everything around the impact.
    /// </summary>
    public class HammerAttack : MonoBehaviour, IWeapon
    {
        [Header("References")]
        [Tooltip("The attacker (usually the player). Used as the origin when looking for targets.")]
        public Transform owner;
        public PlayerFacing facing;
        public HoverFollower follower;
        public AimRotation aim;
        [Tooltip("Optional. Plays a short slow-motion effect when a slam connects.")]
        public HitStop hitStop;
        [Tooltip("Optional. Dust raised by the impact.")]
        public Sprite dustSprite;

        [Header("Stats")]
        public float damage = 20f;
        [Tooltip("Radius of the circle hurt by the impact.")]
        public float impactRadius = 1.1f;
        [Tooltip("How far in front of the attacker the slam lands when there is no target.")]
        public float slamDistance = 1.4f;
        [Tooltip("How far from the owner a target can be to be auto-aimed.")]
        public float aimRange = 2.4f;
        [Tooltip("How high above the impact the hammer is lifted.")]
        public float liftHeight = 1.4f;

        [Header("Timing (seconds)")]
        public float windupTime = 0.34f;
        public float slamTime = 0.08f;
        public float recoverTime = 0.18f;
        [Tooltip("Extra pause after each slam before the next one can start.")]
        public float attackCooldown = 0.45f;

        private readonly HitScanner scanner = new HitScanner();
        private bool attacking;
        private bool buffered;

        /// <inheritdoc/>
        public WeaponClass Class { get { return WeaponClass.Hammer; } }

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

                Transform target = TargetFinder.FindNearest(owner.position, aimRange, owner);
                Vector2 impact = target != null ? (Vector2)target.position : (Vector2)owner.position + facing.Vector * slamDistance;

                yield return Lift(impact + Vector2.up * liftHeight);
                yield return Slam(impact);
                yield return new WaitForSeconds(recoverTime + attackCooldown);

                again = buffered;
            }
            while (again);

            aim.Aim(Vector2.up);
            follower.Paused = false;
            attacking = false;
        }

        // Raises the hammer over the impact point with its head up.
        private IEnumerator Lift(Vector2 top)
        {
            Vector2 from = transform.position;
            float fromAngle = transform.eulerAngles.z;

            for (float t = 0f; t < windupTime; t += Time.deltaTime)
            {
                float progress = Easing.EaseOutQuad(t / windupTime);
                Place(Vector2.Lerp(from, top, progress), Mathf.LerpAngle(fromAngle, 90f, progress));
                yield return null;
            }
            Place(top, 90f);
        }

        // Brings the head down onto the impact point and hurts everything around it.
        private IEnumerator Slam(Vector2 impact)
        {
            Vector2 top = transform.position;
            Vector2 resting = impact + Vector2.up * 0.35f;

            for (float t = 0f; t < slamTime; t += Time.deltaTime)
            {
                float progress = Easing.EaseInQuad(t / slamTime);
                Place(Vector2.Lerp(top, resting, progress), Mathf.Lerp(90f, -80f, progress));
                yield return null;
            }
            Place(resting, -80f);

            scanner.Reset();
            if (scanner.Scan(impact, impactRadius, damage, owner) && hitStop != null) hitStop.Trigger();
            RaiseDust(impact);
        }

        private void Place(Vector2 position, float angle)
        {
            transform.position = new Vector3(position.x, position.y, transform.position.z);
            aim.Aim(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)));
        }

        private void RaiseDust(Vector2 center)
        {
            if (dustSprite == null) return;

            Material material = GetComponent<SpriteRenderer>().sharedMaterial;
            for (int i = 0; i < 8; i++)
            {
                float angle = i * Mathf.PI * 2f / 8f;
                Vector2 position = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle) * 0.6f) * impactRadius * 0.8f;
                EffectFade.Spawn(dustSprite, material, position, 0f, Vector2.one, 0.35f, 1.6f, 8);
            }
        }
    }
}
