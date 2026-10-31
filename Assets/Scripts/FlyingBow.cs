using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlyingBow : MonoBehaviour
{
    public PlayerController player;

    [Header("Sprites")]
    public Sprite idle;
    public Sprite nock;
    public Sprite draw;
    public Sprite release;
    public Sprite arrowSprite;

    [Header("Follow")]
    public Vector2 hoverOffset = new Vector2(0.8f, 0.8f);
    public float followSmooth = 0.1f;
    public float bobAmount = 0.05f;
    public float bobSpeed = 3f;

    [Header("Combat")]
    public float damage = 8f;
    public float arrowSpeed = 11f;
    public float aimRange = 6f;
    public float cooldown = 0.12f;

    public bool IsBusy { get { return attacking; } }

    SpriteRenderer sr;
    Vector3 vel;
    bool attacking;
    bool buffered;

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
        sr.sprite = idle;
    }

    void OnDisable()
    {
        attacking = false;
        buffered = false;
    }

    void Update()
    {
        if (player == null) return;

        float side = player.facing == PlayerController.Facing.Left ? -1f : 1f;
        Vector3 goal = player.transform.position + new Vector3(hoverOffset.x * side, hoverOffset.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount, 0f);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref vel, followSmooth);

        if (!attacking) SetAngle(player.FacingVector());
    }

    void SetAngle(Vector2 dir)
    {
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, ang);
    }

    public void RequestAttack()
    {
        if (attacking) { buffered = true; return; }
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        attacking = true;
        bool again;
        do
        {
            buffered = false;

            Transform tgt = FindTarget(player.transform.position);
            Vector2 dir = AimDir(tgt);
            SetAngle(dir);

            sr.sprite = nock;
            yield return new WaitForSeconds(0.06f);

            dir = AimDir(tgt);      // keep tracking while drawing
            SetAngle(dir);
            sr.sprite = draw;
            yield return new WaitForSeconds(0.10f);

            dir = AimDir(tgt);
            SetAngle(dir);
            Fire(dir);
            sr.sprite = release;
            transform.position -= (Vector3)(dir * 0.12f);   // small recoil
            yield return new WaitForSeconds(0.08f);

            sr.sprite = idle;
            yield return new WaitForSeconds(cooldown);
            again = buffered;
        } while (again);

        attacking = false;
    }

    Vector2 AimDir(Transform tgt)
    {
        if (tgt != null)
        {
            Vector2 to = (Vector2)tgt.position + Vector2.up * 0.4f - (Vector2)transform.position;
            if (to.sqrMagnitude > 0.001f) return to.normalized;
        }
        return player.FacingVector();
    }

    void Fire(Vector2 dir)
    {
        GameObject go = new GameObject("Arrow", typeof(SpriteRenderer), typeof(Arrow));
        SpriteRenderer asr = go.GetComponent<SpriteRenderer>();
        asr.sprite = arrowSprite;
        asr.sharedMaterial = sr.sharedMaterial;
        asr.sortingOrder = 9;
        go.transform.position = transform.position + (Vector3)(dir * 0.45f);
        go.GetComponent<Arrow>().Init(dir, damage, arrowSpeed);
    }

    Transform FindTarget(Vector2 from)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(from, aimRange);
        Transform best = null;
        float bestD = float.MaxValue;
        for (int i = 0; i < cols.Length; i++)
        {
            IDamageable d = cols[i].GetComponentInParent<IDamageable>();
            Component comp = d as Component;
            if (comp == null) continue;
            float dist = ((Vector2)comp.transform.position - from).sqrMagnitude;
            if (dist < bestD) { bestD = dist; best = comp.transform; }
        }
        return best;
    }
}
