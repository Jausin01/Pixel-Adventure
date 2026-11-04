using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// What makes one piece of gear different from another of the same kind: its upgrade level, how many times it was
    /// reforged and its nature. Final stats = base bonus x (1 + level and reforge bonuses) x nature multiplier.
    /// </summary>
    public class GearState
    {
        /// <summary>Highest upgrade level.</summary>
        public const int MaxLevel = 5;

        /// <summary>Share of the base stats added by each upgrade level.</summary>
        public const float LevelBonus = 0.2f;

        /// <summary>Share of the base stats added permanently by each reforge.</summary>
        public const float ReforgeBonus = 0.1f;

        /// <summary>The kind of item this piece is.</summary>
        public readonly ItemDefinition Item;

        /// <summary>Upgrade level, from 0 to <see cref="MaxLevel"/>.</summary>
        public int Level { get; private set; }

        /// <summary>How many times the piece was reforged.</summary>
        public int Reforges { get; private set; }

        /// <summary>The nature, or null when the piece has none.</summary>
        public GearNature Nature { get; private set; }

        private GearState(ItemDefinition item, int level, int reforges, GearNature nature)
        {
            Item = item;
            Level = level;
            Reforges = reforges;
            Nature = nature;
        }

        /// <summary>A brand new piece with a random nature.</summary>
        public static GearState CreateNew(ItemDefinition item)
        {
            return new GearState(item, 0, 0, item.natures != null ? item.natures.Roll(item) : null);
        }

        /// <summary>Rebuilds a piece from saved values. An unknown nature id gives a piece without nature.</summary>
        public static GearState Restore(ItemDefinition item, int level, int reforges, string natureId)
        {
            GearNature nature = item.natures != null ? item.natures.Find(natureId) : null;
            return new GearState(item, Mathf.Clamp(level, 0, MaxLevel), Mathf.Max(0, reforges), nature);
        }

        /// <summary>Attack bonus of the piece.</summary>
        public float Attack { get { return Stat(Item.attackBonus, Nature != null ? Nature.attack : 1f, Level); } }

        /// <summary>Defense bonus of the piece.</summary>
        public float Defense { get { return Stat(Item.defenseBonus, Nature != null ? Nature.defense : 1f, Level); } }

        /// <summary>Maximum health bonus of the piece.</summary>
        public float MaxHealth { get { return Stat(Item.maxHealthBonus, Nature != null ? Nature.maxHealth : 1f, Level); } }

        /// <summary>True when the piece can be upgraded further.</summary>
        public bool CanLevelUp { get { return Level < MaxLevel; } }

        /// <summary>Name with nature and level, for example "Sharp Great Sword +3".</summary>
        public string DisplayName
        {
            get
            {
                string prefix = Nature != null ? Nature.Prefix : string.Empty;
                string name = prefix.Length > 0 ? prefix + " " + Item.displayName : Item.displayName;
                return Level > 0 ? name + " +" + Level : name;
            }
        }

        /// <summary>The nature as text for the piece's details.</summary>
        public string NatureText
        {
            get { return Nature == null || Nature.IsNeutral ? "Plain" : Nature.displayName + " (" + Nature.Describe() + ")"; }
        }

        /// <summary>Raises the level by one. Does nothing at the highest level.</summary>
        public void LevelUp()
        {
            if (CanLevelUp) Level++;
        }

        /// <summary>Back to level 0, one more permanent reforge bonus and a new nature.</summary>
        public void Reforge(GearNature newNature)
        {
            Level = 0;
            Reforges++;
            Nature = newNature;
        }

        private float Stat(float baseValue, float natureMultiplier, int level)
        {
            return baseValue * (1f + level * LevelBonus + Reforges * ReforgeBonus) * natureMultiplier;
        }
    }
}
