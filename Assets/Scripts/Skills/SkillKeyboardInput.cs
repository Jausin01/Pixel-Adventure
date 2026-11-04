using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace MobileRpg
{
    /// <summary>
    /// Desktop testing helper: keys 1 to 4 cast the skills in the four slots.
    /// </summary>
    public class SkillKeyboardInput : MonoBehaviour
    {
        public SkillCaster caster;

        private void Update()
        {
            for (int i = 0; i < SkillLoadout.SlotCount; i++)
            {
                if (Pressed(i)) caster.TryCast(i);
            }
        }

        private static bool Pressed(int index)
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard k = Keyboard.current;
            if (k == null) return false;
            switch (index)
            {
                case 0: return k.digit1Key.wasPressedThisFrame;
                case 1: return k.digit2Key.wasPressedThisFrame;
                case 2: return k.digit3Key.wasPressedThisFrame;
                default: return k.digit4Key.wasPressedThisFrame;
            }
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Alpha1 + index);
#else
            return false;
#endif
        }
    }
}
