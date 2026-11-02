using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Moves an enemy's body in the direction set by its brain, plus any knockback push.
    /// Decides nothing by itself.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        [Tooltip("Walking speed in world units per second.")]
        public float moveSpeed = 2f;

        /// <summary>Direction to walk in (unit vector, or zero to stand still). Set by the brain.</summary>
        public Vector2 Direction { get; set; }

        /// <summary>True while the enemy is asked to walk.</summary>
        public bool IsMoving { get { return Direction.sqrMagnitude > 0.01f; } }

        private Rigidbody2D body;
        private Knockback knockback;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            knockback = GetComponent<Knockback>();
        }

        private void FixedUpdate()
        {
            Vector2 velocity = Direction * moveSpeed;
            if (knockback != null) velocity += knockback.Velocity;
            if (velocity.sqrMagnitude < 0.0001f) return;

            body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
        }
    }
}
