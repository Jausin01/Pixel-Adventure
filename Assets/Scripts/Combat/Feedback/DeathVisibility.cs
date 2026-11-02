using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Hides the sprite and disables the colliders while the attached <see cref="Health"/> is dead,
    /// and restores them when it is revived.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class DeathVisibility : MonoBehaviour
    {
        [Tooltip("Sprites to hide. The object's own sprite is used when left empty.")]
        public SpriteRenderer[] renderers;

        [Tooltip("Colliders to disable while dead. Found automatically in children when left empty.")]
        public Collider2D[] colliders;

        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
            if (renderers == null || renderers.Length == 0) renderers = new[] { GetComponent<SpriteRenderer>() };
            if (colliders == null || colliders.Length == 0) colliders = GetComponentsInChildren<Collider2D>();
        }

        private void OnEnable()
        {
            health.Died += OnDied;
            health.Revived += OnRevived;
        }

        private void OnDisable()
        {
            health.Died -= OnDied;
            health.Revived -= OnRevived;
        }

        private void OnDied()
        {
            SetVisible(false);
        }

        private void OnRevived()
        {
            SetVisible(true);
        }

        private void SetVisible(bool visible)
        {
            for (int i = 0; i < renderers.Length; i++) renderers[i].enabled = visible;
            for (int i = 0; i < colliders.Length; i++) colliders[i].enabled = visible;
        }
    }
}
