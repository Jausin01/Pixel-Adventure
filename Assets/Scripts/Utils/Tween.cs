using System;
using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Small coroutine-based tweening helpers. Yield the returned enumerator from a coroutine to wait for it.
    /// </summary>
    public static class Tween
    {
        /// <summary>
        /// Moves a transform in the XY plane from one point to another over a duration.
        /// </summary>
        /// <param name="target">The transform to move. Its Z position is preserved.</param>
        /// <param name="from">Start position.</param>
        /// <param name="to">End position.</param>
        /// <param name="duration">Duration in seconds (scaled time).</param>
        /// <param name="ease">Easing function applied to the normalized time.</param>
        /// <param name="onStep">Optional callback invoked every step with the eased progress in [0, 1].</param>
        public static IEnumerator MoveTo(Transform target, Vector2 from, Vector2 to, float duration,
                                         Func<float, float> ease, Action<float> onStep = null)
        {
            float z = target.position.z;
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float progress = ease(t / duration);
                Vector2 p = Vector2.Lerp(from, to, progress);
                target.position = new Vector3(p.x, p.y, z);
                if (onStep != null) onStep(progress);
                yield return null;
            }

            target.position = new Vector3(to.x, to.y, z);
            if (onStep != null) onStep(1f);
        }
    }
}
