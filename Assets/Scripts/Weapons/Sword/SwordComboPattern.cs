using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Describes the strike path of each step in the sword combo.
    /// </summary>
    public static class SwordComboPattern
    {
        /// <summary>Number of steps in the combo (vertical, lateral, diagonal).</summary>
        public const int StepCount = 3;

        /// <summary>
        /// Calculates where the sword starts and ends a strike.
        /// </summary>
        /// <param name="step">Combo step: 0 vertical, 1 lateral, 2 diagonal.</param>
        /// <param name="center">World position the strike is centered on (usually the target).</param>
        /// <param name="forward">Unit vector from the attacker towards the center.</param>
        /// <param name="start">World position where the strike begins.</param>
        /// <param name="end">World position where the strike ends.</param>
        public static void GetPath(int step, Vector2 center, Vector2 forward, out Vector2 start, out Vector2 end)
        {
            switch (step % StepCount)
            {
                case 0: // vertical: from above the target down through it
                    start = center + Vector2.up * 1.6f;
                    end = center + Vector2.down * 0.3f;
                    break;

                case 1: // lateral: sweep across the target, perpendicular to the attack direction
                    Vector2 perpendicular = new Vector2(-forward.y, forward.x);
                    start = center - perpendicular * 1.5f;
                    end = center + perpendicular * 1.5f;
                    break;

                default: // diagonal: from the upper left to the lower right
                    start = center + new Vector2(-1.2f, 1.4f);
                    end = center + new Vector2(1.2f, -1.0f);
                    break;
            }
        }
    }
}
