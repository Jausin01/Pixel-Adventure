using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Keeps an <see cref="AimRotation"/> pointed in the player's facing direction while not paused.
    /// </summary>
    [RequireComponent(typeof(AimRotation))]
    public class IdleFacingRotator : MonoBehaviour
    {
        public PlayerFacing facing;

        /// <summary>When true the rotation is left alone (used while a weapon is attacking).</summary>
        public bool Paused { get; set; }

        private AimRotation aim;

        private void Awake()
        {
            aim = GetComponent<AimRotation>();
        }

        private void Update()
        {
            if (Paused || facing == null) return;
            aim.Aim(facing.Vector);
        }
    }
}
