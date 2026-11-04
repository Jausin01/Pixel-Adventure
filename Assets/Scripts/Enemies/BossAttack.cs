using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>The moves a boss can use. Combine them to give each boss its own set.</summary>
    [Flags]
    public enum BossMoves
    {
        None = 0,
        Slam = 1,
        Charge = 2,
        Volley = 4,
        Rain = 8
    }

    /// <summary>
    /// The attacks of a boss. Each time it can attack it picks one of its moves, never the same twice in a row, and
    /// every move is telegraphed with a red warning so the player can dodge:
    /// Slam (a circle around the boss), Charge (a rush along a marked lane), Volley (a fan of projectiles) and
    /// Rain (blasts landing around the player). Below half health the boss is enraged: it attacks faster and with more.
    /// </summary>
    public class BossAttack : EnemyAttack
    {
        [Header("Moves")]
        public BossMoves moves = BossMoves.Slam | BossMoves.Charge | BossMoves.Volley;

        [Header("Appearance")]
        [Tooltip("A ring about 2 units wide, scaled to the area it warns about.")]
        public Sprite warningCircle;
        [Tooltip("A plain square used to mark the charge lane.")]
        public Sprite warningSquare;
        public Sprite projectileSprite;
        public Sprite blastSprite;
        public Material material;
        public Color warningColor = new Color(1f, 0.2f, 0.15f, 1f);
        public int sortingOrder = 8;

        [Header("Slam")]
        public float slamRadius = 2.2f;
        public float slamWindup = 0.8f;
        [Tooltip("Damage multiplier of this move.")]
        public float slamDamage = 1.1f;

        [Header("Charge")]
        public float chargeWindup = 0.65f;
        public float chargeSpeed = 9f;
        public float chargeTime = 0.45f;
        public float chargeHitRadius = 1.0f;
        public float chargeRecover = 0.4f;
        public float chargeDamage = 1.2f;

        [Header("Volley")]
        public int volleyCount = 5;
        public float volleySpread = 55f;
        public float volleyWindup = 0.6f;
        public float projectileSpeed = 6f;
        public float projectileLifetime = 2.4f;
        public float projectileRadius = 0.35f;
        public float volleyDamage = 0.6f;

        [Header("Rain")]
        public int rainBlasts = 7;
        public float rainAreaRadius = 2.6f;
        public float rainBlastRadius = 0.95f;
        public float rainWarning = 0.9f;
        public float rainInterval = 0.16f;
        public float rainDamage = 0.7f;

        [Header("Enrage")]
        [Range(0f, 1f)]
        public float enrageBelow = 0.5f;

        private EnemyMovement movement;
        private BossMoves lastMove = BossMoves.None;
        private bool enraged;

        private bool Enraged { get { return OwnHealth.Fraction <= enrageBelow; } }

        private void Start()
        {
            movement = GetComponent<EnemyMovement>();
        }

        /// <inheritdoc/>
        protected override IEnumerator Perform(Transform target)
        {
            if (Enraged && !enraged)
            {
                enraged = true;
                Sfx.Play("boss_growl");
                cooldown *= 0.6f;
                GetComponent<SpriteRenderer>().color = new Color(1f, 0.75f, 0.7f, 1f);
            }

            BossMoves move = Choose(target);
            lastMove = move;

            if (move == BossMoves.Slam) yield return Slam(target);
            else if (move == BossMoves.Charge) yield return Charge(target);
            else if (move == BossMoves.Volley) yield return Volley(target);
            else yield return Rain(target);
        }

        // Close targets get melee moves; far ones get the ranged moves and the charge.
        private BossMoves Choose(Transform target)
        {
            bool close = Vector2.Distance(transform.position, target.position) < slamRadius + 0.6f;
            List<BossMoves> options = new List<BossMoves>();

            if (Has(BossMoves.Slam) && close) options.Add(BossMoves.Slam);
            if (Has(BossMoves.Charge)) options.Add(BossMoves.Charge);
            if (Has(BossMoves.Volley) && !close) options.Add(BossMoves.Volley);
            if (Has(BossMoves.Rain) && !close) options.Add(BossMoves.Rain);
            if (options.Count == 0) options.Add(moves != BossMoves.None ? FirstMove() : BossMoves.Slam);

            if (options.Count > 1) options.Remove(lastMove);
            return options[UnityEngine.Random.Range(0, options.Count)];
        }

        private bool Has(BossMoves move) { return (moves & move) != 0; }

        private BossMoves FirstMove()
        {
            if (Has(BossMoves.Slam)) return BossMoves.Slam;
            if (Has(BossMoves.Charge)) return BossMoves.Charge;
            if (Has(BossMoves.Volley)) return BossMoves.Volley;
            return BossMoves.Rain;
        }

        private float WindupScale { get { return enraged ? 0.75f : 1f; } }

        private IEnumerator Slam(Transform target)
        {
            Vector2 center = (Vector2)transform.position + Vector2.up * 0.3f;
            SpriteRenderer warning = Warn(warningCircle, center, new Vector2(slamRadius, slamRadius), 0f, 0.35f);
            Sfx.Play("boss_warning");

            yield return new WaitForSeconds(slamWindup * WindupScale);
            Destroy(warning.gameObject);
            if (!OwnHealth.IsAlive) yield break;

            Strike(target, center, slamRadius, slamDamage);
            Sfx.Play("stomp");
            Burst(center, slamRadius);
            yield return new WaitForSeconds(0.35f);
        }

        private IEnumerator Charge(Transform target)
        {
            Vector2 start = (Vector2)transform.position + Vector2.up * 0.3f;
            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            if (direction.sqrMagnitude < 0.01f) direction = Vector2.right;

            float length = chargeSpeed * chargeTime;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            SpriteRenderer lane = Warn(warningSquare, start + direction * length * 0.5f,
                new Vector2(length, chargeHitRadius * 2f), angle, 0.3f);

            Sfx.Play("boss_warning");
            yield return new WaitForSeconds(chargeWindup * WindupScale);
            Destroy(lane.gameObject);
            if (!OwnHealth.IsAlive) yield break;

            Sfx.Play("boss_charge");
            bool hit = false;
            for (float t = 0f; t < chargeTime && OwnHealth.IsAlive; t += Time.deltaTime)
            {
                movement.Dash = direction * chargeSpeed;
                if (!hit) hit = Strike(target, (Vector2)transform.position + Vector2.up * 0.4f, chargeHitRadius, chargeDamage);
                yield return null;
            }
            movement.Dash = Vector2.zero;

            yield return new WaitForSeconds(chargeRecover); // tired after the rush: the opening to hit back
        }

        private IEnumerator Volley(Transform target)
        {
            SpriteRenderer glow = Warn(warningCircle, (Vector2)transform.position + Vector2.up * 0.5f, Vector2.one * 0.8f, 0f, 0.5f);
            Sfx.Play("boss_warning");
            yield return new WaitForSeconds(volleyWindup * WindupScale);
            Destroy(glow.gameObject);
            Sfx.Play("boss_volley");

            int waves = enraged ? 2 : 1;
            int count = volleyCount + (enraged ? 2 : 0);
            for (int wave = 0; wave < waves && OwnHealth.IsAlive; wave++)
            {
                Vector2 origin = (Vector2)transform.position + Vector2.up * 0.5f;
                Vector2 aim = ((Vector2)target.position + Vector2.up * BodyHeight - origin).normalized;
                for (int i = 0; i < count; i++)
                {
                    float t = count == 1 ? 0f : (float)i / (count - 1) - 0.5f;
                    FireProjectile(target, origin, Rotate(aim, t * volleySpread));
                }
                yield return new WaitForSeconds(0.35f);
            }
            yield return new WaitForSeconds(0.2f);
        }

        private IEnumerator Rain(Transform target)
        {
            int blasts = rainBlasts + (enraged ? 3 : 0);
            Vector2 center = (Vector2)target.position;

            for (int i = 0; i < blasts && OwnHealth.IsAlive; i++)
            {
                // The first blast lands right on the player, the rest scatter around.
                Vector2 spot = i == 0 ? center : center + UnityEngine.Random.insideUnitCircle * rainAreaRadius;
                StartCoroutine(Blast(target, spot));
                yield return new WaitForSeconds(rainInterval);
            }
            yield return new WaitForSeconds(rainWarning * WindupScale);
        }

        private IEnumerator Blast(Transform target, Vector2 spot)
        {
            SpriteRenderer warning = Warn(warningCircle, spot, Vector2.one * rainBlastRadius, 0f, 0.35f);
            yield return new WaitForSeconds(rainWarning * WindupScale);
            Destroy(warning.gameObject);

            Strike(target, spot, rainBlastRadius, rainDamage);
            Sfx.Play("fireball_explosion");
            Burst(spot, rainBlastRadius);
        }

        private void FireProjectile(Transform target, Vector2 position, Vector2 direction)
        {
            SpriteRenderer shot = SpriteEffect.Create("Boss Projectile", projectileSprite, material, position, sortingOrder + 2);
            float original = damage;
            shot.gameObject.AddComponent<EnemyProjectile>().Launch(transform, target, direction, projectileSpeed,
                projectileLifetime, original * volleyDamage, projectileRadius, 0f);
        }

        // Damages the target when it stands inside the circle. Returns true on a hit.
        private bool Strike(Transform target, Vector2 center, float radius, float multiplier)
        {
            float original = damage;
            damage = original * multiplier;
            bool hit = TryDamage(target, center, radius);
            damage = original;
            return hit;
        }

        private SpriteRenderer Warn(Sprite sprite, Vector2 position, Vector2 scale, float angle, float alpha)
        {
            SpriteRenderer warning = SpriteEffect.Create("Boss Warning", sprite, material, position, sortingOrder);
            warning.color = warningColor;
            SpriteEffect.SetAlpha(warning, alpha);
            warning.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            warning.transform.localScale = new Vector3(scale.x, scale.y, 1f);
            Destroy(warning.gameObject, 4f); // safety net
            return warning;
        }

        private void Burst(Vector2 center, float radius)
        {
            if (blastSprite == null) return;
            EffectFade.Spawn(blastSprite, material, center, 0f, Vector2.one * radius * 1.2f, 0.3f, 1.4f, sortingOrder + 1);
        }

        private static Vector2 Rotate(Vector2 direction, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            return new Vector2(direction.x * cos - direction.y * sin, direction.x * sin + direction.y * cos);
        }
    }
}
