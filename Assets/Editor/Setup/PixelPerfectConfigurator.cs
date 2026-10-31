using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Adds and configures the URP Pixel Perfect Camera component. It is added by type name so the
    /// project still compiles if the package is not available.
    /// </summary>
    public static class PixelPerfectConfigurator
    {
        private const string TypeName = "UnityEngine.Rendering.Universal.PixelPerfectCamera, Unity.RenderPipelines.Universal.Runtime";

        /// <summary>Adds the component and sets pixels per unit and reference resolution.</summary>
        public static void Configure(GameObject cameraObject)
        {
            System.Type type = System.Type.GetType(TypeName);
            if (type == null)
            {
                Debug.LogWarning("Pixel Perfect Camera not found. Add it manually to the camera.");
                return;
            }

            Component component = cameraObject.AddComponent(type);
            SerializedObject serialized = new SerializedObject(component);
            SetInt(serialized, "m_AssetsPPU", ProjectConstants.PixelsPerUnit);
            SetInt(serialized, "m_RefResolutionX", ProjectConstants.ReferenceWidth);
            SetInt(serialized, "m_RefResolutionY", ProjectConstants.ReferenceHeight);
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetInt(SerializedObject serialized, string name, int value)
        {
            SerializedProperty property = serialized.FindProperty(name);
            if (property != null) property.intValue = value;
        }
    }
}
