using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates training dummies the player can hit.
    /// </summary>
    public static class EnemyBuilder
    {
        /// <summary>Builds the dummies at fixed positions.</summary>
        public static void Build(Material material)
        {
            CreateDummy("Training Dummy 1", new Vector2(3.5f, 1.5f), material);
            CreateDummy("Training Dummy 2", new Vector2(-3.5f, -1f), material);
        }

        private static void CreateDummy(string name, Vector2 position, Material material)
        {
            GameObject dummy = new GameObject(name);
            dummy.transform.position = position;

            SpriteRenderer renderer = dummy.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/dummy");
            renderer.sharedMaterial = material;

            CircleCollider2D collider = dummy.AddComponent<CircleCollider2D>();
            collider.radius = 0.4f;
            collider.offset = new Vector2(0f, 0.45f);

            dummy.AddComponent<Health>();

            HitFlash flash = dummy.AddComponent<HitFlash>();
            flash.target = renderer;

            dummy.AddComponent<HitShake>();

            DeathVisibility death = dummy.AddComponent<DeathVisibility>();
            death.target = renderer;
            death.colliders = new Collider2D[] { collider };

            dummy.AddComponent<RespawnTimer>();
            dummy.AddComponent<DamageLogger>();
        }
    }
}
