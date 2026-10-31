namespace MobileRpg
{
    /// <summary>
    /// Easing functions that map a normalized time value in [0, 1] to an eased progress value in [0, 1].
    /// </summary>
    public static class Easing
    {
        /// <summary>Starts fast and slows down towards the end.</summary>
        public static float EaseOutQuad(float t)
        {
            return 1f - (1f - t) * (1f - t);
        }

        /// <summary>Starts slow and accelerates towards the end.</summary>
        public static float EaseInQuad(float t)
        {
            return t * t;
        }
    }
}
