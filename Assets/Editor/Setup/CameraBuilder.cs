using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the main camera, follows the player and enables pixel-perfect rendering.
    /// </summary>
    public static class CameraBuilder
    {
        /// <summary>Builds the camera.</summary>
        public static void Build(Transform target)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = ProjectConstants.ReferenceHeight / 2f / ProjectConstants.PixelsPerUnit;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;

            cameraObject.AddComponent<CameraFollow>().target = target;
            PixelPerfectConfigurator.Configure(cameraObject);
        }
    }
}
