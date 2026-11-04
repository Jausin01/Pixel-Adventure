using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace MobileRpg
{
    /// <summary>
    /// Desktop testing helper: Space or J performs the main action (attack or tool), K throws the secondary weapon, L uses the quick item.
    /// </summary>
    public class WeaponKeyboardInput : MonoBehaviour
    {
        public ActionRouter router;
        public SecondaryThrower thrower;
        public QuickItem quick;

        private void Update()
        {
            if (AttackPressed()) router.Perform();
            if (KeyPressed(KeyboardKey.Secondary)) thrower.Throw();
            if (KeyPressed(KeyboardKey.Quick)) quick.Use();
        }

        private enum KeyboardKey { Secondary, Quick }

        private static bool KeyPressed(KeyboardKey key)
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard k = Keyboard.current;
            if (k == null) return false;
            return key == KeyboardKey.Secondary ? k.kKey.wasPressedThisFrame : k.lKey.wasPressedThisFrame;
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(key == KeyboardKey.Secondary ? KeyCode.K : KeyCode.L);
#else
            return false;
#endif
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
    }
}
