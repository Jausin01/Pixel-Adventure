using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Plays a footstep every fixed distance walked. Grass in town and in field arenas, stone elsewhere.
    /// </summary>
    public class FootstepSfx : MonoBehaviour
    {
        public ArenaRun run;

        [Tooltip("Distance walked between two steps, in world units.")]
        public float stride = 0.8f;

        private Vector2 last;
        private float walked;

        private void OnEnable()
        {
            last = transform.position;
            walked = 0f;
        }

        private void Update()
        {
            Vector2 now = transform.position;
            float moved = Vector2.Distance(now, last);
            last = now;

            if (moved > 1.5f)
            {
                walked = 0f; // teleport (entering or leaving an arena)
                return;
            }

            walked += moved;
            if (walked < stride) return;
            walked = 0f;
            Sfx.Play(OnGrass() ? "step_grass" : "step_stone");
        }

        private bool OnGrass()
        {
            if (run == null || run.Current == null) return true;
            string theme = run.Current.arena.theme;
            return theme == "cornfield" || theme == "farm";
        }
    }
}
