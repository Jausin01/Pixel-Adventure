using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteAnimator : MonoBehaviour
{
    public PlayerController player;
    public float fps = 8f;

    public Sprite[] walkDown;
    public Sprite[] walkUp;
    public Sprite[] walkLeft;
    public Sprite[] walkRight;

    SpriteRenderer sr;
    float t;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (player == null) player = GetComponent<PlayerController>();
    }

    void Update()
    {
        Sprite[] set = SetFor(player.facing);
        if (set == null || set.Length == 0) return;

        if (player.IsMoving)
        {
            t += Time.deltaTime * fps;
            sr.sprite = set[(int)t % set.Length];
        }
        else
        {
            t = 0f;
            sr.sprite = set[0];   // frame 0 is the standing pose
        }
    }

    Sprite[] SetFor(PlayerController.Facing f)
    {
        switch (f)
        {
            case PlayerController.Facing.Up: return walkUp;
            case PlayerController.Facing.Left: return walkLeft;
            case PlayerController.Facing.Right: return walkRight;
            default: return walkDown;
        }
    }
}
