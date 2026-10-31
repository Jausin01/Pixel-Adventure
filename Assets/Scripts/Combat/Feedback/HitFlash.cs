using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Tints a sprite for a short moment whenever the attached <see cref="Health"/> takes damage.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class HitFlash : MonoBehaviour
    {
        [Tooltip("Sprite to tint. Found automatically in children when left empty.")]
        public SpriteRenderer target;

        [Tooltip("Color shown while flashing.")]
        public Color flashColor = new Color(1f, 0.35f, 0.35f);

        [Tooltip("Flash duration in seconds.")]
        public float duration = 0.08f;

        private Health health;
        private Color baseColor;
        private Coroutine running;

        private void Awake()
        {
            health = GetComponent<Health>();
            if (target == null) target = GetComponentInChildren<SpriteRenderer>();
            baseColor = target.color;
        }

        private void OnEnable()
        {
            health.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            health.Damaged -= OnDamaged;
            if (running != null) StopCoroutine(running);
            running = null;
            target.color = baseColor;
        }

        private void OnDamaged(float amount, Vector2 source)
        {
            if (running != null) StopCoroutine(running);
            running = StartCoroutine(Flash());
        }

        private IEnumerator Flash()
        {
            target.color = flashColor;
            yield return new WaitForSeconds(duration);
            target.color = baseColor;
            running = null;
        }
    }
}
