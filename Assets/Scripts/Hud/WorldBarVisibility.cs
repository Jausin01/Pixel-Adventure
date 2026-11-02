using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Shows a world bar only while its meter is partly filled: hidden when full or empty.
    /// </summary>
    public class WorldBarVisibility : MonoBehaviour
    {
        [Tooltip("Component that implements IMeter (for example Health).")]
        public MonoBehaviour source;

        [Tooltip("Object holding the bar's sprites. It is switched on and off.")]
        public GameObject visual;

        private IMeter meter;

        private void Awake()
        {
            meter = (IMeter)source;
        }

        private void OnEnable()
        {
            meter.FractionChanged += Refresh;
        }

        private void OnDisable()
        {
            meter.FractionChanged -= Refresh;
        }

        private void Start()
        {
            Refresh();
        }

        private void Refresh()
        {
            float fraction = meter.Fraction;
            visual.SetActive(fraction > 0f && fraction < 1f);
        }
    }
}
