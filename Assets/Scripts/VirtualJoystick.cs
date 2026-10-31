using UnityEngine;
using UnityEngine.EventSystems;

// Floating joystick. Put it on a transparent Image that covers the left half of the screen.
public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static Vector2 Value;

    public RectTransform baseRect;   // joystick background (child, anchored at the centre of this zone)
    public RectTransform knobRect;   // knob (child of baseRect)
    public float radius = 110f;
    public float deadZone = 0.12f;
    public Vector2 restMargin = new Vector2(230f, 210f);   // distance from the bottom-left corner when idle

    RectTransform zone;

    void Awake()
    {
        zone = (RectTransform)transform;
    }

    void Start()
    {
        ResetStick();
    }

    void OnDisable()
    {
        Value = Vector2.zero;
    }

    Vector2 RestPosition()
    {
        Rect r = zone.rect;
        return new Vector2(r.xMin + restMargin.x, r.yMin + restMargin.y);
    }

    void ResetStick()
    {
        Value = Vector2.zero;
        if (baseRect != null) baseRect.anchoredPosition = RestPosition();
        if (knobRect != null) knobRect.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData e)
    {
        Vector2 local;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(zone, e.position, e.pressEventCamera, out local))
            baseRect.anchoredPosition = local;
        OnDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        Vector2 local;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, e.position, e.pressEventCamera, out local)) return;
        Vector2 d = Vector2.ClampMagnitude(local, radius);
        knobRect.anchoredPosition = d;
        Vector2 v = d / radius;
        Value = v.magnitude < deadZone ? Vector2.zero : v;
    }

    public void OnPointerUp(PointerEventData e)
    {
        ResetStick();
    }
}
