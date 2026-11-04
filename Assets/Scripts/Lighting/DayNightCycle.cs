using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MobileRpg
{
    /// <summary>
    /// Drives the town's ambient light through a slow day, dusk, night and dawn cycle, and turns the street lamps up
    /// as it gets dark. While the player is in an arena the ambient light fades to that arena's own mood instead.
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        public Light2D globalLight;
        public ArenaRun run;

        [Tooltip("Real seconds for a whole day.")]
        public float cycleSeconds = 240f;

        [Tooltip("Where in the day the game starts (0 = dawn, 0.2 = noon, 0.7 = night).")]
        [Range(0f, 1f)]
        public float startTime = 0.1f;

        [Tooltip("Ambient color over the day.")]
        public Gradient colors;

        [Tooltip("Ambient intensity over the day.")]
        public AnimationCurve intensity;

        [Header("Street lamps")]
        public Light2D[] lamps;
        public float lampDayIntensity = 0.15f;
        public float lampNightIntensity = 1.1f;

        [Tooltip("Seconds to fade between the town's light and an arena's light.")]
        public float arenaFadeSeconds = 0.8f;

        private bool inArena;
        private float arenaBlend;
        private Color arenaColor = Color.white;
        private float arenaIntensity = 1f;
        /// <summary>True while the town is dark enough to count as night.</summary>
        public bool IsNight { get; private set; }

        private ArenaSite site;

        private void OnEnable()
        {
            run.Entered += OnEntered;
            run.Ended += OnEnded;
        }

        private void OnDisable()
        {
            run.Entered -= OnEntered;
            run.Ended -= OnEnded;
        }

        private void OnEntered(ArenaSite newSite)
        {
            inArena = true;
            this.site = newSite;
            arenaColor = newSite.ambientColor;
            arenaIntensity = newSite.ambientIntensity;
        }

        private void OnEnded() { inArena = false; }

        private void Update()
        {
            float time = (startTime + Time.time / cycleSeconds) % 1f;
            Color townColor = colors.Evaluate(time);
            float townIntensity = intensity.Evaluate(time);
            IsNight = townIntensity < 0.6f;

            if (inArena && site != null)
            {
                // Some arenas change their light while the player is in them.
                arenaColor = site.ambientColor;
                arenaIntensity = site.ambientIntensity;
            }

            arenaBlend = Mathf.MoveTowards(arenaBlend, inArena ? 1f : 0f, Time.deltaTime / arenaFadeSeconds);
            globalLight.color = Color.Lerp(townColor, arenaColor, arenaBlend);
            globalLight.intensity = Mathf.Lerp(townIntensity, arenaIntensity, arenaBlend);

            float dark = 1f - Mathf.InverseLerp(0.35f, 1f, townIntensity);
            float lampLevel = Mathf.Lerp(lampDayIntensity, lampNightIntensity, dark);
            for (int i = 0; i < lamps.Length; i++) lamps[i].intensity = lampLevel;
        }
    }
}
