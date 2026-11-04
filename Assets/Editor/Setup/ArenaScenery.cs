using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Places the props of one arena: solid ones the player and enemies collide with, and decoration they walk over.
    /// Random props keep away from the player's arrival point and from each other.
    /// </summary>
    public class ArenaScenery
    {
        /// <summary>A glow attached to a prop.</summary>
        public class PropLight
        {
            public Color color = Color.white;
            public float intensity = 1f;
            public float radius = 4f;
            public float flicker;

                public float height = 0.5f;
        }

        private readonly Transform root;
        private readonly Material material;
        private readonly System.Random random;
        private readonly Vector2 halfSize;
        private readonly Vector2 spawn;
        private readonly List<Vector2> taken = new List<Vector2>();

        /// <param name="root">Parent object; positions are relative to it.</param>
        /// <param name="size">Size of the play area.</param>
        /// <param name="spawn">Where the player arrives; kept clear.</param>
        public ArenaScenery(Transform root, Material material, Vector2 size, Vector2 spawn, int seed)
        {
            this.root = root;
            this.material = material;
            this.spawn = spawn;
            halfSize = size / 2f - new Vector2(1.5f, 1.5f);
            random = new System.Random(seed);
        }

        /// <summary>Places one solid prop at a fixed position. The collider sits on the prop's base.</summary>
        public GameObject Solid(string sprite, Vector2 position, Vector2 colliderSize, PropLight glow = null)
        {
            GameObject prop = Place(sprite, position, 0);
            AddGlow(prop, glow);
            BoxCollider2D collider = prop.AddComponent<BoxCollider2D>();
            collider.size = colliderSize;
            collider.offset = new Vector2(0f, colliderSize.y / 2f);
            taken.Add(position);
            return prop;
        }

        /// <summary>Places one decoration at a fixed position.</summary>
        public GameObject Decor(string sprite, Vector2 position, PropLight glow = null)
        {
            GameObject prop = Place(sprite, position, -90);
            AddGlow(prop, glow);
            return prop;
        }

        /// <summary>Scatters solid props at random free spots.</summary>
        /// <param name="spacing">Smallest distance between two props.</param>
        public void ScatterSolid(string sprite, int count, Vector2 colliderSize, float spacing, PropLight glow = null)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 spot;
                if (FindSpot(spacing, out spot)) Solid(sprite, spot, colliderSize, glow);
            }
        }

        /// <summary>Scatters decorations at random free spots. Decorations may be close to each other.</summary>
        public void ScatterDecor(string[] sprites, int count, PropLight glow = null)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 spot;
                if (FindSpot(0f, out spot)) AddGlow(Place(sprites[random.Next(sprites.Length)], spot, -90), glow);
            }
        }

        private static void AddGlow(GameObject prop, PropLight glow)
        {
            if (glow == null) return;
            LightFactory.Point("Glow", prop.transform, new Vector2(0f, glow.height), glow.color, glow.intensity, glow.radius, glow.flicker);
        }

        private bool FindSpot(float spacing, out Vector2 spot)
        {
            for (int attempt = 0; attempt < 40; attempt++)
            {
                spot = new Vector2((float)(random.NextDouble() * 2 - 1) * halfSize.x, (float)(random.NextDouble() * 2 - 1) * halfSize.y);
                if (Vector2.Distance(spot, spawn) < 3.5f) continue;
                if (IsTooClose(spot, spacing)) continue;
                return true;
            }
            spot = Vector2.zero;
            return false;
        }

        private bool IsTooClose(Vector2 spot, float spacing)
        {
            for (int i = 0; i < taken.Count; i++)
            {
                if (Vector2.Distance(spot, taken[i]) < spacing) return true;
            }
            return false;
        }

        private GameObject Place(string sprite, Vector2 position, int sortingOrder)
        {
            GameObject go = new GameObject(System.IO.Path.GetFileName(sprite));
            go.transform.SetParent(root, false);
            go.transform.localPosition = position;

            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load(sprite);
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;
            renderer.sortingOrder = sortingOrder;
            return go;
        }
    }
}
