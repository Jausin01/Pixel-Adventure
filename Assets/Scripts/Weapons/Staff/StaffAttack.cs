using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Runs the staff's attack: points at the nearest enemy and fires a magic bolt from its tip.
    /// Pressing attack again during the sequence queues another bolt.
    /// </summary>
    public class StaffAttack : MonoBehaviour, IWeapon
    {
        [Header("References")]
        [Tooltip("The caster (usually the player). Used as the origin when looking for targets.")]
        public Transform owner;
        public PlayerFacing facing;
        public HoverFollower follower;
        public AimRotation aim;
        public Sprite boltSprite;

        [Header("Stats")]
        public float damage = 9f;
        public float boltSpeed = 10f;
        public float boltLifetime = 1.3f;
        public float boltRadius = 0.25f;
        [Tooltip("How far from the owner a target can be to be auto-aimed.")]
        public float aimRange = 6.5f;
        [Tooltip("Distance from the middle of the staff to its tip, where bolts appear.")]
        public float tipOffset = 0.6f;
        [Tooltip("How far the staff is pushed back when it fires.")]
        public float recoilDistance = 0.15f;

        [Header("Timing (seconds)")]
        public float castTime = 0.14f;
        [Tooltip("Pause after each bolt before the next one can start.")]
        public float attackCooldown = 0.32f;

        private bool attacking;
        private bool buffered;

        /// <inheritdoc/>
        public WeaponClass Class { get { return WeaponClass.Staff; } }

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
            StartCoroutine(CastSequence());
        }

        private IEnumerator CastSequence()
        {
            attacking = true;
            follower.Paused = true;

            bool again;
            do
            {
                buffered = false;

                Vector2 origin = (Vector2)owner.position + Vector2.up * 0.5f;
                Transform target = TargetFinder.FindNearest(owner.position, aimRange, owner);
                Vector2 direction = target != null ? ((Vector2)target.position + Vector2.up * 0.3f - origin).normalized : facing.Vector;
                if (direction.sqrMagnitude < 0.001f) direction = facing.Vector;

                aim.Aim(direction);
                Vector2 raised = origin + direction * 0.7f;
                yield return Tween.MoveTo(transform, transform.position, raised, castTime, Easing.EaseOutQuad);

                FireBolt(direction);
                transform.position -= (Vector3)(direction * recoilDistance);
                yield return new WaitForSeconds(attackCooldown);

                again = buffered;
            }
            while (again);

            aim.Aim(Vector2.up);
            follower.Paused = false;
            attacking = false;
        }

        private void FireBolt(Vector2 direction)
        {
            GameObject bolt = new GameObject("Magic Bolt", typeof(SpriteRenderer), typeof(ProjectileMotion), typeof(ProjectileImpact));
            SpriteRenderer source = GetComponent<SpriteRenderer>();

            SpriteRenderer sprite = bolt.GetComponent<SpriteRenderer>();
            sprite.sprite = boltSprite;
            sprite.sharedMaterial = source.sharedMaterial;
            sprite.sortingOrder = 9;

            Vector2 position = (Vector2)transform.position + direction * tipOffset;
            bolt.transform.position = new Vector3(position.x, position.y, 0f);
            bolt.GetComponent<ProjectileImpact>().Configure(damage, boltRadius, owner);
            bolt.GetComponent<ProjectileMotion>().Launch(direction, boltSpeed, boltLifetime);
        }
    }
}
