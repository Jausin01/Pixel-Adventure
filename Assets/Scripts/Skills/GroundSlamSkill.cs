using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The Ground Slam skill: the sword rises over the nearest enemy and crashes down, damaging everything in a wide circle.
    /// </summary>
    public class GroundSlamSkill : SwordSkillBase
    {
        [Tooltip("How far in front of the player the slam lands.")]
        public float distance = 1.8f;
        [Tooltip("How far an enemy can be to be aimed at.")]
        public float aimRange = 4f;
        [Tooltip("Radius of the damaged circle.")]
        public float slamRadius = 1.8f;
        [Tooltip("How high above the landing spot the sword rises.")]
        public float raiseHeight = 1.8f;
        public float raiseTime = 0.25f;
        public float slamTime = 0.08f;
        public float recoverTime = 0.15f;

        /// <inheritdoc/>
        public override SkillType Type { get { return SkillType.GroundSlam; } }

        /// <inheritdoc/>
        protected override IEnumerator Perform(float damage)
        {
            Vector2 direction = AimDirection(aimRange);
            Vector2 landing = (Vector2)owner.position + direction * distance;
            Vector2 top = landing + Vector2.up * raiseHeight;

            visual.Show(Vector2.up, false);
            yield return Tween.MoveTo(transform, transform.position, top, raiseTime, Easing.EaseOutQuad);

            visual.Show(Vector2.down, true);
            yield return Tween.MoveTo(transform, top, landing, slamTime, Easing.EaseInQuad);

            Hit(new HitScanner(), landing, slamRadius, damage);
            yield return new WaitForSeconds(recoverTime);
        }
    }
}
