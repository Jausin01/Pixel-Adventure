using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Remembers which of the four directions the player last moved towards.
    /// </summary>
    [RequireComponent(typeof(PlayerMoveInput))]
    public class PlayerFacing : MonoBehaviour
    {
        private PlayerMoveInput input;

        /// <summary>The direction the player is currently facing.</summary>
        public FacingDirection Current { get; private set; }

        /// <summary>The facing direction as a unit vector.</summary>
        public Vector2 Vector
        {
            get
            {
                switch (Current)
                {
                    case FacingDirection.Up: return Vector2.up;
                    case FacingDirection.Left: return Vector2.left;
                    case FacingDirection.Right: return Vector2.right;
                    default: return Vector2.down;
                }
            }
        }

        private void Awake()
        {
            input = GetComponent<PlayerMoveInput>();
            Current = FacingDirection.Down;
        }

        private void Update()
        {
            if (!input.IsMoving) return;

            Vector2 v = input.MoveInput;
            if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
                Current = v.x > 0f ? FacingDirection.Right : FacingDirection.Left;
            else
                Current = v.y > 0f ? FacingDirection.Up : FacingDirection.Down;
        }
    }
}
