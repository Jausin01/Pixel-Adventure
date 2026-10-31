using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileRpg
{
    /// <summary>
    /// Touch input for a floating joystick. Put it on a transparent UI element that covers the area where
    /// the joystick can be used: the stick starts wherever the finger first touches. It only reports the
    /// result; drawing is done by <see cref="JoystickVisual"/>.
    /// </summary>
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Tooltip("Distance (in canvas units) the finger must travel for full input.")]
        public float radius = 110f;

        [Tooltip("Input below this magnitude is ignored.")]
        [Range(0f, 0.5f)]
        public float deadZone = 0.12f;

        /// <summary>Stick value with a magnitude between 0 and 1.</summary>
        public Vector2 Value { get; private set; }

        /// <summary>True while a finger is holding the stick.</summary>
        public bool IsHeld { get; private set; }

        /// <summary>Where the touch started, in the local space of <see cref="Zone"/>.</summary>
        public Vector2 Origin { get; private set; }

        /// <summary>Offset of the finger from <see cref="Origin"/>, limited to <see cref="radius"/>.</summary>
        public Vector2 KnobOffset { get; private set; }

        /// <summary>The touch area.</summary>
        public RectTransform Zone { get { return zone; } }

        private RectTransform zone;

        private void Awake()
        {
            zone = (RectTransform)transform;
        }

        private void OnDisable()
        {
            Release();
        }

        /// <inheritdoc/>
        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 local;
            if (!TryGetLocalPoint(eventData, out local)) return;

            Origin = local;
            IsHeld = true;
            UpdateStick(local);
        }

        /// <inheritdoc/>
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 local;
            if (IsHeld && TryGetLocalPoint(eventData, out local)) UpdateStick(local);
        }

        /// <inheritdoc/>
        public void OnPointerUp(PointerEventData eventData)
        {
            Release();
        }

        private bool TryGetLocalPoint(PointerEventData eventData, out Vector2 local)
        {
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(zone, eventData.position, eventData.pressEventCamera, out local);
        }

        private void UpdateStick(Vector2 local)
        {
            KnobOffset = Vector2.ClampMagnitude(local - Origin, radius);
            Vector2 normalized = KnobOffset / radius;
            Value = normalized.magnitude < deadZone ? Vector2.zero : normalized;
        }

        private void Release()
        {
            IsHeld = false;
            Value = Vector2.zero;
            KnobOffset = Vector2.zero;
        }
    }
}
