using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace MobileRpg
{
    /// <summary>
    /// Produces the player's movement intent every frame from the on-screen joystick and,
    /// for desktop testing, the keyboard (WASD or arrow keys). It does not move anything by itself.
    /// </summary>
    public class PlayerMoveInput : MonoBehaviour
    {
        [Tooltip("On-screen joystick. Optional: the keyboard works without it.")]
        public VirtualJoystick joystick;

        /// <summary>Movement intent with a magnitude between 0 and 1.</summary>
        public Vector2 MoveInput { get; private set; }

        /// <summary>True while the player wants to move.</summary>
        public bool IsMoving { get { return MoveInput.sqrMagnitude > 0.01f; } }

        private void Update()
        {
            Vector2 v = ReadKeyboard();
            if (joystick != null) v += joystick.Value;
            if (v.sqrMagnitude > 1f) v.Normalize();
            MoveInput = v;
        }

        private static Vector2 ReadKeyboard()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard k = Keyboard.current;
            if (k == null) return Vector2.zero;
            float x = ((k.dKey.isPressed || k.rightArrowKey.isPressed) ? 1f : 0f)
                    - ((k.aKey.isPressed || k.leftArrowKey.isPressed) ? 1f : 0f);
            float y = ((k.wKey.isPressed || k.upArrowKey.isPressed) ? 1f : 0f)
                    - ((k.sKey.isPressed || k.downArrowKey.isPressed) ? 1f : 0f);
            return new Vector2(x, y);
#elif ENABLE_LEGACY_INPUT_MANAGER
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#else
            return Vector2.zero;
#endif
        }
    }
}
