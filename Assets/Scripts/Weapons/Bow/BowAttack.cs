using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Runs the bow's shooting sequence: nock, draw, release (spawning an arrow) and cooldown.
    /// Pressing attack again during the sequence queues another shot.
    /// Sprites, rotation, arrow creation and targeting are delegated to other components.
    /// </summary>
    public class BowAttack : MonoBehaviour, IWeapon
    {
        [Header("References")]
        [Tooltip("The shooter (usually the player). Used as the origin when looking for targets.")]
        public Transform owner;
        public PlayerFacing facing;
        public BowVisual visual;
        public AimRotation aim;
        public IdleFacingRotator idleRotator;
        public ArrowSpawner spawner;

        [Header("Aiming")]
        [Tooltip("How far from the owner a target can be to be auto-aimed.")]
        public float aimRange = 6f;
        [Tooltip("Height added to the target position so arrows aim at its body instead of its feet.")]
        public float aimHeight = 0.4f;

        [Header("Timing (seconds)")]
        public float nockTime = 0.06f;
        public float drawTime = 0.10f;
        public float releaseTime = 0.08f;
        public float cooldown = 0.12f;

        [Header("Feel")]
        [Tooltip("How far the bow is pushed back when the arrow is released.")]
        public float recoilDistance = 0.12f;

        private bool attacking;
        private bool buffered;

        /// <inheritdoc/>
        public bool IsBusy { get { return attacking; } }

        private void OnDisable()
        {
            attacking = false;
            buffered = false;
            if (idleRotator != null) idleRotator.Paused = false;
        }

        /// <inheritdoc/>
        public void RequestAttack()
        {
            if (attacking)
            {
                buffered = true;
                return;
            }
            StartCoroutine(ShootSequence());
        }

        private IEnumerator ShootSequence()
        {
            attacking = true;
            idleRotator.Paused = true;

            bool again;
            do
            {
                buffered = false;
                Transform target = TargetFinder.FindNearest(owner.position, aimRange);

                AimAt(target);
                visual.ShowNock();
                yield return new WaitForSeconds(nockTime);

                AimAt(target);
                visual.ShowDraw();
                yield return new WaitForSeconds(drawTime);

                Vector2 direction = AimAt(target);
                spawner.Fire(transform.position, direction);
                visual.ShowRelease();
                transform.position -= (Vector3)(direction * recoilDistance);
                yield return new WaitForSeconds(releaseTime);

                visual.ShowIdle();
                yield return new WaitForSeconds(cooldown);
                again = buffered;
            }
            while (again);

            idleRotator.Paused = false;
            attacking = false;
        }

        private Vector2 AimAt(Transform target)
        {
            Vector2 direction = facing.Vector;
            if (target != null)
            {
                Vector2 toTarget = (Vector2)target.position + Vector2.up * aimHeight - (Vector2)transform.position;
                if (toTarget.sqrMagnitude > 0.001f) direction = toTarget.normalized;
            }

            aim.Aim(direction);
            return direction;
        }
    }
}
