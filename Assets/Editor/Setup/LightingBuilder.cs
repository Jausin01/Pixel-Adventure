using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Sets up the lighting of the town scene: the day and night cycle and the light that follows the player.
    /// Arena scenes only describe their mood (see ArenaSite); the cycle blends to it while a run is active.
    /// </summary>
    public static class LightingBuilder
    {
        /// <summary>Creates the ambient light and the cycle that drives it and the street lamps.</summary>
        public static void BuildTown(Light2D[] lamps, ArenaRun run)
        {
            Light2D ambient = LightFactory.Global("Ambient Light", Color.white, 1f);

            DayNightCycle cycle = ambient.gameObject.AddComponent<DayNightCycle>();
            cycle.globalLight = ambient;
            cycle.run = run;
            cycle.lamps = lamps;
            cycle.colors = DayColors();
            cycle.intensity = new AnimationCurve(
                new Keyframe(0f, 0.7f), new Keyframe(0.2f, 1f), new Keyframe(0.45f, 1f), new Keyframe(0.55f, 0.8f),
                new Keyframe(0.7f, 0.38f), new Keyframe(0.9f, 0.38f), new Keyframe(1f, 0.7f));
        }

        /// <summary>Gives the player a soft warm light so the surroundings stay readable in dark places.</summary>
        public static void AttachPlayerLight(GameObject player)
        {
            LightFactory.Point("Player Light", player.transform, new Vector2(0f, 0.5f), new Color(1f, 0.93f, 0.8f), 0.75f, 6f, 0f);
        }

        private static Gradient DayColors()
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(new Color(1f, 0.84f, 0.67f), 0f),
                    new GradientColorKey(new Color(1f, 1f, 0.96f), 0.2f),
                    new GradientColorKey(new Color(1f, 1f, 0.94f), 0.45f),
                    new GradientColorKey(new Color(1f, 0.67f, 0.43f), 0.55f),
                    new GradientColorKey(new Color(0.47f, 0.55f, 0.9f), 0.7f),
                    new GradientColorKey(new Color(0.43f, 0.51f, 0.86f), 0.9f),
                    new GradientColorKey(new Color(1f, 0.84f, 0.67f), 1f)
                },
                new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });
            return gradient;
        }
    }
}
