using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public enum Facing { Down, Up, Left, Right }

    public float moveSpeed = 3.5f;
    public Facing facing = Facing.Down;

    public Vector2 MoveInput { get; private set; }
    public bool IsMoving { get { return MoveInput.sqrMagnitude > 0.01f; } }

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        Vector2 v = VirtualJoystick.Value + ReadKeyboard();
        if (v.sqrMagnitude > 1f) v.Normalize();
        MoveInput = v;

        if (IsMoving)
        {
            if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
                facing = v.x > 0f ? Facing.Right : Facing.Left;
            else
                facing = v.y > 0f ? Facing.Up : Facing.Down;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + MoveInput * moveSpeed * Time.fixedDeltaTime);
    }

    public Vector2 FacingVector()
    {
        switch (facing)
        {
            case Facing.Up: return Vector2.up;
            case Facing.Left: return Vector2.left;
            case Facing.Right: return Vector2.right;
            default: return Vector2.down;
        }
    }

    static Vector2 ReadKeyboard()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        if (k == null) return Vector2.zero;
        float x = ((k.dKey.isPressed || k.rightArrowKey.isPressed) ? 1f : 0f) - ((k.aKey.isPressed || k.leftArrowKey.isPressed) ? 1f : 0f);
        float y = ((k.wKey.isPressed || k.upArrowKey.isPressed) ? 1f : 0f) - ((k.sKey.isPressed || k.downArrowKey.isPressed) ? 1f : 0f);
        return new Vector2(x, y);
#elif ENABLE_LEGACY_INPUT_MANAGER
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#else
        return Vector2.zero;
#endif
    }
}
