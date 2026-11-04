using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Builds the town's static scenery: the tiled ground (grass, plaza, roads), the invisible walls,
    /// the buildings and the decoration. Walkable objects (NPCs, trees, ores, dummies) are made by their own builders.
    /// </summary>
    public static class TownWorldBuilder
    {
        private const string TileFolder = "Assets/Data/Tiles";
        private const float BuildingWidth = 4f;
        private const float BuildingHeight = 3.25f;

        private static Material material;
        private static Transform root;

        /// <summary>Builds the ground, walls, buildings and decoration under one "Town" object.</summary>
        public static Light2D[] Build(Material spriteMaterial)
        {
            material = spriteMaterial;
            root = new GameObject("Town").transform;

            BuildGround();
            WorldBuilder.CreateWalls(root, TownLayout.Size);
            BuildBuildings();
            BuildYardFence();
            Light2D[] lamps = BuildProps();
            BuildScatter();
            LightFactory.Point("Forge Glow", root, TownLayout.Forge + new Vector2(0f, 0.9f), new Color(1f, 0.5f, 0.2f), 1.1f, 4.5f, 0.2f);
            return lamps;
        }

        // ---------------------------------------------------------------- ground

        private static void BuildGround()
        {
            GameObject gridObject = new GameObject("Ground");
            gridObject.transform.SetParent(root, false);
            gridObject.AddComponent<Grid>();

            GameObject mapObject = new GameObject("Tilemap");
            mapObject.transform.SetParent(gridObject.transform, false);
            Tilemap map = mapObject.AddComponent<Tilemap>();
            TilemapRenderer renderer = mapObject.AddComponent<TilemapRenderer>();
            renderer.sharedMaterial = material;
            renderer.sortingOrder = -100;

            Tile[] grass = { GetTile("grass_0"), GetTile("grass_1"), GetTile("grass_2"), GetTile("grass_3") };
            Tile[] plaza = { GetTile("plaza_0"), GetTile("plaza_1") };
            Tile plazaRim = GetTile("plaza_rim");
            Tile[] road = { GetTile("road_0"), GetTile("road_1") };

            int halfX = Mathf.CeilToInt(TownLayout.Size.x / 2f) + TownLayout.GrassMargin;
            int halfY = Mathf.CeilToInt(TownLayout.Size.y / 2f) + TownLayout.GrassMargin;
            System.Random random = new System.Random(11);

            for (int x = -halfX; x < halfX; x++)
            {
                for (int y = -halfY; y < halfY; y++)
                {
                    Vector2 center = new Vector2(x + 0.5f, y + 0.5f);
                    Tile tile;
                    if (IsPlaza(center)) tile = IsPlazaRim(center) ? plazaRim : plaza[random.Next(plaza.Length)];
                    else if (IsRoad(center)) tile = road[random.Next(road.Length)];
                    else tile = grass[random.Next(grass.Length)];
                    map.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        private static bool IsPlaza(Vector2 p)
        {
            return Vector2.Distance(p, TownLayout.PlazaCenter) <= TownLayout.PlazaRadius;
        }

        private static bool IsPlazaRim(Vector2 p)
        {
            return Vector2.Distance(p, TownLayout.PlazaCenter) > TownLayout.PlazaRadius - 0.9f;
        }

        private static bool IsRoad(Vector2 p)
        {
            float halfX = TownLayout.Size.x / 2f;
            float halfY = TownLayout.Size.y / 2f;
            if (Mathf.Abs(p.x) > halfX || Mathf.Abs(p.y) > halfY) return false;

            bool northSouth = p.x > TownLayout.RoadX.x && p.x < TownLayout.RoadX.y;
            bool eastWest = p.y > TownLayout.RoadY.x && p.y < TownLayout.RoadY.y;
            return northSouth || eastWest;
        }

        private static Tile GetTile(string name)
        {
            return TileAssetFactory.Get("Tiles/" + name);
        }

        // ---------------------------------------------------------------- buildings and props

        private static void BuildBuildings()
        {
            Building("Guild Hall", "guild_hall", TownLayout.GuildHall);
            Building("Dojo", "dojo", TownLayout.Dojo);
            Building("Market", "market", TownLayout.Market);
            Building("Forge", "forge", TownLayout.Forge);
            Building("Storehouse", "storehouse", TownLayout.Storehouse);
        }

        private static void Building(string name, string sprite, Vector2 position)
        {
            GameObject building = Place(name, "Town/" + sprite, position, 0);
            BoxCollider2D collider = building.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(BuildingWidth - 0.2f, BuildingHeight - 0.25f);
            collider.offset = new Vector2(0f, (BuildingHeight - 0.25f) / 2f);
        }

        private static void BuildYardFence()
        {
            Rect yard = TownLayout.Yard;
            for (float x = yard.xMin + 0.5f; x < yard.xMax; x += 1f)
            {
                Fence("Fence", "fence_h", new Vector2(x, yard.yMax), new Vector2(1f, 0.3f), new Vector2(0f, 0.15f));
                Fence("Fence", "fence_h", new Vector2(x, yard.yMin), new Vector2(1f, 0.3f), new Vector2(0f, 0.15f));
            }

            // East side is closed; the west side keeps a two-unit gap as the entrance.
            for (float y = yard.yMin + 1f; y < yard.yMax - 1f; y += 1f)
            {
                Fence("Fence", "fence_v", new Vector2(yard.xMax - 0.25f, y), new Vector2(0.3f, 1f), new Vector2(0f, 0.5f));
            }
            Fence("Fence", "fence_v", new Vector2(yard.xMin + 0.25f, yard.yMax - 1f), new Vector2(0.3f, 1f), new Vector2(0f, 0.5f));
            Fence("Fence", "fence_v", new Vector2(yard.xMin + 0.25f, yard.yMin + 1f), new Vector2(0.3f, 1f), new Vector2(0f, 0.5f));
        }

        private static void Fence(string name, string sprite, Vector2 position, Vector2 size, Vector2 offset)
        {
            GameObject fence = Place(name, "Town/" + sprite, position, 0);
            BoxCollider2D collider = fence.AddComponent<BoxCollider2D>();
            collider.size = size;
            collider.offset = offset;
        }

        private static Light2D[] BuildProps()
        {
            List<Light2D> lamps = new List<Light2D>();
            Prop("Anvil", "anvil", TownLayout.Anvil, new Vector2(0.7f, 0.35f));

            foreach (Vector2 position in TownLayout.Lamps)
            {
                GameObject lamp = Prop("Lamp", "lamp", position, new Vector2(0.3f, 0.2f));
                lamps.Add(LightFactory.Point("Lamp Light", lamp.transform, new Vector2(0f, 1.7f), new Color(1f, 0.85f, 0.55f), 1f, 4.5f, 0.05f));
            }

            Prop("Barrel", "barrel", new Vector2(7.4f, 5.6f), new Vector2(0.6f, 0.4f));
            Prop("Crate", "crate", new Vector2(8.0f, 5.0f), new Vector2(0.7f, 0.4f));
            Prop("Barrel", "barrel", new Vector2(-9.2f, -4.4f), new Vector2(0.6f, 0.4f));
            Prop("Crate", "crate", new Vector2(-9.8f, -5.4f), new Vector2(0.7f, 0.4f));
            Prop("Crate", "crate", new Vector2(-15.4f, -4.6f), new Vector2(0.7f, 0.4f));
            Prop("Bush", "bush_0", new Vector2(-8.5f, 6.2f), new Vector2(0.7f, 0.4f));
            Prop("Bush", "bush_1", new Vector2(-1.9f, 6.2f), new Vector2(0.7f, 0.4f));
            Prop("Bush", "bush_1", new Vector2(1.9f, 6.2f), new Vector2(0.7f, 0.4f));
            Prop("Bush", "bush_0", new Vector2(8.5f, 6.2f), new Vector2(0.7f, 0.4f));
            return lamps.ToArray();
        }

        private static GameObject Prop(string name, string sprite, Vector2 position, Vector2 colliderSize)
        {
            GameObject prop = Place(name, "Town/" + sprite, position, 0);
            BoxCollider2D collider = prop.AddComponent<BoxCollider2D>();
            collider.size = colliderSize;
            collider.offset = new Vector2(0f, colliderSize.y / 2f);
            return prop;
        }

        // ---------------------------------------------------------------- random decoration

        private static void BuildScatter()
        {
            System.Random random = new System.Random(7);
            Transform parent = new GameObject("Decoration").transform;
            parent.SetParent(root, false);

            string[] flowerSprites = { "flowers_red", "flowers_yellow", "flowers_blue" };
            for (int i = 0; i < 110; i++)
            {
                Vector2 spot;
                if (!TryFindFreeSpot(random, 0.6f, out spot)) continue;
                GameObject flower = Place("Flowers", "Town/" + flowerSprites[random.Next(flowerSprites.Length)], spot, -90);
                flower.transform.SetParent(parent, true);
            }

            for (int i = 0; i < 16; i++)
            {
                Vector2 spot;
                if (!TryFindFreeSpot(random, 1.2f, out spot)) continue;
                bool isRock = i % 3 == 0;
                GameObject prop = Place(isRock ? "Rock" : "Bush", "Town/" + (isRock ? "rock" : (i % 2 == 0 ? "bush_0" : "bush_1")), spot, 0);
                BoxCollider2D collider = prop.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(0.7f, 0.4f);
                collider.offset = new Vector2(0f, 0.2f);
                prop.transform.SetParent(parent, true);
            }
        }

        private static bool TryFindFreeSpot(System.Random random, float clearance, out Vector2 spot)
        {
            float halfX = TownLayout.Size.x / 2f - 0.7f;
            float halfY = TownLayout.Size.y / 2f - 0.7f;
            for (int attempt = 0; attempt < 60; attempt++)
            {
                spot = new Vector2((float)(random.NextDouble() * 2 - 1) * halfX, (float)(random.NextDouble() * 2 - 1) * halfY);
                if (IsOpenGrass(spot, clearance)) return true;
            }
            spot = Vector2.zero;
            return false;
        }

        /// <summary>True when nothing else (path, building, NPC, tree, ore...) is close to the point.</summary>
        private static bool IsOpenGrass(Vector2 p, float clearance)
        {
            if (Vector2.Distance(p, TownLayout.PlazaCenter) < TownLayout.PlazaRadius + clearance) return false;
            if (p.x > TownLayout.RoadX.x - clearance && p.x < TownLayout.RoadX.y + clearance) return false;
            if (p.y > TownLayout.RoadY.x - clearance && p.y < TownLayout.RoadY.y + clearance) return false;

            foreach (Rect rect in BlockedAreas())
            {
                if (new Rect(rect.x - clearance, rect.y - clearance, rect.width + clearance * 2f, rect.height + clearance * 2f).Contains(p)) return false;
            }
            foreach (Vector2 tree in TownLayout.Trees)
            {
                if (Vector2.Distance(p, tree) < 1.5f + clearance) return false;
            }
            return true;
        }

        private static IEnumerable<Rect> BlockedAreas()
        {
            yield return BuildingArea(TownLayout.GuildHall);
            yield return BuildingArea(TownLayout.Dojo);
            yield return BuildingArea(TownLayout.Market);
            yield return BuildingArea(TownLayout.Forge);
            yield return BuildingArea(TownLayout.Storehouse);
            yield return TownLayout.Yard;

            Vector2 first = TownLayout.OrePosition(0);
            Vector2 last = TownLayout.OrePosition(9);
            yield return new Rect(first.x - 0.8f, last.y - 0.3f, last.x - first.x + 1.6f, first.y - last.y + 1.8f);
            yield return new Rect(TownLayout.Stash.x - 1.2f, TownLayout.Stash.y - 1f, 2.4f, 2f);
            yield return new Rect(-16f, 4.4f, 32f, 1.8f); // the doorsteps with the NPCs in front of the shops
        }

        private static Rect BuildingArea(Vector2 basePosition)
        {
            return new Rect(basePosition.x - BuildingWidth / 2f, basePosition.y - 0.4f, BuildingWidth, BuildingHeight + 0.4f);
        }

        // ---------------------------------------------------------------- helpers

        /// <summary>Creates a sprite object sorted by its base (bottom pivot) under the town root.</summary>
        private static GameObject Place(string name, string sprite, Vector2 position, int sortingOrder)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(root, false);
            go.transform.position = position;

            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load(sprite);
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;
            renderer.sortingOrder = sortingOrder;
            return go;
        }
    }
}
