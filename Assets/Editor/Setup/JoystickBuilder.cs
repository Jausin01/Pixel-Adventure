using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the floating virtual joystick on the left half of the screen.
    /// </summary>
    public static class JoystickBuilder
    {
        /// <summary>Builds the touch zone and its visual.</summary>
        public static VirtualJoystick Build(Transform canvas)
        {
            GameObject zoneObject = new GameObject("Joystick Zone", typeof(RectTransform), typeof(Image));
            zoneObject.transform.SetParent(canvas, false);

            RectTransform zone = (RectTransform)zoneObject.transform;
            zone.anchorMin = Vector2.zero;
            zone.anchorMax = new Vector2(0.5f, 1f);
            zone.offsetMin = Vector2.zero;
            zone.offsetMax = Vector2.zero;

            // Nearly transparent but still raycastable so it receives touches.
            zoneObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.004f);

            VirtualJoystick joystick = zoneObject.AddComponent<VirtualJoystick>();

            Image joystickBase = UiFactory.CreateImage("Base", zone, SpriteLoader.Load("UI/joystick_base"), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(240f, 240f));
            Image knob = UiFactory.CreateImage("Knob", joystickBase.transform, SpriteLoader.Load("UI/joystick_knob"), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(110f, 110f));
            joystickBase.raycastTarget = false;
            knob.raycastTarget = false;

            JoystickVisual visual = zoneObject.AddComponent<JoystickVisual>();
            visual.joystick = joystick;
            visual.baseRect = (RectTransform)joystickBase.transform;
            visual.knobRect = (RectTransform)knob.transform;
            return joystick;
        }
    }
}
