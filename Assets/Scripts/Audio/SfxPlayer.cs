using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Plays sound effects from a <see cref="SoundBank"/> using a small pool of audio sources.
    /// Avoids repeating the same variation twice in a row and limits how fast one id can retrigger.
    /// </summary>
    public class SfxPlayer : MonoBehaviour
    {
        [Tooltip("Bank with every sound effect.")]
        public SoundBank bank;

        [Tooltip("Number of sounds that can overlap.")]
        public int voices = 10;

        [Range(0f, 1f)]
        public float masterVolume = 1f;

        private AudioSource[] sources;
        private int next;
        private readonly Dictionary<string, float> lastTime = new Dictionary<string, float>();
        private readonly Dictionary<string, int> lastIndex = new Dictionary<string, int>();

        private void Awake()
        {
            sources = new AudioSource[voices];
            for (int i = 0; i < voices; i++)
            {
                sources[i] = gameObject.AddComponent<AudioSource>();
                sources[i].playOnAwake = false;
                sources[i].spatialBlend = 0f;
            }
        }

        private void OnEnable() { Sfx.Player = this; }

        private void OnDisable() { if (Sfx.Player == this) Sfx.Player = null; }

        /// <summary>Plays the sound with the given id.</summary>
        public void Play(string id)
        {
            SoundBank.Entry entry;
            if (bank == null || !bank.TryGet(id, out entry)) return;

            float last;
            if (lastTime.TryGetValue(id, out last) && Time.unscaledTime - last < entry.minInterval) return;
            lastTime[id] = Time.unscaledTime;

            int index = Random.Range(0, entry.clips.Length);
            int previous;
            if (entry.clips.Length > 1 && lastIndex.TryGetValue(id, out previous) && previous == index)
            {
                index = (index + 1) % entry.clips.Length;
            }
            lastIndex[id] = index;

            AudioSource source = sources[next];
            next = (next + 1) % sources.Length;
            source.pitch = 1f + Random.Range(-entry.pitchVariation, entry.pitchVariation);
            source.PlayOneShot(entry.clips[index], entry.volume * masterVolume * VolumeSettings.Sfx);
        }
    }
}
