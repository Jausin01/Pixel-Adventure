using System.Collections;
using UnityEngine;

public class TrainingDummy : MonoBehaviour, IDamageable
{
    public float maxHp = 100f;

    float hp;
    SpriteRenderer sr;
    Vector3 home;
    Color baseColor;
    bool dead;
    Coroutine hitRoutine;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        home = transform.position;
        baseColor = sr.color;
        hp = maxHp;
    }

    public void TakeDamage(float amount, Vector2 from)
    {
        if (dead) return;
        hp -= amount;
        Debug.Log(name + " took " + amount + " damage (" + Mathf.Max(hp, 0f) + "/" + maxHp + ")");
        if (hitRoutine != null) StopCoroutine(hitRoutine);
        hitRoutine = StartCoroutine(Hit(from));
        if (hp <= 0f) StartCoroutine(Respawn());
    }

    IEnumerator Hit(Vector2 from)
    {
        Vector2 away = ((Vector2)transform.position - from).normalized;
        sr.color = new Color(1f, 0.35f, 0.35f);
        transform.position = home + (Vector3)(away * 0.12f);
        yield return new WaitForSeconds(0.08f);
        transform.position = home;
        sr.color = baseColor;
    }

    IEnumerator Respawn()
    {
        dead = true;
        sr.enabled = false;
        yield return new WaitForSeconds(1.2f);
        hp = maxHp;
        dead = false;
        sr.enabled = true;
    }
}
