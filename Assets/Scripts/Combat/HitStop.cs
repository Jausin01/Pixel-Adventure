using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Briefly slows down time to give hits more impact ("hit stop" / "hit freeze").
    /// </summary>
    public class HitStop : MonoBehaviour
    {
        [Tooltip("Time scale used while the effect is active.")]
        public float slowScale = 0.05f;

        [Tooltip("Real-time duration of the effect in seconds.")]
        public float duration = 0.04f;

        private Coroutine running;

        /// <summary>Starts the effect. Ignored if it is already playing.</summary>
        public void Trigger()
        {
            if (running != null) return;
            running = StartCoroutine(Freeze());
        }

        private IEnumerator Freeze()
        {
            Time.timeScale = slowScale;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
            running = null;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
            running = null;
        }
    }
}
