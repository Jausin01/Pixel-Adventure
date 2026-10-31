using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the EventSystem with the input module that matches the active input backend.
    /// </summary>
    public static class EventSystemBuilder
    {
        /// <summary>Builds the event system.</summary>
        public static void Build()
        {
            GameObject go = new GameObject("EventSystem", typeof(EventSystem));
#if ENABLE_INPUT_SYSTEM
            UnityEngine.InputSystem.UI.InputSystemUIInputModule module = go.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            module.AssignDefaultActions();
#else
            go.AddComponent<StandaloneInputModule>();
#endif
        }
    }
}
