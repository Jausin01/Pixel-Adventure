using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Connects an <see cref="IMeter"/> (health, energy...) to the <see cref="IFillBar"/> on the same object,
    /// keeping the bar in sync with the value.
    /// </summary>
    public class MeterBarBinder : MonoBehaviour
    {
        [Tooltip("Component that implements IMeter (for example Health or Energy).")]
        public MonoBehaviour source;

        private IMeter meter;
        private IFillBar bar;

        private void Awake()
        {
            meter = (IMeter)source;
            bar = GetComponent<IFillBar>();
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
            bar.SetFraction(meter.Fraction);
        }
    }
}
