using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The Piercing Thrust skill: a quick long stab towards the nearest enemy that goes through everything on its line.
    /// </summary>
    public class PiercingThrustSkill : SwordSkillBase
    {
        [Tooltip("How far from the player the stab reaches.")]
        public float reach = 4.5f;
        [Tooltip("How far an enemy can be to be aimed at.")]
        public float aimRange = 5f;
        public float hitRadius = 0.7f;
        public float windupTime = 0.12f;
        public float thrustTime = 0.16f;

        /// <inheritdoc/>
        public override SkillType Type { get { return SkillType.PiercingThrust; } }

        /// <inheritdoc/>
        protected override IEnumerator Perform(float damage)
        {
            Vector2 direction = AimDirection(aimRange);
            Vector2 origin = owner.position;
            Vector2 start = origin - direction * 0.3f;
            Vector2 end = origin + direction * reach;

            visual.Show(direction, false);
            yield return Tween.MoveTo(transform, transform.position, start, windupTime, Easing.EaseOutQuad);

            HitScanner scanner = new HitScanner();
            visual.Show(direction, true);
            yield return Tween.MoveTo(transform, start, end, thrustTime, Easing.EaseInQuad,
                progress => Hit(scanner, transform.position, hitRadius, damage));
        }
    }
}
