using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace MobileRpg
{
    /// <summary>
    /// Desktop testing helper: Space or J attacks, Q or Tab swaps weapons.
    /// </summary>
    public class WeaponKeyboardInput : MonoBehaviour
    {
        public WeaponHolder holder;

        private void Update()
        {
            if (AttackPressed()) holder.Attack();
            if (SwapPressed()) holder.SwapNext();
        }

        private static bool AttackPressed()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard k = Keyboard.current;
            return k != null && (k.spaceKey.wasPressedThisFrame || k.jKey.wasPressedThisFrame);
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J);
#else
            return false;
#endif
        }

        private static bool SwapPressed()
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard k = Keyboard.current;
            return k != null && (k.qKey.wasPressedThisFrame || k.tabKey.wasPressedThisFrame);
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Tab);
#else
            return false;
#endif
        }
    }
}
