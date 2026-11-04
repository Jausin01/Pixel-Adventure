using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the desktop keyboard shortcuts used for testing.
    /// </summary>
    public static class KeyboardInputBuilder
    {
        /// <summary>Builds the keyboard input object.</summary>
        public static void Build(ActionRouter router, GameObject player)
        {
            GameObject go = new GameObject("Keyboard Input");
            WeaponKeyboardInput keyboard = go.AddComponent<WeaponKeyboardInput>();
            keyboard.router = router;
            keyboard.thrower = player.GetComponent<SecondaryThrower>();
            keyboard.quick = player.GetComponent<QuickItem>();
        }
    }
}
