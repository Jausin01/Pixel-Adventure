using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The Sword Throw skill: the sword is thrown spinning towards the nearest enemy, then flies back to the player.
    /// It can hit each target once on the way out and once on the way back.
    /// </summary>
    public class SwordThrowSkill : SwordSkillBase
    {
        [Tooltip("How far from the player the sword flies.")]
        public float range = 6f;
        public float hitRadius = 0.8f;
        public float outTime = 0.25f;
        public float backTime = 0.25f;
        [Tooltip("Spin speed in degrees per second.")]
        public float spinSpeed = 1080f;
        [Tooltip("Where the sword returns to, relative to the player.")]
        public Vector2 returnOffset = new Vector2(0.75f, 0.95f);

        /// <inheritdoc/>
        public override SkillType Type { get { return SkillType.SwordThrow; } }

        /// <inheritdoc/>
        protected override IEnumerator Perform(float damage)
        {
            Vector2 direction = AimDirection(range);
            Vector2 from = transform.position;
            Vector2 farthest = (Vector2)owner.position + direction * range;
            HitScanner scanner = new HitScanner();
            float angle = 0f;

            for (float t = 0f; t < outTime; t += Time.deltaTime)
            {
                angle += spinSpeed * Time.deltaTime;
                SetPosition(Vector2.Lerp(from, farthest, Easing.EaseOutQuad(t / outTime)));
                ShowSpin(angle);
                Hit(scanner, transform.position, hitRadius, damage);
                yield return null;
            }

            scanner.Reset();
            Vector2 turnPoint = transform.position;
            for (float t = 0f; t < backTime; t += Time.deltaTime)
            {
                angle += spinSpeed * Time.deltaTime;
                Vector2 home = (Vector2)owner.position + returnOffset;
                SetPosition(Vector2.Lerp(turnPoint, home, Easing.EaseInQuad(t / backTime)));
                ShowSpin(angle);
                Hit(scanner, transform.position, hitRadius, damage);
                yield return null;
            }
        }

        private void ShowSpin(float angleDegrees)
        {
            float radians = angleDegrees * Mathf.Deg2Rad;
            visual.Show(new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)), true);
        }
    }
}
