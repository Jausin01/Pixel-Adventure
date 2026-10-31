using UnityEngine;
using UnityEngine.EventSystems;

// Touch button that reacts on finger-down (no waiting for release).
public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Kind { Attack, Skin, Swap }

    public Kind kind = Kind.Attack;
    public int skinIndex;
    public WeaponHolder weapons;

    public void OnPointerDown(PointerEventData e)
    {
        transform.localScale = Vector3.one * 0.92f;
        if (weapons == null) return;
        if (kind == Kind.Attack) weapons.Attack();
        else if (kind == Kind.Swap) weapons.Swap();
        else if (weapons.sword != null) weapons.sword.SetSkin(skinIndex);
    }

    public void OnPointerUp(PointerEventData e)
    {
        transform.localScale = Vector3.one;
    }
}
