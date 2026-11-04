using System.IO;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Reads and writes the save file (JSON in the persistent data folder).
    /// </summary>
    public static class SaveFile
    {
        private const string FileName = "save.json";

        /// <summary>Full path of the save file.</summary>
        public static string Path { get { return System.IO.Path.Combine(Application.persistentDataPath, FileName); } }

        /// <summary>True when a save file exists.</summary>
        public static bool Exists { get { return File.Exists(Path); } }

        /// <summary>Reads the save, or returns null when there is none or it cannot be read.</summary>
        public static SaveData Read()
        {
            if (!Exists) return null;

            try { return JsonUtility.FromJson<SaveData>(File.ReadAllText(Path)); }
            catch (System.Exception error)
            {
                Debug.LogWarning("Could not read the save file: " + error.Message);
                return null;
            }
        }

        /// <summary>Writes the save. A temporary file is written first so a crash cannot leave a broken save.</summary>
        public static void Write(SaveData data)
        {
            string temp = Path + ".tmp";
            File.WriteAllText(temp, JsonUtility.ToJson(data, true));
            if (File.Exists(Path)) File.Delete(Path);
            File.Move(temp, Path);
        }

        /// <summary>Deletes the save file, if any.</summary>
        public static void Delete()
        {
            if (Exists) File.Delete(Path);
        }
    }
}
