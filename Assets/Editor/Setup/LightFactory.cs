using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates 2D lights: the ambient (global) light of a scene and point lights for lamps, fires and glowing things.
    /// </summary>
    public static class LightFactory
    {
        /// <summary>Creates the scene-wide ambient light.</summary>
        public static Light2D Global(string name, Color color, float intensity)
        {
            GameObject go = new GameObject(name);
            Light2D light = go.AddComponent<Light2D>();
            light.lightType = Light2D.LightType.Global;
            light.color = color;
            light.intensity = intensity;
            return light;
        }

        /// <summary>Creates a point light as a child of an object.</summary>
        /// <param name="flicker">Fraction the intensity wavers by (0 for a steady light).</param>
        public static Light2D Point(string name, Transform parent, Vector2 localPosition, Color color, float intensity, float radius, float flicker)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;

            Light2D light = go.AddComponent<Light2D>();
            light.lightType = Light2D.LightType.Point;
            light.color = color;
            light.intensity = intensity;
            light.pointLightInnerRadius = radius * 0.15f;
            light.pointLightOuterRadius = radius;

            if (flicker > 0f) go.AddComponent<LightFlicker>().amount = flicker;
            return light;
        }
    }
}
