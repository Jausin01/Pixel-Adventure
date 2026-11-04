using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MobileRpg
{
    /// <summary>
    /// Makes a 2D light waver like a flame, around the intensity it has when the scene starts.
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    public class LightFlicker : MonoBehaviour
    {
        [Tooltip("How far the intensity strays from its base, as a fraction (0.2 = plus or minus 20%).")]
        public float amount = 0.2f;

        [Tooltip("How fast the flicker changes.")]
        public float speed = 6f;

        private Light2D lamp;
        private float baseIntensity;
        private float offset;

        private void Awake()
        {
            lamp = GetComponent<Light2D>();
            baseIntensity = lamp.intensity;
            offset = Random.value * 100f;
        }

        private void Update()
        {
            float noise = Mathf.PerlinNoise(offset, Time.time * speed) - 0.5f;
            lamp.intensity = baseIntensity * (1f + noise * 2f * amount);
        }
    }
}
