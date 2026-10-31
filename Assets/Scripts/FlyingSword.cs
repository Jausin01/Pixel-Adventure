using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwordSkin
{
    public string name;
    public Sprite up;      // blade pointing up
    public Sprite diag;    // blade pointing up-right
    public Sprite smear;   // stretched diagonal (used while striking)
    public Sprite side;    // blade pointing right
}

[RequireComponent(typeof(SpriteRenderer))]
public class FlyingSword : MonoBehaviour
{
    public PlayerController player;
    public SwordSkin[] skins;
    public int skinIndex = 0;

    [Header("Follow")]
    public Vector2 hoverOffset = new Vector2(0.75f, 0.95f);
    public float followSmooth = 0.12f;
    public float bobAmount = 0.06f;
    public float bobSpeed = 3f;

    [Header("Combat")]
    public float damage = 10f;
    public float hitRadius = 0.6f;
    public float aimRange = 3.5f;
    public float comboReset = 1.0f;

    SpriteRenderer sr;
    Vector3 vel;
    bool attacking;
    bool buffered;
    int combo;
    float lastEnd = -10f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (player == null)
        {
#if UNITY_2023_1_OR_NEWER
            player = FindFirstObjectByType<PlayerController>();
#else
            player = FindObjectOfType<PlayerController>();
#endif
        }
        SetSkin(skinIndex);
        SetOrientation(Vector2.up, false);
    }

    public void SetSkin(int i)
    {
        if (skins == null || skins.Length == 0) return;
        skinIndex = Mathf.Clamp(i, 0, skins.Length - 1);
        if (!attacking) SetOrientation(Vector2.up, false);
    }

    void Update()
    {
        if (attacking || player == null) return;

        float side = player.facing == PlayerController.Facing.Left ? -1f : 1f;
        Vector3 goal = player.transform.position + new Vector3(hoverOffset.x * side, hoverOffset.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount, 0f);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref vel, followSmooth);
        SetOrientation(Vector2.up, false);
    }

    public void RequestAttack()
    {
        if (attacking) { buffered = true; return; }
        StartCoroutine(AttackRoutine());
    }

    // ---------------------------------------------------------------- attack
    IEnumerator AttackRoutine()
    {
        attacking = true;
        bool again;
        do
        {
            buffered = false;
            if (Time.time - lastEnd > comboReset) combo = 0;

            Vector2 p = player.transform.position;
            Transform tgt = FindTarget(p);
            Vector2 fwd = tgt != null ? ((Vector2)tgt.position - p).normalized : player.FacingVector();
            if (fwd.sqrMagnitude < 0.001f) fwd = player.FacingVector();
            Vector2 c = tgt != null ? (Vector2)tgt.position : p + fwd * 1.6f;
            Vector2 perp = new Vector2(-fwd.y, fwd.x);

            Vector2 a, b;
            switch (combo % 3)
            {
                case 0:   // vertical, top to bottom
                    a = c + Vector2.up * 1.6f;
                    b = c + Vector2.down * 0.3f;
                    break;
                case 1:   // lateral sweep
                    a = c - perp * 1.5f;
                    b = c + perp * 1.5f;
                    break;
                default:  // diagonal, upper left to lower right
                    a = c + new Vector2(-1.2f, 1.4f);
                    b = c + new Vector2(1.2f, -1.0f);
                    break;
            }

            Vector2 dir = (b - a).normalized;
            yield return MoveRoutine(a, dir, 0.12f);
            yield return StrikeRoutine(a, b, dir, 0.10f);
            yield return new WaitForSeconds(0.06f);

            combo++;
            lastEnd = Time.time;
            again = buffered;
        } while (again);

        attacking = false;
    }

    IEnumerator MoveRoutine(Vector2 to, Vector2 dir, float dur)
    {
        Vector2 from = transform.position;
        SetOrientation(dir, false);
        for (float t = 0f; t < dur; t += Time.deltaTime)
        {
            float k = 1f - (1f - t / dur) * (1f - t / dur);   // ease out
            transform.position = Vector2.Lerp(from, to, k);
            yield return null;
        }
        transform.position = to;
    }

    IEnumerator StrikeRoutine(Vector2 a, Vector2 b, Vector2 dir, float dur)
    {
        List<IDamageable> hit = new List<IDamageable>();
        SetOrientation(dir, true);
        for (float t = 0f; t < dur; t += Time.deltaTime)
        {
            float k = (t / dur) * (t / dur);   // ease in
            transform.position = Vector2.Lerp(a, b, k);
            if (HitCheck(hit)) yield return HitStop();
            yield return null;
        }
        transform.position = b;
        if (HitCheck(hit)) yield return HitStop();
    }

    bool HitCheck(List<IDamageable> already)
    {
        bool any = false;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, hitRadius);
        for (int i = 0; i < cols.Length; i++)
        {
            IDamageable d = cols[i].GetComponentInParent<IDamageable>();
            if (d == null || already.Contains(d)) continue;
            already.Add(d);
            d.TakeDamage(damage, transform.position);
            any = true;
        }
        return any;
    }

    IEnumerator HitStop()
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(0.04f);
        Time.timeScale = 1f;
    }

    Transform FindTarget(Vector2 from)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(from, aimRange);
        Transform best = null;
        float bestD = float.MaxValue;
        for (int i = 0; i < cols.Length; i++)
        {
            IDamageable d = cols[i].GetComponentInParent<IDamageable>();
            if (d == null) continue;
            Component comp = d as Component;
            if (comp == null) continue;
            float dist = ((Vector2)comp.transform.position - from).sqrMagnitude;
            if (dist < bestD) { bestD = dist; best = comp.transform; }
        }
        return best;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
        attacking = false;
        buffered = false;
    }

    public bool IsBusy { get { return attacking; } }

    // ---------------------------------------------------------- orientation
    // Picks the drawn frame + flips for the nearest of 8 directions (no free rotation, so pixels stay crisp).
    void SetOrientation(Vector2 dir, bool smear)
    {
        if (skins == null || skins.Length == 0) return;
        SwordSkin s = skins[Mathf.Clamp(skinIndex, 0, skins.Length - 1)];

        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        int k = Mathf.RoundToInt(ang / 45f);
        k = ((k % 8) + 8) % 8;   // 0 right, 1 up-right, 2 up, 3 up-left, 4 left, 5 down-left, 6 down, 7 down-right

        Sprite sp;
        bool fx = false, fy = false;
        switch (k)
        {
            case 0: sp = s.side; break;
            case 1: sp = s.diag; break;
            case 2: sp = s.up; break;
            case 3: sp = s.diag; fx = true; break;
            case 4: sp = s.side; fx = true; break;
            case 5: sp = s.diag; fx = true; fy = true; break;
            case 6: sp = s.up; fy = true; break;
            default: sp = s.diag; fy = true; break;
        }
        if (smear && s.smear != null && (k == 1 || k == 3 || k == 5 || k == 7)) sp = s.smear;

        sr.sprite = sp;
        sr.flipX = fx;
        sr.flipY = fy;
    }
}
