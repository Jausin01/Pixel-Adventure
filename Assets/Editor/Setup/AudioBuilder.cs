using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Builds the sound banks from the files in Assets/Audio and creates the audio objects of the scene.
    /// Files named "x_1", "x_2" become one id "x" with variations. When a group has an .mp3/.ogg, any .wav
    /// of the same group is treated as a placeholder and ignored.
    /// </summary>
    public static class AudioBuilder
    {
        private const string SfxFolder = "Assets/Audio/Sfx";
        private const string MusicFolder = "Assets/Audio/Music";
        private const string BankFolder = "Assets/Data/Audio";

        // Per-id tweaks; ids not listed use the defaults of SoundBank.Entry.
        private static readonly Dictionary<string, float> Volumes = new Dictionary<string, float>
        {
            { "ui_click", 0.6f }, { "step_grass", 0.15f }, { "step_stone", 0.15f }, { "hit_enemy", 0.7f },
            { "hit_light", 0.6f }, { "hit_heavy", 0.8f }, { "rat", 0.6f }, { "bat", 0.6f }, { "coin_pickup", 0.7f },
            { "item_pickup", 0.7f }, { "victory", 0.8f }, { "death_groan", 0.7f }, { "sword_kill", 0.6f }
        };

        private static readonly Dictionary<string, float> Intervals = new Dictionary<string, float>
        {
            { "step_grass", 0.15f }, { "step_stone", 0.15f }, { "boss_warning", 0.3f }, { "rat", 0.5f }, { "bat", 0.5f },
            { "death_groan", 0.1f }, { "hit_enemy", 0.05f }, { "player_hurt", 0.2f }, { "fireball_explosion", 0.08f },
            { "coin_pickup", 0.05f }, { "ui_click", 0.05f }
        };

        private static readonly HashSet<string> NoPitch = new HashSet<string>
        {
            "quest_complete", "save_bell", "victory", "checkpoint", "skill_learn", "heal", "arena_transform", "ui_error"
        };

        private static readonly HashSet<string> OneShotMusic = new HashSet<string> { "game_over" };

        /// <summary>Rebuilds both banks and adds the audio objects to the scene.</summary>
        public static void Build(GameObject player, ArenaRun run)
        {
            AssetFolderUtil.Ensure(BankFolder);
            SoundBank sfx = BuildBank(SfxFolder, BankFolder + "/Sfx Bank.asset", false);
            SoundBank music = BuildBank(MusicFolder, BankFolder + "/Music Bank.asset", true);

            GameObject root = new GameObject("Audio");
            SfxPlayer sfxPlayer = root.AddComponent<SfxPlayer>();
            sfxPlayer.bank = sfx;
            MusicPlayer musicPlayer = root.AddComponent<MusicPlayer>();
            musicPlayer.bank = music;

            Health health = player.GetComponent<Health>();

            MusicDirector director = root.AddComponent<MusicDirector>();
            director.music = musicPlayer;
            director.run = run;
            director.director = run.director;
            director.dayNight = Object.FindAnyObjectByType<DayNightCycle>();
            director.playerHealth = health;

            SfxEvents events = root.AddComponent<SfxEvents>();
            events.run = run;
            events.director = run.director;
            events.playerHealth = health;

            FootstepSfx steps = player.AddComponent<FootstepSfx>();
            steps.run = run;
        }

        private static SoundBank BuildBank(string folder, string assetPath, bool isMusic)
        {
            SoundBank bank = AssetDatabase.LoadAssetAtPath<SoundBank>(assetPath);
            if (bank == null)
            {
                bank = ScriptableObject.CreateInstance<SoundBank>();
                AssetDatabase.CreateAsset(bank, assetPath);
            }

            Dictionary<string, List<AudioClip>> groups = new Dictionary<string, List<AudioClip>>();
            HashSet<string> hasRealFile = new HashSet<string>();
            string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { folder });
            System.Array.Sort(guids, (a, b) => string.CompareOrdinal(AssetDatabase.GUIDToAssetPath(a), AssetDatabase.GUIDToAssetPath(b)));

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (Path.GetExtension(path).ToLowerInvariant() != ".wav") hasRealFile.Add(GroupOf(path));
            }

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                string group = GroupOf(path);
                bool placeholder = Path.GetExtension(path).ToLowerInvariant() == ".wav";
                if (placeholder && hasRealFile.Contains(group)) continue;

                if (!groups.ContainsKey(group)) groups[group] = new List<AudioClip>();
                groups[group].Add(AssetDatabase.LoadAssetAtPath<AudioClip>(path));
            }

            List<SoundBank.Entry> entries = new List<SoundBank.Entry>();
            foreach (KeyValuePair<string, List<AudioClip>> pair in groups)
            {
                SoundBank.Entry entry = new SoundBank.Entry { id = pair.Key, clips = pair.Value.ToArray() };
                float volume;
                if (Volumes.TryGetValue(pair.Key, out volume)) entry.volume = volume;
                float interval;
                if (Intervals.TryGetValue(pair.Key, out interval)) entry.minInterval = interval;
                if (NoPitch.Contains(pair.Key) || isMusic) entry.pitchVariation = 0f;
                if (pair.Key.StartsWith("step_")) entry.pitchVariation = 0.1f;
                entry.loop = !OneShotMusic.Contains(pair.Key);
                entries.Add(entry);
            }

            bank.entries = entries.ToArray();
            EditorUtility.SetDirty(bank);
            return bank;
        }

        // "Assets/Audio/Sfx/victory_2.mp3" -> "victory". Names with no trailing number keep their whole name.
        private static string GroupOf(string path)
        {
            return Regex.Replace(Path.GetFileNameWithoutExtension(path), @"_\d+$", string.Empty);
        }
    }
}
