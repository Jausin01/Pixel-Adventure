using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Makes the URP 2D renderer sort sprites by their Y position (sprites lower on the screen draw in front).
    /// URP 2D reads this from the Renderer2D Data asset, not from Graphics Settings.
    /// </summary>
    public static class Renderer2DSortingConfigurator
    {
        private const int CustomAxisMode = 3; // TransparencySortMode.CustomAxis

        /// <summary>Sets Custom Axis (0, 1, 0) on every 2D renderer used by the project's render pipeline assets.</summary>
        public static void Apply()
        {
            List<RenderPipelineAsset> assets = CollectPipelineAssets();
            for (int i = 0; i < assets.Count; i++) ConfigureAsset(assets[i]);
            AssetDatabase.SaveAssets();
        }

        private static List<RenderPipelineAsset> CollectPipelineAssets()
        {
            List<RenderPipelineAsset> assets = new List<RenderPipelineAsset>();
            AddUnique(assets, GraphicsSettings.defaultRenderPipeline);

            int levels = QualitySettings.names.Length;
            for (int i = 0; i < levels; i++) AddUnique(assets, QualitySettings.GetRenderPipelineAssetAt(i));
            return assets;
        }

        private static void AddUnique(List<RenderPipelineAsset> list, RenderPipelineAsset asset)
        {
            if (asset != null && !list.Contains(asset)) list.Add(asset);
        }

        private static void ConfigureAsset(RenderPipelineAsset asset)
        {
            SerializedProperty rendererList = new SerializedObject(asset).FindProperty("m_RendererDataList");
            if (rendererList == null) return;

            for (int i = 0; i < rendererList.arraySize; i++)
            {
                Object rendererData = rendererList.GetArrayElementAtIndex(i).objectReferenceValue;
                if (rendererData != null) ConfigureRendererData(rendererData);
            }
        }

        private static void ConfigureRendererData(Object rendererData)
        {
            SerializedObject serialized = new SerializedObject(rendererData);
            SerializedProperty mode = serialized.FindProperty("m_TransparencySortMode");
            SerializedProperty axis = serialized.FindProperty("m_TransparencySortAxis");

            if (mode == null || axis == null)
            {
                Debug.LogWarning("Could not set Y sorting on " + rendererData.name
                    + ". Set it manually: Renderer2D asset > Transparency Sort Mode = Custom Axis, Axis = (0, 1, 0).");
                return;
            }

            mode.enumValueIndex = CustomAxisMode;
            axis.vector3Value = new Vector3(0f, 1f, 0f);
            serialized.ApplyModifiedProperties();
            EditorUtility.SetDirty(rendererData);
        }
    }
}
