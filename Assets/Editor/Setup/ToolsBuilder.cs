using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the axe, pickaxe and shovel and the holder that uses them.
    /// </summary>
    public static class ToolsBuilder
    {
        /// <summary>Builds the tools.</summary>
        public static ToolHolder Build(Material material, GameObject player)
        {
            GameObject holderObject = new GameObject("Tools");
            ToolHolder holder = holderObject.AddComponent<ToolHolder>();
            holder.tools = new[]
            {
                CreateTool(holderObject.transform, material, player, ToolType.Axe, "axe"),
                CreateTool(holderObject.transform, material, player, ToolType.Pickaxe, "pickaxe"),
                CreateTool(holderObject.transform, material, player, ToolType.Shovel, "shovel")
            };
            return holder;
        }

        private static ToolSwing CreateTool(Transform parent, Material material, GameObject player, ToolType type, string spritePrefix)
        {
            GameObject go = new GameObject(type.ToString());
            go.transform.SetParent(parent, false);

            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sharedMaterial = material;
            renderer.sortingOrder = 10;

            ToolSwingVisual visual = go.AddComponent<ToolSwingVisual>();
            visual.frames = SpriteLoader.LoadFrames("Tools/" + spritePrefix + "_", 4);

            ToolSwing swing = go.AddComponent<ToolSwing>();
            swing.tool = type;
            swing.owner = player.transform;
            swing.visual = visual;
            return swing;
        }
    }
}
