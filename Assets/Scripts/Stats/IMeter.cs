using System;

namespace MobileRpg
{
    /// <summary>
    /// Anything that has a current value as a fraction of a maximum (health, energy...) and can be shown on a bar.
    /// </summary>
    public interface IMeter
    {
        /// <summary>Current value as a fraction from 0 to 1.</summary>
        float Fraction { get; }

        /// <summary>Raised whenever <see cref="Fraction"/> changes.</summary>
        event Action FractionChanged;
    }
}
