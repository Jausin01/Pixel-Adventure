using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Spins an object around Z at a constant speed (a thrown shuriken or boomerang).
    /// </summary>
    public class Spinner : MonoBehaviour
    {
        [Tooltip("Degrees per second. Negative spins clockwise.")]
        public float degreesPerSecond = 900f;

        private void Update()
        {
            transform.Rotate(0f, 0f, degreesPerSecond * Time.deltaTime);
        }
    }
}
