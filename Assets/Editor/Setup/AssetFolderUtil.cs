using UnityEditor;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Makes sure folders exist in the project.
    /// </summary>
    public static class AssetFolderUtil
    {
        /// <summary>Creates every missing folder of a path such as "Assets/Data/Items".</summary>
        public static void Ensure(string path)
        {
            string[] parts = path.Split('/');
            string current = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
