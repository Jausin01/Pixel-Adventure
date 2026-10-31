using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Owns the two weapons, forwards the attack input to the active one and swaps between them.
public class WeaponHolder : MonoBehaviour
{
    public FlyingSword sword;
    public FlyingBow bow;
    public bool startWithBow = false;

    bool usingBow;

    void Start()
    {
        usingBow = startWithBow;
        Apply();
    }

    void Apply()
    {
        if (sword != null) sword.gameObject.SetActive(!usingBow);
        if (bow != null) bow.gameObject.SetActive(usingBow);
    }

    public void Attack()
    {
        if (usingBow) { if (bow != null) bow.RequestAttack(); }
        else if (sword != null) sword.RequestAttack();
    }

    public void Swap()
    {
        if (sword == null || bow == null) return;
        if (sword.IsBusy || bow.IsBusy) return;

        Transform from = usingBow ? bow.transform : sword.transform;
        usingBow = !usingBow;
        Transform to = usingBow ? bow.transform : sword.transform;
        to.position = from.position;
        Apply();
    }

    void Update()
    {
        if (AttackKeyDown()) Attack();
        if (SwapKeyDown()) Swap();
    }

    static bool AttackKeyDown()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        return k != null && (k.spaceKey.wasPressedThisFrame || k.jKey.wasPressedThisFrame);
#elif ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J);
#else
        return false;
#endif
    }

    static bool SwapKeyDown()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        return k != null && (k.qKey.wasPressedThisFrame || k.tabKey.wasPressedThisFrame);
#elif ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Tab);
#else
        return false;
#endif
    }
}
