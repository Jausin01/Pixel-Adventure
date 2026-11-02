using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>Which attack a monster uses.</summary>
    public enum MonsterAttackKind
    {
        Claws,
        Chomp
    }

    /// <summary>
    /// Describes one kind of monster: art, stats and attack. <see cref="MonsterBuilder"/> turns it into an object.
    /// </summary>
    public class MonsterSpec
    {
        public string name;
        public string framesPrefix;          // for example "Enemies/rat_brown_"
        public int frameCount = 3;
        public string effectSprite;          // attack sprite, for example "Effects/claws_brown"
        public MonsterAttackKind attack;

        public float maxHealth;
        public float moveSpeed;
        public float damage;
        public float attackRange;
        public float sightRange = 6f;

        public Vector2 colliderSize;
        public Vector2 colliderOffset;
        public float healthBarHeight;

        public bool animateWhenIdle;
        public bool spriteFacesRight;        // true: mirror the sprite to look at the target

        /// <summary>Brown rat: slow, tough, slashes with claws.</summary>
        public static MonsterSpec BrownRat()
        {
            return new MonsterSpec
            {
                name = "Brown Rat", framesPrefix = "Enemies/rat_brown_", effectSprite = "Effects/claws_brown",
                attack = MonsterAttackKind.Claws, maxHealth = 40f, moveSpeed = 1.8f, damage = 8f, attackRange = 1f,
                colliderSize = new Vector2(0.8f, 0.4f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 0.8f, animateWhenIdle = false, spriteFacesRight = true
            };
        }

        /// <summary>White rat: faster and more fragile than the brown one.</summary>
        public static MonsterSpec WhiteRat()
        {
            return new MonsterSpec
            {
                name = "White Rat", framesPrefix = "Enemies/rat_white_", effectSprite = "Effects/claws_white",
                attack = MonsterAttackKind.Claws, maxHealth = 30f, moveSpeed = 2.4f, damage = 6f, attackRange = 1f,
                colliderSize = new Vector2(0.8f, 0.4f), colliderOffset = new Vector2(0f, 0.25f),
                healthBarHeight = 0.8f, animateWhenIdle = false, spriteFacesRight = true
            };
        }

        /// <summary>Bat: flies fast and bites.</summary>
        public static MonsterSpec Bat()
        {
            return new MonsterSpec
            {
                name = "Bat", framesPrefix = "Enemies/bat_", effectSprite = "Effects/fangs",
                attack = MonsterAttackKind.Chomp, maxHealth = 25f, moveSpeed = 2.8f, damage = 6f, attackRange = 0.9f,
                colliderSize = new Vector2(0.9f, 0.35f), colliderOffset = new Vector2(0f, 0.44f),
                healthBarHeight = 1.0f, animateWhenIdle = true, spriteFacesRight = false
            };
        }
    }
}
