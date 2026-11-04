using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates training dummies the player can hit.
    /// </summary>
    public static class TrainingDummyBuilder
    {
        /// <summary>Builds the dummies at fixed positions.</summary>
        public static void Build(Material material)
        {
            CreateDummy("Training Dummy 1", TownLayout.DummyA, material);
            CreateDummy("Training Dummy 2", TownLayout.DummyB, material);
        }

        private static void CreateDummy(string name, Vector2 position, Material material)
        {
            GameObject dummy = new GameObject(name);
            dummy.transform.position = position;

            SpriteRenderer renderer = dummy.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/dummy");
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot; // sort by the base, not the sprite center

            CircleCollider2D collider = dummy.AddComponent<CircleCollider2D>();
            collider.radius = 0.4f;
            collider.offset = new Vector2(0f, 0.45f);

            Health health = dummy.AddComponent<Health>();
            health.maxHealth = 60f;

            dummy.AddComponent<HitFlash>().target = renderer;
            dummy.AddComponent<HitShake>();

            DeathVisibility death = dummy.AddComponent<DeathVisibility>();
            death.renderers = new[] { renderer };
            death.colliders = new Collider2D[] { collider };

            dummy.AddComponent<RespawnTimer>();
            dummy.AddComponent<DamageLogger>();

            WorldHealthBarBuilder.Build(dummy.transform, material, health, new Vector2(0f, 1.2f));
        }
    }
}
