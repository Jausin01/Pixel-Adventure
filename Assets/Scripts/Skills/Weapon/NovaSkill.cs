using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The weapon crashes down at the player's feet and rings of force spread outwards, one after another,
    /// hurting everything they cross once (Seismic Slam).
    /// </summary>
    public class NovaSkill : WeaponSkillBase
    {
        public int waves = 3;
        public float firstRadius = 1.4f;
        public float radiusStep = 1.1f;
        public float waveInterval = 0.14f;
        public float raiseHeight = 1.6f;
        public float raiseTime = 0.3f;
        public float slamTime = 0.07f;
        public float recover = 0.2f;

        /// <inheritdoc/>
        protected override IEnumerator Perform(float damage)
        {
            Vector2 feet = (Vector2)owner.position + facing.Vector * 0.5f;
            Vector2 top = feet + Vector2.up * raiseHeight;

            Point(Vector2.up);
            yield return Tween.MoveTo(transform, transform.position, top, raiseTime, Easing.EaseOutQuad);

            Point(Vector2.down);
            yield return Tween.MoveTo(transform, top, feet, slamTime, Easing.EaseInQuad);

            HitScanner scanner = new HitScanner(); // each enemy is hurt by the first ring that reaches it
            for (int wave = 0; wave < waves; wave++)
            {
                float radius = firstRadius + radiusStep * wave;
                Hit(scanner, (Vector2)owner.position, radius, damage);

                int puffs = 8 + wave * 4;
                for (int i = 0; i < puffs; i++)
                {
                    Vector2 offset = Rotate(Vector2.right, 360f * i / puffs) * radius;
                    Puff(dustSprite, (Vector2)owner.position + offset, 1f, 0.35f, 1.8f);
                }
                yield return new WaitForSeconds(waveInterval);
            }

            yield return new WaitForSeconds(recover);
        }
    }
}
