using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The Spin Slash skill: the sword circles the player, cutting everything around. Each full turn can hit a target once.
    /// </summary>
    public class SpinSlashSkill : SwordSkillBase
    {
        [Header("Shape")]
        [Tooltip("Distance from the player the sword circles at.")]
        public float orbitRadius = 1.5f;
        public float hitRadius = 0.9f;
        [Tooltip("Number of full turns.")]
        public int turns = 2;
        [Tooltip("Total time in seconds.")]
        public float duration = 0.7f;
        [Tooltip("Time in seconds to move from the resting place out to the orbit.")]
        public float spreadTime = 0.1f;

        /// <inheritdoc/>
        public override SkillType Type { get { return SkillType.SpinSlash; } }

        /// <inheritdoc/>
        protected override IEnumerator Perform(float damage)
        {
            Vector2 offset = (Vector2)transform.position - (Vector2)owner.position;
            float startAngle = Mathf.Atan2(offset.y, offset.x);
            float startRadius = offset.magnitude;

            HitScanner scanner = new HitScanner();
            int lastTurn = -1;

            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float progress = t / duration;
                float angle = startAngle + progress * turns * Mathf.PI * 2f;
                float radius = Mathf.Lerp(startRadius, orbitRadius, Mathf.Clamp01(t / spreadTime));
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                Vector2 position = (Vector2)owner.position + direction * radius;
                SetPosition(position);
                visual.Show(direction, true);

                int turn = (int)(progress * turns);
                if (turn != lastTurn)
                {
                    scanner.Reset();
                    lastTurn = turn;
                }

                Hit(scanner, position, hitRadius, damage);
                yield return null;
            }
        }
    }
}
