using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Moves the player's Rigidbody2D according to <see cref="PlayerMoveInput"/>.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerMoveInput))]
    public class PlayerMovement : MonoBehaviour
    {
        [Tooltip("Movement speed in world units per second.")]
        public float moveSpeed = 3.5f;

        private Rigidbody2D body;
        private PlayerMoveInput input;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            input = GetComponent<PlayerMoveInput>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            body.MovePosition(body.position + input.MoveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
