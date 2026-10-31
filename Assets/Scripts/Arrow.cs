using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 11f;
    public float damage = 8f;
    public float life = 1.4f;
    public float radius = 0.18f;

    Vector2 dir = Vector2.right;

    public void Init(Vector2 direction, float dmg, float spd)
    {
        dir = direction.normalized;
        damage = dmg;
        speed = spd;
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
        life -= Time.deltaTime;
        if (life <= 0f) { Destroy(gameObject); return; }

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
        for (int i = 0; i < cols.Length; i++)
        {
            Collider2D c = cols[i];
            if (c.isTrigger || c.GetComponentInParent<PlayerController>() != null) continue;

            IDamageable d = c.GetComponentInParent<IDamageable>();
            if (d != null) d.TakeDamage(damage, transform.position);
            Destroy(gameObject);   // hit a target or a wall
            return;
        }
    }
}
