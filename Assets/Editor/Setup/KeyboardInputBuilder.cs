using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the desktop keyboard shortcuts used for testing.
    /// </summary>
    public static class KeyboardInputBuilder
    {
        /// <summary>Builds the keyboard input object.</summary>
        public static void Build(WeaponHolder weapons, ActionRouter router)
        {
            GameObject go = new GameObject("Keyboard Input");
            WeaponKeyboardInput keyboard = go.AddComponent<WeaponKeyboardInput>();
            keyboard.holder = weapons;
            keyboard.router = router;
        }
    }
}
