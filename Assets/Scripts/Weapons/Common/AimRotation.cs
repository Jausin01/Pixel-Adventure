using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Rotates this object around Z so its right side points along a given direction.
    /// </summary>
    public class AimRotation : MonoBehaviour
    {
        /// <summary>Points the object's right side along <paramref name="direction"/>.</summary>
        public void Aim(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
