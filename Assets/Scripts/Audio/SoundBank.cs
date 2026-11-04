using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// A set of named sounds. Each id can own several clips; one is picked at random on every play.
    /// Used for both sound effects and music tracks.
    /// </summary>
    [CreateAssetMenu(menuName = "Mobile RPG/Sound Bank")]
    public class SoundBank : ScriptableObject
    {
        /// <summary>One named sound with its variations.</summary>
        [Serializable]
        public class Entry
        {
            [Tooltip("Name used by code, e.g. sword_swing.")]
            public string id;

            [Tooltip("Variations. One is chosen at random.")]
            public AudioClip[] clips;

            [Range(0f, 1f)]
            public float volume = 1f;

            [Tooltip("Random pitch change (+/-) applied to each play.")]
            [Range(0f, 0.5f)]
            public float pitchVariation = 0.05f;

            [Tooltip("Minimum seconds between two plays of this id, to stop stacking.")]
            public float minInterval = 0.04f;

            [Tooltip("Music only: whether the track loops.")]
            public bool loop = true;
        }

        /// <summary>All sounds in the bank.</summary>
        public Entry[] entries = new Entry[0];

        private Dictionary<string, Entry> lookup;

        /// <summary>Finds an entry by id. Returns false when the id is unknown or has no clips.</summary>
        public bool TryGet(string id, out Entry entry)
        {
            if (lookup == null) Build();
            if (!string.IsNullOrEmpty(id) && lookup.TryGetValue(id, out entry)) return entry.clips != null && entry.clips.Length > 0;
            entry = null;
            return false;
        }

        /// <summary>True when the bank has playable clips for the id.</summary>
        public bool Has(string id)
        {
            Entry entry;
            return TryGet(id, out entry);
        }

        private void Build()
        {
            lookup = new Dictionary<string, Entry>();
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i] != null && !string.IsNullOrEmpty(entries[i].id)) lookup[entries[i].id] = entries[i];
            }
        }

        private void OnEnable() { lookup = null; }
    }
}
