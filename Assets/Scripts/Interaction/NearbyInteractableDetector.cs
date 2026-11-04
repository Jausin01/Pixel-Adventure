using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Watches for <see cref="Interactable"/> objects close to this object and reports the nearest one.
    /// </summary>
    public class NearbyInteractableDetector : MonoBehaviour
    {
        [Tooltip("How far around the object interactables are detected.")]
        public float range = 0.3f;

        [Tooltip("Seconds between scans.")]
        public float scanInterval = 0.1f;

        /// <summary>The nearest interactable, or null when there is none.</summary>
        public Interactable Current { get; private set; }

        /// <summary>Raised when <see cref="Current"/> changes.</summary>
        public event Action<Interactable> CurrentChanged;

        private readonly Collider2D[] buffer = new Collider2D[16];
        private float timer;

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer > 0f) return;
            timer = scanInterval;

            Interactable nearest = FindNearest();
            if (nearest == Current) return;

            Current = nearest;
            if (CurrentChanged != null) CurrentChanged(nearest);
        }

        private Interactable FindNearest()
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.NoFilter(); // include trigger colliders, like the old call did
            int count = Physics2D.OverlapCircle(transform.position, range, filter, buffer);
            Interactable nearest = null;
            float best = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Interactable candidate = buffer[i].GetComponent<Interactable>();
                if (candidate == null) continue;

                float distance = ((Vector2)candidate.transform.position - (Vector2)transform.position).sqrMagnitude;
                if (distance < best)
                {
                    best = distance;
                    nearest = candidate;
                }
            }
            return nearest;
        }
    }
}
