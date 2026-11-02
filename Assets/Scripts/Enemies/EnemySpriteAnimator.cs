using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Plays an enemy's frame animation. Walkers animate only while moving; flyers can animate all the time.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemySpriteAnimator : MonoBehaviour
    {
        public EnemyMovement movement;

        [Tooltip("Animation frames in order.")]
        public Sprite[] frames;

        public float framesPerSecond = 8f;

        [Tooltip("Keep animating while standing still (for example a flapping bat).")]
        public bool animateWhenIdle;

        private SpriteRenderer spriteRenderer;
        private float time;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!animateWhenIdle && !movement.IsMoving)
            {
                time = 0f;
                spriteRenderer.sprite = frames[0];
                return;
            }

            time += Time.deltaTime;
            spriteRenderer.sprite = frames[(int)(time * framesPerSecond) % frames.Length];
        }
    }
}
