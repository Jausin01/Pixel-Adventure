using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Draws a <see cref="VirtualJoystick"/>: places the base where the touch started (or at a resting
    /// corner when idle) and moves the knob with the finger.
    /// </summary>
    public class JoystickVisual : MonoBehaviour
    {
        public VirtualJoystick joystick;

        [Tooltip("Joystick background. Must be anchored at the center of the touch zone.")]
        public RectTransform baseRect;

        [Tooltip("Joystick knob (child of the base).")]
        public RectTransform knobRect;

        [Tooltip("Distance of the idle joystick from the bottom-left corner of the touch zone.")]
        public Vector2 restMargin = new Vector2(190f, 170f);

        private void Update()
        {
            baseRect.anchoredPosition = joystick.IsHeld ? joystick.Origin : RestPosition();
            knobRect.anchoredPosition = joystick.KnobOffset;
        }

        private Vector2 RestPosition()
        {
            Rect rect = joystick.Zone.rect;
            return new Vector2(rect.xMin + restMargin.x, rect.yMin + restMargin.y);
        }
    }
}
