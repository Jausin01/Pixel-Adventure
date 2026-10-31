using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Plays the walk cycle that matches the player's facing direction, and shows the
    /// first frame of that cycle as the standing pose when the player is not moving.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerSpriteAnimator : MonoBehaviour
    {
        public PlayerMoveInput input;
        public PlayerFacing facing;

        [Tooltip("Walk animation speed in frames per second.")]
        public float framesPerSecond = 8f;

        public Sprite[] walkDown;
        public Sprite[] walkUp;
        public Sprite[] walkLeft;
        public Sprite[] walkRight;

        private SpriteRenderer spriteRenderer;
        private float frameTimer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            Sprite[] frames = FramesFor(facing.Current);
            if (frames == null || frames.Length == 0) return;

            if (input.IsMoving)
            {
                frameTimer += Time.deltaTime * framesPerSecond;
                spriteRenderer.sprite = frames[(int)frameTimer % frames.Length];
            }
            else
            {
                frameTimer = 0f;
                spriteRenderer.sprite = frames[0];
            }
        }

        private Sprite[] FramesFor(FacingDirection direction)
        {
            switch (direction)
            {
                case FacingDirection.Up: return walkUp;
                case FacingDirection.Left: return walkLeft;
                case FacingDirection.Right: return walkRight;
                default: return walkDown;
            }
        }
    }
}
